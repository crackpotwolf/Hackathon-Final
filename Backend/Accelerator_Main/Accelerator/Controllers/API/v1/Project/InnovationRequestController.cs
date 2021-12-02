using AutoMapper;
using Data.Attributes;
using Data.Extensions;
using Data.Interfaces.Repositories;
using Data.Models.DB;
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
    [DisplayName("innovation")]
    [SetRoute]
    public class InnovationRequestController : ControllerBase
    {
        protected IBaseEntityRepository<InnovationRequest> _requestsRepository;

        protected readonly EmailService _emailService;
        private readonly ILogger<IndexModel> _logger;
        protected readonly UserManager _userManager;
        private readonly PathConfig _pathConfig;
        protected readonly IMapper _mapper;

        public InnovationRequestController(
            IBaseEntityRepository<InnovationRequest> requestsRepository,
            IOptions<PathConfig> pathConfig,
            ILogger<IndexModel> logger,
            EmailService emailService,
            UserManager userManager,
            IMapper mapper)
        {
            _requestsRepository = requestsRepository;
            _pathConfig = pathConfig.Value;
            _emailService = emailService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public async Task<IActionResult> Get(Guid guid)
        {
            try
            {
                return Ok(_requestsRepository.GetListQuery().FirstOrDefault(p=>p.Guid==guid));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("all")]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _requestsRepository.GetListQuery().ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        /// <summary>
        /// Для загрузки заявки на инновацию
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public IActionResult Creation(InnovationRequest data)
        {
            try
            {
                var res = _requestsRepository.Add(data);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Для обновления заявки на инновацию
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut]
        [SwaggerResponse(200)]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public IActionResult Update(InnovationRequest data)
        {
            try
            {
                var res = _requestsRepository.Update(data);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Для удаления заявки на инновацию
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerResponse(200)]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public IActionResult Delete(Guid data)
        {
            try
            {
                var request = _requestsRepository.GetListQuery().FirstOrDefault(p => p.Guid == data);
                if (request == null)
                    return NotFound();
                _requestsRepository.Delete(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}