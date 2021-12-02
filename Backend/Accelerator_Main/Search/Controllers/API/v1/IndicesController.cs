using Data.Attributes;
using Data.Extensions;
using Data.Interfaces.Repositories;
using Data.Models.DB.Project;
using Data_Path.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Options;
using Search_Data.Models;
using Search_Data.Search;
using Search_Data.Services;
using System.ComponentModel;

namespace Search.Controllers.API.v1
{
    /// <summary>
    /// Индексирование
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [DisplayName("indices")]
    [SetRoute]
#if RELEASE
    [Authorize]
#endif
    public class IndicesController : ControllerBase
    {
        protected IBaseEntityRepository<Project> _fullProjectRepository;

        protected readonly IndicesManager _indicesManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly PathConfig _pathConfig;

        /// <inheritdoc />
        public IndicesController(ILogger<IndexModel> logger,
            IBaseEntityRepository<Project> fullProjectRepository,
            IndicesManager indicesManager,
            IOptions<PathConfig> pathConfig)
        {
            _fullProjectRepository = fullProjectRepository;
            _indicesManager = indicesManager;
            _pathConfig = pathConfig.Value;
            _logger = logger;
        }

        /// <summary>
        /// Индексирование документа
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="guidFile">Id файла</param>
        /// <returns></returns>
        [HttpPost("file/{guidFile:guid}")]
        [DisableRequestSizeLimit]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult AddDocumentForIndex(string filePath, Guid guidFile)
        {
            try
            {
                _indicesManager.AddDocumentForIndex(filePath, guidFile);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось проиндексировать документ. " +
                    $"Ошибка: {ex}");

                return StatusCode(500, "Не удалось проиндексировать документ.");
            }
        }

        /// <summary>
        /// Удаление документа
        /// </summary>
        /// <param name="guidFile">Id файла</param>
        /// <returns></returns>
        [HttpDelete("file/{guidFile:guid}")]
        [DisableRequestSizeLimit]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult DeleteDocumentForIndex(Guid guidFile)
        {
            try
            {
                _indicesManager.DeleteDocumentForIndex(guidFile);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось удалить документ. " +
                    $"Ошибка: {ex}");

                return StatusCode(500, "Не удалось удалить документ.");
            }
        }

        /// <summary>
        /// Удаление всех документов
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [DisableRequestSizeLimit]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult DeleteAllDocuments()
        {
            try
            {
                _indicesManager.DeleteAllDocuments();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось удалить все документы. " +
                    $"Ошибка: {ex}");

                return StatusCode(500, "Не удалось удалить все документы.");
            }
        }

        /// <summary>
        /// Обновление индексов
        /// </summary>
        /// <returns></returns>
        [HttpPut("update-indexes")]
        [DisableRequestSizeLimit]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult UpdateDocumentForIndex()
        {
            try
            {
                _logger.LogInformation($"Начало индексирования записей.");

                // Для замера времени
                DateTime timeStart = DateTime.UtcNow;

                var projects = _fullProjectRepository.GetList();

                var search = new WordSearch(_pathConfig.DocumentsIndexes);

                search.DeleteAllIndexes();

                projects.ForEach(p => p.AddSearchableObjectToIndexSeparately(p.Guid, search));

                search.CommitChanges();

                _logger.LogInformation($"Индексирование завершено за: {(DateTime.UtcNow - timeStart).TotalSeconds} секунд.");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось проиндексировать документ. " +
                    $"Ошибка: {ex.Message}");

                return StatusCode(500, "Не удалось проиндексировать документ.");
            }
        }

        /// <summary>
        /// Обновление документа
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="guidFile">Id файла</param>
        /// <returns></returns>
        [HttpPut("file/{guidFile:guid}")]
        [DisableRequestSizeLimit]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult UpdateDocumentForIndex(string filePath, Guid guidFile)
        {
            try
            {
                _indicesManager.UpdateDocumentForIndex(filePath, guidFile);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось проиндексировать документ. " +
                    $"Ошибка: {ex}");

                return StatusCode(500, "Не удалось проиндексировать документ.");
            }
        }
    }
}