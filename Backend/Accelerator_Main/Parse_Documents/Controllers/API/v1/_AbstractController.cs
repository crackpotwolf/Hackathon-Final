using IronOcr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using OfficeOpenXml;
using Parse_Documents.MSOfficeParsers;

namespace Parse_Documents.Controllers.API.v1
{
    /// <summary>
    /// Базовый класс
    /// </summary>
    public class _AbstractController : ControllerBase
    {
        private readonly ILogger<IndexModel> _logger;

        /// <inheritdoc />
        public _AbstractController(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Парсинг pdf документов
        /// </summary>
        /// <param name="filePath">Путь к файлу для распознования</param>
        /// <returns>Результат распознавания</returns>
        protected static string ParsePDF(string filePath)
        {
            var ocr = new IronTesseract
            {
                Language = OcrLanguage.English
            };

            ocr.AddSecondaryLanguage(OcrLanguage.Russian);
            ocr.AddSecondaryLanguage(OcrLanguage.Arabic);

            var result = "";

            using var input = new OcrInput(filePath);

            var resultText = ocr.Read(input);
            result = resultText.Text;

            return result;
        }

        /// <summary>
        /// Парсинг docx документов
        /// </summary>
        /// <param name="filePath">Путь к файлу для распознования</param>
        /// <returns>Результат распознавания</returns>
        protected static string ParseDocx(string filePath)
        {
            var docxParser = new DocxParser();
            docxParser.Read(filePath);

            return docxParser.Text();
        }

        /// <summary>
        /// Парсинг pptx документов
        /// </summary>
        /// <param name="filePath">Путь к файлу для распознования</param>
        /// <returns>Результат распознавания</returns>
        protected static string ParsePptx(string filePath)
        {
            var pptxParser = new PptxParser();
            pptxParser.Read(filePath);

            return pptxParser.Text();
        }

        /// <summary>
        /// Парсинг xlsx документов
        /// </summary>
        /// <param name="filePath">Путь к файлу для распознования</param>
        /// <returns>Результат распознавания</returns>
        protected static string ParseExcel(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var result = "";

            var existingFile = new FileInfo(filePath);
            using var package = new ExcelPackage(existingFile);

            foreach (var worksheet in package.Workbook.Worksheets)
            {
                // get Column Count
                var colCount = worksheet.Dimension.End.Column;

                // get row count
                var rowCount = worksheet.Dimension.End.Row;

                for (var row = 1; row <= rowCount; row++)
                {
                    for (var col = 1; col <= colCount; col++)
                    {
                        var value = worksheet.Cells[row, col].Value;
                        if (value != null)
                        {
                            result += value.ToString().Trim() + " ";
                        }
                    }
                    result += "\n";
                }
            }

            return result;
        }
    }
}