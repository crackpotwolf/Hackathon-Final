using AutoMapper;
using Data.Attributes;
using Data.Extensions;
using Data.Interfaces.Repositories;
using Data.Models.DB.Project;
using Data.Models.Services;
using Data.Services.Account;
using Data_Path.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Options;
using Search_Data.Search;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Accelerator.Controllers.API.v1.Projects
{
    /// <summary>
    /// Контроллер заявок
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [DisplayName("project")]
    [SetRoute]
#if RELEASE
    [Authorize]
#endif
    public class ProjectController : ControllerBase
    {
        protected IBaseEntityRepository<Project> _projectsRepository;

        protected readonly EmailService _emailService;
        private readonly ILogger<IndexModel> _logger;
        protected readonly UserManager _userManager;
        private readonly PathConfig _pathConfig;
        protected readonly IMapper _mapper;

        public ProjectController(
            IBaseEntityRepository<Project> projectsRepository,
            IOptions<PathConfig> pathConfig,
            ILogger<IndexModel> logger,
            EmailService emailService,
            UserManager userManager,
            IMapper mapper)
        {
            _projectsRepository = projectsRepository;
            _pathConfig = pathConfig.Value;
            _emailService = emailService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("all")]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _projectsRepository.GetListQuery().ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        /// <summary>
        /// Для загрузки проектов в бд одним объектом
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("project-creation")]
        [SwaggerResponse(200)]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public IActionResult Creation(List<Project> data)
        {
            var search = new WordSearch(_pathConfig.DocumentsIndexes);

            try
            {
                var res = _projectsRepository.AddRange(data);
                res.ToList().ForEach(p => p.AddSearchableObjectToIndexSeparately(p.Guid, search));
                search.CommitChanges();

                return Ok(res.Count(p => p.Guid != Guid.Empty));
            }
            catch (Exception ex)
            {
                search.DiscardChanges();
                return StatusCode(500, ex);
            }
        }
    }
}