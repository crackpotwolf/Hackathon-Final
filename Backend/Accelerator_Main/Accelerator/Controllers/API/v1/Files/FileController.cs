using Data.Attributes;
using Data.Interfaces.Repositories;
using Data.Models.DB.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using Data.Models.DB.Project;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using Data.Extensions.Files;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Data.Models.ModelViews.Files;
using Data_Path.Models;
using Search_Data.Services;
using Microsoft.AspNetCore.Authorization;

namespace Accelerator.Controllers.API.v1.Files
{
    /// <summary>
    /// API загрузки документа
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [DisplayName("files")]
    [SetRoute]
    public class FileController : DocumentOperation
    {
        private readonly IBaseEntityRepository<DocumentInfo> _documentInfo;
        private readonly IBaseEntityRepository<FileVersion> _fileVersion;
        private readonly IBaseEntityRepository<Project> _project;

        private readonly ILogger<IndexModel> _logger;

        /// <inheritdoc />
        public FileController(ILogger<IndexModel> logger,
            IBaseEntityRepository<FileVersion> fileVersion,
            IBaseEntityRepository<DocumentInfo> documentInfo,
            IBaseEntityRepository<Project> project,
            IWebHostEnvironment appEnvironment,
            IndicesManager indicesManager,
            IOptions<ApiConfig> apiConfig,
            IOptions<PathConfig> pathConfig)
            : base(logger, documentInfo, fileVersion,
                appEnvironment, indicesManager, apiConfig, pathConfig)
        {
            _documentInfo = documentInfo;
            _fileVersion = fileVersion;
            _project = project;
            _logger = logger;
        }

        /// <summary>
        /// Загрузить документ
        /// </summary>
        /// <param name="projectGuid">Guid заявки</param>
        /// <param name="files">Файлы документа</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [SwaggerResponse(200, "Информация о документах", typeof(List<DocumentInfo>))]
        [ProducesResponseType(typeof(Exception), 400)]
        [RequestSizeLimit(1024 * 1024 * 100)]
        public IActionResult Upload([BindRequired] Guid projectGuid, List<IFormFile> files)
        {
            try
            {
                _logger.LogInformation($"Начало загрузки документов.");

                // Получить заявку
                var project = _project.FirstOrDefault(p => p.Guid == projectGuid);

                // Проверка заявки
                if (project == null)
                {
                    _logger.LogInformation($"Такой заявки не существует.");
                    return StatusCode(404, "Такой заявки не существует");
                }

                // Проверка файлов
                if (files.Count == 0)
                {
                    _logger.LogInformation($"Файлы не выбраны.");
                    return StatusCode(404, "Файлы не выбраны");
                }

                foreach (var file in files)
                {
                    try
                    {
                        DocumentCheck(file);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(400, ex);
                    }
                }

                _logger.LogInformation($"Файлы проверены.");

                var documentsInfo = new List<DocumentView>();

                foreach (var file in files)
                {
                    // Создать информация о документе
                    var documentInfo = CreateDocumentInfo(project.Guid, file.FileName);

                    _logger.LogInformation($"Информация о документе создана.");

                    // Получить имя для сохранения файла
                    var sourceDoсName = DocumentManage.GenerateDocumentName(file.FileName);

                    // Получить путь для сохранения файла
                    var sourceDocPath = GetPathFileName(project.PathName, documentInfo.PathName, sourceDoсName);

                    try
                    {
                        // Сохранить файл
                        SaveDocument(sourceDocPath, file);
                        _logger.LogInformation($"Сохранение завершено.");

                        // Создать информацию о версии файла 
                        var fileVersion = CreateFileVersion(project.Guid, documentInfo.Guid, file.FileName,
                            sourceDoсName);

                        // Работа с файлом: парсинг, индексация, создание информации о версии файла
                        var jobId = BackgroundJob.Schedule(() =>
                            WorkingWithFile(documentInfo.Guid, fileVersion.Guid, false),
                            TimeSpan.FromSeconds(20));

                        documentsInfo.Add(new DocumentView()
                        {
                            Guid = documentInfo.Guid,
                            OriginalName = documentInfo.OriginalName,
                        });
                    }
                    catch (Exception ex)
                    {
                        DeleteDocument(sourceDocPath);
                        _documentInfo.Delete(documentInfo);

                        _logger.LogError($"Не удалось загрузить файлы. " +
                            $" Ошибка: {ex}");

                        return StatusCode(500, $"Не удалось загрузить файлы");
                    }
                }

                return Ok(documentsInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось загрузить файлы. " +
                    $" Ошибка: {ex}");

                return StatusCode(500, $"Не удалось загрузить файлы");
            }
        }

