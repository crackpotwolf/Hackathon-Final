using Accelerator.Controllers.API.v1.Files;
using Data.Attributes;
using Data.Extensions.Files;
using Data.Interfaces.Repositories;
using Data.Models.DB.Account;
using Data.Models.DB.Files;
using Data.Models.DB.Project;
using Data_Path.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Options;
using Search_Data.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Accelerator.Controllers.API.v1
{
    /// <summary>
    /// API для фото
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [DisplayName("project-photos")]
    [SetRoute]
    public class ProjectPhotoController : DocumentOperation
    {
        private readonly IBaseEntityRepository<Project> _projects;
        private readonly ILogger<IndexModel> _logger;

        /// <inheritdoc />
        public ProjectPhotoController(ILogger<IndexModel> logger,
            IBaseEntityRepository<DocumentInfo> documentInfo,
            IBaseEntityRepository<FileVersion> fileVersion,
            IWebHostEnvironment appEnvironment,
            IndicesManager indicesManager,
            IOptions<PathConfig> pathConfig,
            IOptions<ApiConfig> apiConfig,
            IBaseEntityRepository<Project> projects)
            : base(logger, documentInfo, fileVersion,
                appEnvironment, indicesManager, apiConfig, pathConfig)
        {
            _projects = projects;
            _logger = logger;
        }

        /// <summary>
        /// Получить URL фотки проекта
        /// </summary>
        /// <param name="projectGuid">Guid пользователя</param>
        /// <returns></returns>
        [HttpGet("{projectGuid:guid}/photos/url")]
        [Produces("application/json")]
        [SwaggerResponse(200, "URL фото", typeof(List<string>))]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult GetUrl([BindRequired] Guid projectGuid)
        {
            try
            {
                _logger.LogInformation($"Начало получения фоток проекта");

                var project = _projects.FirstOrDefault(p => p.Guid == projectGuid);

                // Проверка проекта
                if (project == null)
                {
                    _logger.LogInformation($"Такого проекта не существует");
                    return StatusCode(404, "Такого проекта не существует");
                }

                // Если фоток нету
                if (project.Photos.Count == 0)
                {
                    _logger.LogInformation($"Фоток отсутсвует");
                    return StatusCode(204, "Фоток отсутсвует");
                }

                var photosUri = new List<string>();

                foreach (var photoUri in project.Photos)
                {
                    // Получить Uri photo                 
                    photosUri.Add($"/photos/{photoUri}");
                }

                _logger.LogInformation($"Фотки проекта успешно получено");

                return Ok(photosUri);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось получить фотки проекта. " +
                                 $" Ошибка: {ex}");

                return StatusCode(500, $"Не удалось получить фотки проекта");
            }
        }

        /// <summary>
        /// Добавить фото проекта
        /// </summary>
        /// <param name="projectGuid">Guid проекта</param>
        /// <param name="file">Фото</param>
        /// <returns></returns>
        [HttpPost("{projectGuid:guid}/photo")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Фото успешно обновленно")]
        [ProducesResponseType(typeof(Exception), 400)]
        [RequestSizeLimit(1024 * 1024 * 10)]
        public IActionResult Upload([BindRequired] Guid projectGuid, [BindRequired] IFormFile file)
        {
            try
            {
                _logger.LogInformation($"Начало добавления фото проекта");

                var project = _projects.FirstOrDefault(p => p.Guid == projectGuid);

                // Проверка проекта
                if (project == null)
                {
                    _logger.LogInformation($"Такого проекта не существует");
                    return StatusCode(404, "Такого проекта не существует");
                }

                // Получить имя для сохранения файла
                var sourceDoсName = DocumentManage.GenerateDocumentName(file.FileName);

                // Получить путь для сохранения файла
                var sourceDocPath = Path.Combine(GetPathProjectPhotos(), sourceDoсName);

                // Сохранить файл
                SaveDocument(sourceDocPath, file);

                // Обновить проект
                project.Photos.Add(sourceDoсName);

                _projects.Update(project);

                _logger.LogInformation($"Фото проекта успешно добавлено");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось обновить фотку проекта. " +
                                 $" Ошибка: {ex}");

                return StatusCode(500, $"Не удалось обновить фотку проекта");
            }
        }

        /// <summary>
        /// Удалить фото проекта
        /// </summary>
        /// <param name="projectGuid">Guid проекта</param>
        /// <param name="photoName">Имя фото</param>
        /// <returns></returns>
        [HttpDelete("{projectGuid:guid}/photo")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Фото успешно удаленно")]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Delete([BindRequired] Guid projectGuid, string photoName)
        {
            try
            {
                _logger.LogInformation($"Начало удаления фото проекта");

                var project = _projects.FirstOrDefault(p => p.Guid == projectGuid);

                // Проверка проекта
                if (project == null)
                {
                    _logger.LogInformation($"Такого проекта не существует");
                    return StatusCode(404, "Такого проекта не существует");
                }

                // Если фотка уже есть
                if (project.Photos.Contains(photoName))
                {
                    // Удалить фото
                    DeleteDocument(Path.Combine(GetPathProjectPhotos(), photoName));
                }
                else 
                {
                    return StatusCode(404, "Такого фото не существует");
                }

                // Обновить проект
                project.Photos.Remove(photoName);

                _projects.Update(project);

                _logger.LogInformation($"Фото проекта успешно удалено");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось удалить фото проекта. " +
                                 $" Ошибка: {ex}");

                return StatusCode(500, $"Не удалось удалить фото проекта");
            }
        }
    }
}