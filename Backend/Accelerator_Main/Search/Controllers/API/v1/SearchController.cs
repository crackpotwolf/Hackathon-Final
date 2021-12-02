using AutoMapper;
using Data.Attributes;
using Data.Extensions;
using Data.Interfaces.Repositories;
using Data.Models.DB.Project;
using Data.Models.Services;
using Data.Services.Account;
using Data_Path.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Options;
using Search_Data.Models;
using Search_Data.Search;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Search.Controllers.API.v1
{
    /// <summary>
    /// Поиск
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [DisplayName("search")]
    [SetRoute]
    public class SearchController : ControllerBase
    {
        protected IBaseEntityRepository<Project> _projectRepository;
        protected IBaseEntityRepository<Order> _ordersRepository;

        private readonly ILogger<IndexModel> _logger;
        private readonly PathConfig _pathConfig;

        /// <inheritdoc />
        public SearchController(ILogger<IndexModel> logger,
            IBaseEntityRepository<Project> projectRepository,
            IBaseEntityRepository<Order> ordersRepository,
            IOptions<PathConfig> pathConfig)
        {
            _projectRepository = projectRepository;
            _ordersRepository = ordersRepository;
            _pathConfig = pathConfig.Value;
            _logger = logger;
        }

        ///// <summary>
        ///// Список фильтров
        ///// </summary>
        ///// <param name="inputText">Текст для поиска</param>
        ///// <returns>Название файлов с совпадениями в порядке релевантности</returns>
        //[HttpGet("guids/{inputText}")]
        //[DisableRequestSizeLimit]
        //[Produces("application/json")]
        //[SwaggerResponse(200, "Название файлов с совпадениями в порядке релевантности", typeof(Dictionary<Guid, float>))]
        //[ProducesResponseType(typeof(Exception), 400)]
        //public IActionResult Filters()
        //{
        //    try
        //    {
        //        var stages = _ordersRepository.GetListQuery().Select(p=>p.Stage).Distinct().ToList();
        //        var certification= _ordersRepository.GetListQuery().Select(p => p.Sertification).Distinct().ToList();
        //        var peopleCount = _ordersRepository.GetListQuery().Select(p => p.Sertification).Distinct().ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex);
        //    }
        //}

        /// <summary>
        /// Поиск объектов по ключевым словам
        /// </summary>
        /// <param name="inputText">Текст для поиска</param>
        /// <returns>Название файлов с совпадениями в порядке релевантности</returns>
        [HttpGet("guids/{inputText}")]
        [DisableRequestSizeLimit]
        [Produces("application/json")]
        [SwaggerResponse(200, "Название файлов с совпадениями в порядке релевантности", typeof(Dictionary<Guid, float>))]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult SearchGuidsByText(string inputText)
        {
            try
            {
                // Для замера времени
                DateTime timeStart = DateTime.UtcNow;

                // Инициализация
                var search = new WordSearch(_pathConfig.DocumentsIndexes, typeof(Project).GetSerachableFieldsNames());

                // Поиск
                _logger.LogInformation($"Поиск. {inputText}");

                var result = search.Search(inputText);

                // Результаты
                _logger.LogInformation($"Запрос: '{result.Query}' всего: {result.TotalHits}. {inputText}");

                Dictionary<Guid, float> results = new Dictionary<Guid, float>();

                foreach (var item in result.Hits)
                {
                    results.Add(item.Guid, item.Score);

                    _logger.LogInformation($"{item.Guid} --- Score: {item.Score}. {inputText}");
                }

                _logger.LogInformation($"Поиск завершен за: {(DateTime.UtcNow - timeStart).TotalSeconds} секунд. {inputText}");

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось выполнить поиск по документам. " +
                    $"{inputText} Ошибка: {ex.Message}");

                return StatusCode(500, "Не удалось выполнить поиск по документам");
            }
        }

        /// <summary>
        /// Поиск объектов по ключевым словам
        /// </summary>
        /// <param name="inputText">Текст для поиска</param>
        /// <returns>Название файлов с совпадениями в порядке релевантности</returns>
        [HttpGet("objects/{inputText}")]
        [DisableRequestSizeLimit]
        [Produces("application/json")]
        [SwaggerResponse(200, "Объекты с совпадениями в порядке релевантности", typeof(List<Project>))]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult SearchObjectsByText(string inputText)
        {
            try
            {
                // Для замера времени
                DateTime timeStart = DateTime.UtcNow;

                // Инициализация
                var search = new WordSearch(_pathConfig.DocumentsIndexes, typeof(Project).GetSerachableFieldsNames());

                // Поиск
                _logger.LogInformation($"Поиск. {inputText}");

                var result = search.Search(inputText);

                // Результаты
                _logger.LogInformation($"Запрос: '{result.Query}' всего: {result.TotalHits}. {inputText}");

                var results = _projectRepository.GetListQuery().Where(p => result.Hits.Select(t => t.Guid).Contains(p.Guid))
                    .Include(p => p.Order)
                    .ToList();

                _logger.LogInformation($"Поиск завершен за: {(DateTime.UtcNow - timeStart).TotalSeconds} секунд. {inputText}");

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось выполнить поиск по документам. " +
                    $"{inputText} Ошибка: {ex.Message}");

                return StatusCode(500, "Не удалось выполнить поиск по документам");
            }
        }

        /// <summary>
        /// Поиск объектов по ключевым словам
        /// </summary>
        /// <param name="input">Текст для поиска</param>
        /// <returns>Название файлов с совпадениями в порядке релевантности</returns>
        [HttpPost("extend-objects")]
        [DisableRequestSizeLimit]
        [Produces("application/json")]
        [SwaggerResponse(200, "Объекты с совпадениями в порядке релевантности", typeof(List<Project>))]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult ExtendedSearchByText(ExtendSearch input)
        {
            try
            {
                // Для замера времени
                DateTime timeStart = DateTime.UtcNow;

                // Инициализация
                var results = new List<Guid>();

                var teamGuids = new List<Guid>();
                if (!String.IsNullOrEmpty(input.Team))
                {
                    var search = new WordSearch(_pathConfig.DocumentsIndexes, new List<string>() { "OrderTeam" }.ToArray());

                    // Поиск
                    _logger.LogInformation($"Поиск. {input.Team}");

                    var result = search.Search(input.Team);
                    teamGuids = result.Hits.Select(t => t.Guid).ToList();
                }

                var projectGuids = new List<Guid>();
                if (!String.IsNullOrEmpty(input.Project))
                {
                    var search = new WordSearch(_pathConfig.DocumentsIndexes, new List<string>() { "Name" }.ToArray());

                    // Поиск
                    _logger.LogInformation($"Поиск. {input.Project}");

                    var result = search.Search(input.Project);
                    projectGuids= result.Hits.Select(t => t.Guid).ToList();
                }

                var descrGuids = new List<Guid>();

                if (!String.IsNullOrEmpty(input.Description))
                {
                    var search = new WordSearch(_pathConfig.DocumentsIndexes, typeof(Project).GetSerachableFieldsNames().ToList().Except(new List<string>() { "Name", "OrderTeam" }).ToArray());

                    // Поиск
                    _logger.LogInformation($"Поиск. {input.Description}");

                    var result = search.Search(input.Description);
                    descrGuids = result.Hits.Select(t => t.Guid).ToList();
                }
                if (!String.IsNullOrEmpty(input.Team) && !String.IsNullOrEmpty(input.Project) && !String.IsNullOrEmpty(input.Description))
                    results = teamGuids.Intersect(projectGuids).Intersect(descrGuids).Distinct().ToList();
                else if (String.IsNullOrEmpty(input.Team) && !String.IsNullOrEmpty(input.Project) && !String.IsNullOrEmpty(input.Description))
                    results = projectGuids.Intersect(descrGuids).Distinct().ToList();
                else if (String.IsNullOrEmpty(input.Team) && String.IsNullOrEmpty(input.Project) && !String.IsNullOrEmpty(input.Description))
                    results = descrGuids.Distinct().ToList();
                else if (!String.IsNullOrEmpty(input.Team) && String.IsNullOrEmpty(input.Project) && !String.IsNullOrEmpty(input.Description))
                    results = teamGuids.Intersect(descrGuids).Distinct().ToList();
                else if (!String.IsNullOrEmpty(input.Team) && !String.IsNullOrEmpty(input.Project) && String.IsNullOrEmpty(input.Description))
                    results = teamGuids.Intersect(projectGuids).Distinct().ToList();
                else if (!String.IsNullOrEmpty(input.Team) && String.IsNullOrEmpty(input.Project) && String.IsNullOrEmpty(input.Description))
                    results = teamGuids.Distinct().ToList();
                else if (String.IsNullOrEmpty(input.Team) && !String.IsNullOrEmpty(input.Project) && String.IsNullOrEmpty(input.Description))
                    results = projectGuids.Distinct().ToList();
                return Ok(_projectRepository.GetListQuery().Where(p => results.Contains(p.Guid))
                    .Include(p => p.Activities)
                    .Include(p => p.Budget)
                    .Include(p => p.Effects)
                    .Include(p => p.Materials)
                    .Include(p => p.Meetings)
                    .Include(p => p.Order)
                    .Include(p => p.Stages)
                    .Include(p => p.Statuses)
                    .Include(p => p.Teams)
                    .ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось выполнить поиск по документам. " +
                    $"Ошибка: {ex.Message}");

                return StatusCode(500, "Не удалось выполнить поиск по документам");
            }
        }
    }
}