        /// <summary>
        /// Обновить документ
        /// </summary>
        /// <param name="documentInfoGuid">Guid документа</param>
        /// <param name="file">Новый файл</param>
        /// <returns></returns>
        [HttpPut("{documentInfoGuid:guid}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Guid документа", typeof(DocumentView))]
        [ProducesResponseType(typeof(Exception), 400)]
        [RequestSizeLimit(1024 * 1024 * 100)]
        public IActionResult Update(Guid documentInfoGuid, [BindRequired] IFormFile file)
        {
            try
            {
                _logger.LogInformation($"Начало обновления документа.");

                // Получить информацию о документе
                var documentInfo = _documentInfo.FirstOrDefault(p => p.Guid == documentInfoGuid);

                // Проверка документа
                if (documentInfo == null)
                {
                    _logger.LogInformation($"Такого документа не существует.");
                    return StatusCode(400, "Такого документа не существует");
                }

                // Получить заявку
                var project = _project.FirstOrDefault(p => p.Guid == documentInfo.ProjectGuid);

                // Проверка заявки
                if (project == null)
                {
                    _logger.LogInformation($"Такой заявки не существует.");
                    return StatusCode(400, "Такой заявки не существует");
                }

                // Проверить файлы документа
                try
                {
                    DocumentCheck(file);
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex);
                }

                _logger.LogInformation($"Файлы проверены.");

                // Получить имя для сохранения файла
                var sourceDoсName = DocumentManage.GenerateDocumentName(file.FileName);

                // Получить путь для сохранения файла
                var sourceDocPath = GetPathFileName(project.PathName, documentInfo.PathName, sourceDoсName);

                try
                {
                    // Сохранить файл
                    SaveDocument(sourceDocPath, file);
                    _logger.LogInformation($"Сохранение завершено.");

                    // Создать информацию о версии файла 
                    var fileVersion = CreateFileVersion(project.Guid, documentInfo.Guid, file.FileName,
                        sourceDoсName);

                    // Работа с файлом: парсинг, индексация, создание информации о версии файла
                    var jobId = BackgroundJob.Schedule(() =>
                        WorkingWithFile(documentInfo.Guid, fileVersion.Guid, true),
                        TimeSpan.FromSeconds(20));

                    // Обновить имя файла
                    documentInfo.OriginalName = file.FileName;
                    _documentInfo.Update(documentInfo);
                }
                catch (Exception ex)
                {
                    // Удалить документ
                    DeleteDocument(sourceDocPath);

                    _logger.LogError($"{ex}");

                    return StatusCode(500, $"Не удалось обновить файл");
                }

                return Ok(new DocumentView()
                {
                    Guid = documentInfo.Guid,
                    OriginalName = documentInfo.OriginalName,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось обновить файл. " +
                    $" Ошибка: {ex}");

                return StatusCode(500, $"Не удалось обновить файл");
            }
        }

        /// <summary>
        /// Скачать документ
        /// </summary>
        /// <param name="documentInfoGuid">Guid документа</param>
        /// <returns></returns>
        [HttpGet("{documentInfoGuid:guid}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Файл", typeof(File))]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Download(Guid documentInfoGuid)
        {
            try
            {
                _logger.LogInformation($"Начало скачивания документа.");

                // Для замера времени
                var timeStart = DateTime.UtcNow;

                // Получить информация о документе
                var documentInfo = _documentInfo.FirstOrDefault(p => p.Guid == documentInfoGuid);

                // Проверка документа
                if (documentInfo == null)
                {
                    _logger.LogInformation($"Такого документа не существует.");
                    return StatusCode(404, "Такого документа не существует");
                }

                // Получить заявку
                var project = _project.FirstOrDefault(p => p.Guid == documentInfo.ProjectGuid);

                // Получить последнюю версию файла
                var fileVersion = _fileVersion.GetListQuery()
                    .Where(p => p.DocumentInfoGuid == documentInfo.Guid)
                    .DefaultIfEmpty()
                    .OrderByDescending(p => p.DateUpload)
                    .First();

                // Получить путь для сохранения файла
                var sourceDocPath = GetPathFileName(project.PathName, documentInfo.PathName, fileVersion.PathName);

                var net = new System.Net.WebClient();
                var data = net.DownloadData(sourceDocPath);
                var content = new MemoryStream(data);
                const string contentType = "APPLICATION/octet-stream";
                var fileName = documentInfo.OriginalName + Path.GetExtension(sourceDocPath);

                _logger.LogInformation($"Документ скачан за " +
                    $"{(DateTime.UtcNow - timeStart).TotalSeconds} секунд.");

                return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось скачать файл. " +
                    $" Ошибка: {ex}");

                return StatusCode(500, $"Не удалось скачать файл");
            }
        }

