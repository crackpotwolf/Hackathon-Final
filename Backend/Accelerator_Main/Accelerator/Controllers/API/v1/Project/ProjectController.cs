using AutoMapper;
using Data;
using Data.Attributes;
using Data.Extensions;
using Data.Interfaces.Repositories;
using Data.Models.DB.Project;
using Data.Models.Services;
using Data.Services.Account;
using Data_Path.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class ProjectController : ControllerBase
    {
        protected IBaseEntityRepository<Project> _projectsRepository;
        protected IBaseEntityRepository<Company> _companiesRepository;
        protected IBaseEntityRepository<Recomendation> _recomendationsRepository;

        protected readonly EmailService _emailService;
        private readonly ILogger<IndexModel> _logger;
        protected readonly UserManager _userManager;
        private readonly PathConfig _pathConfig;
        protected readonly IMapper _mapper;
        protected readonly AcceleratorContext _db;

        public ProjectController(
            AcceleratorContext db,
            IBaseEntityRepository<Project> projectsRepository,
            IBaseEntityRepository<Company> companiesRepository,
            IBaseEntityRepository<Recomendation> recomendationsRepository,
            IOptions<PathConfig> pathConfig,
            ILogger<IndexModel> logger,
            EmailService emailService,
            UserManager userManager,
            IMapper mapper)
        {
            _projectsRepository = projectsRepository;
            _companiesRepository = companiesRepository;
            _recomendationsRepository = recomendationsRepository;
            _pathConfig = pathConfig.Value;
            _emailService = emailService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public async Task<IActionResult> Get(Guid guid)
        {
            try
            {
                return Ok(_projectsRepository.GetListQuery()
                    .Include(p => p.Activities)
                    .Include(p => p.Budget)
                    .Include(p => p.Effects)
                    .Include(p => p.Materials)
                    .Include(p => p.Meetings)
                    .Include(p => p.Order)
                    .Include(p => p.Stages)
                    .Include(p => p.Statuses)
                    .Include(p => p.Teams)
                    .FirstOrDefault(p => p.Guid == guid));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("lite")]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public async Task<IActionResult> GetLite(Guid guid)
        {
            try
            {
                return Ok(_projectsRepository.GetListQuery()
                    .Include(p => p.Order)
                    .FirstOrDefault(p => p.Guid == guid));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("activities")]
        public async Task<IActionResult> GetActivities(Guid projectGuid) => Ok(_db.Activities.Where(p => p.ProjectGuid == projectGuid));

        [HttpGet("budget")]
        public async Task<IActionResult> GetBudgets(Guid projectGuid) => Ok(_db.Budgets.Where(p => p.ProjectGuid == projectGuid));

        [HttpGet("effects")]
        public async Task<IActionResult> GetEffects(Guid projectGuid) => Ok(_db.Effects.Where(p => p.ProjectGuid == projectGuid));

        [HttpGet("materials")]
        public async Task<IActionResult> GetMaterials(Guid projectGuid) => Ok(_db.Materials.Where(p => p.ProjectGuid == projectGuid));

        [HttpGet("meetings")]
        public async Task<IActionResult> GetMeetings(Guid projectGuid) => Ok(_db.Meetings.Where(p => p.ProjectGuid == projectGuid));

        [HttpGet("stages")]
        public async Task<IActionResult> GetStages(Guid projectGuid) => Ok(_db.Stages.Where(p => p.ProjectGuid == projectGuid));

        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatuses(Guid projectGuid) => Ok(_db.Statuses.Where(p => p.ProjectGuid == projectGuid));

        [HttpGet("teams")]
        public async Task<IActionResult> GetTeams(Guid projectGuid) => Ok(_db.Teams.Where(p => p.ProjectGuid == projectGuid));


        [HttpGet("all")]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _projectsRepository.GetListQuery().Include(p => p.Order).ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("get-companies/{guid}")]
        public IActionResult GetCompanies(Guid guid)
        {
            try
            {
                return Ok(_companiesRepository.GetListQuery().Where(p => p.ProjectGuid == guid).DistinctBy(p => p.Name).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("post-companies")]
        public IActionResult PostCompanies(Company data)
        {
            try
            {
                return Ok(_companiesRepository.Add(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("get-recomendations/{guid}")]
        public IActionResult GetRecomendations(Guid guid)
        {
            try
            {
                return Ok(_recomendationsRepository.GetListQuery().Where(p => p.ProjectGuid == guid).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("post-recomendations/{guid}")]
        public IActionResult PostRecomendations(Recomendation data)
        {
            try
            {
                return Ok(_recomendationsRepository.Add(data));
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
        [DisableRequestSizeLimit]
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