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
#if RELEASE
    [Authorize]
#endif
    public class SearchController : ControllerBase
    {
        protected IBaseEntityRepository<Project> _fullProjectRepository;

        private readonly ILogger<IndexModel> _logger;
        private readonly PathConfig _pathConfig;

        /// <inheritdoc />
        public SearchController(ILogger<IndexModel> logger,
            IBaseEntityRepository<Project> fullProjectRepository,
            IOptions<PathConfig> pathConfig)
        {
            _fullProjectRepository = fullProjectRepository;
            _pathConfig = pathConfig.Value;
            _logger = logger;
        }

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
                _logger.LogInformation($"Начало поиска объектов.");

                // Для замера времени
                DateTime timeStart = DateTime.UtcNow;

                // Инициализация
                _logger.LogInformation($"Инициализация.");

                var search = new WordSearch(_pathConfig.DocumentsIndexes, typeof(Project).GetSerachableFieldsNames());

                // Поиск
                _logger.LogInformation($"Поиск. {inputText}");

                var result = search.Search(inputText);

                // Результаты
                _logger.LogInformation($"Запись результатов. {inputText}");

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
                _logger.LogInformation($"Начало поиска объектов.");

                // Для замера времени
                DateTime timeStart = DateTime.UtcNow;

                // Инициализация
                _logger.LogInformation($"Инициализация.");

                var search = new WordSearch(_pathConfig.DocumentsIndexes, typeof(Project).GetSerachableFieldsNames());

                // Поиск
                _logger.LogInformation($"Поиск. {inputText}");

                var result = search.Search(inputText);

                // Результаты
                _logger.LogInformation($"Запись результатов. {inputText}");

                _logger.LogInformation($"Запрос: '{result.Query}' всего: {result.TotalHits}. {inputText}");

                var results = _fullProjectRepository.GetListQuery().Where(p => result.Hits.Select(t => t.Guid).Contains(p.Guid)).ToList();

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
    }
}