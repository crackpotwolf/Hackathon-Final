using Data.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Parse_Documents.Controllers.API.v1
{
    /// <summary>
    /// Парсинг
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [DisplayName("parsing")]
    [SetRoute]
#if RELEASE
    [Authorize]
#endif
    public class ParsingController : _AbstractController
    {
        private readonly ILogger<IndexModel> _logger;

        /// <inheritdoc />
        public ParsingController(ILogger<IndexModel> logger) : base(logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Парсинг документа, и сохранение результатов в файл
        /// </summary>
        /// <param name="filePath">Путь файла</param>
        /// <returns>Путь к файлу с распознанным текстом</returns>
        [HttpPost("text")]
        [DisableRequestSizeLimit]
        [SwaggerResponse(200, "Путь к файлу с распознанным текстом", typeof(string))]
        [ProducesResponseType(typeof(Exception), 400)]
        public IActionResult ParseFromDocumentToSaveFile(string filePath)
        {
            try
            {
                _logger.LogInformation($"Начало парсинга документа");

                // Для замера времени
                var timeStart = DateTime.UtcNow;

                string outputPath = Path.ChangeExtension(filePath, ".txt"),
                    fileName = new FileInfo(filePath).Name;

                var result = "";

                if (Path.GetExtension(filePath) != ".pdf" && Path.GetExtension(filePath) != ".docx" &&
                    Path.GetExtension(filePath) != ".xlsx" && Path.GetExtension(filePath) != ".pptx")
                {
                    _logger.LogInformation($"Документ имеет неверный формат");
                    return StatusCode(500, $"Документ имеет неверный формат");
                }

                _logger.LogInformation($"Документ открыт");

                // Выбор парсера
                if (Path.GetExtension(filePath) == ".pdf")
                {
                    result = ParsePDF(filePath);
                }
                else if (Path.GetExtension(filePath) == ".docx")
                {
                    result = ParseDocx(filePath);
                }
                else if (Path.GetExtension(filePath) == ".xlsx")
                {
                    result = ParseExcel(filePath);
                }
                else if (Path.GetExtension(filePath) == ".pptx")
                {
                    result = ParsePptx(filePath);
                }

                _logger.LogInformation($"Документ обработан за: {(DateTime.UtcNow - timeStart).TotalSeconds} секунд");

                // Сохранить результат в файл
                using (var sw = new StreamWriter(outputPath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(result);
                    _logger.LogInformation($"Документ сохранен");
                }

                return Ok(new FileInfo(outputPath).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось получить текст документа. " +
                    $"Ошибка: {ex}");

                return StatusCode(500, "Не удалось получить текст документа. " +
                    $"Ошибка: {ex}");
            }
        }
    }
}