        /// <summary>
        /// Удалить документ
        /// </summary>
        /// <param name="documentInfoGuid">Guid документа</param>
        /// <returns></returns>
        [HttpDelete("{documentInfoGuid:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult Delete(Guid documentInfoGuid)
        {
            try
            {
                _logger.LogInformation($"Начало удаления документа.");

                // Получить документ
                var documentInfo = _documentInfo.GetListQuery()
                    .Include(p => p.Project)
                    .FirstOrDefault(p => p.Guid == documentInfoGuid);

                // Проверка документа
                if (documentInfo == null)
                {
                    _logger.LogInformation($"Такого документа не существует.");
                    return StatusCode(404, "Такого документа не существует");
                }

                // Путь к заявки
                var niokrPath = Path.Combine(GetPathDocumentation(), documentInfo.PathName);

                // Путь к документу
                var documentPath = Path.Combine(niokrPath, documentInfo.PathName);

                // Удалить все файлы данного документа
                DeleteFolder(documentPath);

                // Удалить индексы документа
                DeleteDocumentForIndex(documentInfo.Guid);

                // Удалить документ из бд
                _documentInfo.RemoveCascade(documentInfo);

                _logger.LogInformation($"Документ удален.");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось удалить документ. " +
                    $" Ошибка: {ex}");

                return StatusCode(500, $"Не удалось удалить документ");
            }
        }

        /// <summary>
        /// Информация о загруженных файлах заявки
        /// </summary>
        /// <param name="projectGuid">Guid заявки</param>
        /// <returns></returns>
        [HttpGet("info/niokr/{niokrGuid:guid}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Информация о файлах заявки", typeof(List<DocumentInfo>))]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult FilesInfo(Guid projectGuid)
        {
            try
            {
                _logger.LogInformation($"Начало получения информации о файлах заявки.");

                // Получить заявку
                var project = _project.FirstOrDefault(p => p.Guid == projectGuid);

                // Проверка заявки
                if (project == null)
                {
                    _logger.LogInformation($"Такого заявки не существует.");
                    return StatusCode(404, "Такого заявки не существует");
                }

                // Получить информацию о файлах
                var documentInfo = _documentInfo.GetListQuery()
                    .Where(p => p.ProjectGuid == project.Guid)
                    .ToList();

                _logger.LogInformation($"Информация о файлах заявки получена.");

                return Ok(documentInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось получить информацию о файлах заявки. " +
                    $" Ошибка: {ex}");

                return StatusCode(500, $"Не удалось получить информацию о файлах заявки.");
            }
        }

        /// <summary>
        /// Скачать версию документ
        /// </summary>
        /// <param name="fileVersionGuid">Guid версии документа</param>
        /// <returns></returns>
        [HttpGet("version/{fileVersionGuid:guid}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Файл", typeof(File))]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult DownloadVersion(Guid fileVersionGuid)
        {
            try
            {
                _logger.LogInformation($"Начало скачивания документа.");

                // Для замера времени
                var timeStart = DateTime.UtcNow;

                // Получить информация о документе
                var fileVersion = _fileVersion.GetListQuery()
                    .Include(p => p.DocumentInfo)
                    .ThenInclude(p => p.Project)
                    .FirstOrDefault(p => p.Guid == fileVersionGuid);

                // Проверка документа
                if (fileVersion == null)
                {
                    _logger.LogInformation($"Такой версии документа не существует.");
                    return StatusCode(404, "Такой версии документа не существует");
                }

                // Получить путь для сохранения файла
                var sourceDocPath = GetPathFileName(fileVersion.DocumentInfo.Project.PathName, fileVersion.DocumentInfo.PathName,
                    fileVersion.PathName);

                var net = new System.Net.WebClient();
                var data = net.DownloadData(sourceDocPath);
                var content = new MemoryStream(data);
                const string contentType = "APPLICATION/octet-stream";
                var fileName = fileVersion.OriginalName + Path.GetExtension(sourceDocPath);

                _logger.LogInformation($"Документ скачан за " +
                    $"{(DateTime.UtcNow - timeStart).TotalSeconds} секунд.");

                return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось скачать версию файла. " +
                    $" Ошибка: {ex}");

                return StatusCode(500, $"Не удалось скачать версию файла");
            }
        }

        /// <summary>
        /// Информация о загруженных версиях документа
        /// </summary>
        /// <param name="documentInfoGuid">Guid документа</param>
        /// <returns></returns>
        [HttpGet("info/niokr/files/{documentInfoGuid:guid}/version")]
        [Produces("application/json")]
        [SwaggerResponse(200, "Информация о файлах заявки", typeof(List<FIleVersionView>))]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult FilesVersionInfo(Guid documentInfoGuid)
        {
            try
            {
                _logger.LogInformation($"Начало получения информации о загруженных версиях файла.");

                // Получить версии файла
                var fileVersions = _fileVersion.GetListQuery()
                    .Where(p => p.DocumentInfoGuid == documentInfoGuid)
                    .DefaultIfEmpty()
                    .OrderByDescending(p => p.DateUpload)
                    .ToList();

                // Проверка заявки
                if (fileVersions.Count == 0)
                {
                    _logger.LogInformation($"Такого файла не существует.");
                    return StatusCode(404, "Такого файла не существует");
                }

                var fileVersionsView = new List<FIleVersionView>();

                foreach (var v in fileVersions)
                {
                    fileVersionsView.Add(new FIleVersionView
                    {
                        Guid = v.Guid,
                        OriginalName = v.OriginalName,
                        DateUpload = v.DateUpload
                    });
                }

                _logger.LogInformation($"Информация о загруженных версиях файла получена.");

                return Ok(fileVersionsView);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось получить информацию о загруженных версиях файла. " +
                    $" Ошибка: {ex}");

                return StatusCode(500, $"Не удалось получить информацию о загруженных версиях файла");
            }
        }
    }
}