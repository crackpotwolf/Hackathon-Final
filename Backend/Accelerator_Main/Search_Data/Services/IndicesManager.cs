using Data_Path.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Search_Data.Models;
using Search_Data.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search_Data.Services
{
    /// <summary>
    /// Работа с индексами
    /// </summary>
    public class IndicesManager
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly PathConfig _pathConfig;

        /// <inheritdoc />
        public IndicesManager(ILogger<IndexModel> logger,
            IOptions<PathConfig> pathConfig)
        {
            _pathConfig = pathConfig.Value;
            _logger = logger;
        }

        /// <summary>
        /// Индексирование документа
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="guidFile">Id файла</param>
        /// <returns></returns>
        public void AddDocumentForIndex(string filePath, Guid guidFile)
        {
            _logger.LogInformation($"Начало индексирования документа.");

            // Для замера времени
            var timeStart = DateTime.UtcNow;

            var document = new TextDocument()
            {
                Guid = guidFile,
                Text = File.ReadAllText(filePath)
            };

            _logger.LogInformation($"Документ считан.");

            // Инициализация
            _logger.LogInformation($"Инициализация документа.");

            var search = new WordSearch(_pathConfig.DocumentsIndexes);

            // Индексирование
            _logger.LogInformation($"Индексирование документа.");

            search.AddIndex(document);

            // Применить изменения
            search.CommitChanges();

            _logger.LogInformation($"Индексирование завершено за: {(DateTime.UtcNow - timeStart).TotalSeconds} секунд.");
        }

        /// <summary>
        /// Удаление документа
        /// </summary>
        /// <param name="guidFile">Id файла</param>
        /// <returns></returns>
        public void DeleteDocumentForIndex(Guid guidFile)
        {
            _logger.LogInformation($"Начало удаления документа.");

            // Для замера времени
            var timeStart = DateTime.UtcNow;

            // Инициализация
            _logger.LogInformation($"Инициализация для документа.");

            var search = new WordSearch(_pathConfig.DocumentsIndexes);

            // Удаление
            _logger.LogInformation($"Удаление документа.");

            search.DeleteIndex(guidFile);

            // Применить изменения
            search.CommitChanges();

            _logger.LogInformation($"Удаление завершено за: {(DateTime.UtcNow - timeStart).TotalSeconds} секунд.");
        }

        /// <summary>
        /// Удаление всех документов
        /// </summary>
        /// <returns></returns>
        public void DeleteAllDocuments()
        {
            _logger.LogInformation($"Начало удаления всех документов");

            // Для замера времени
            var timeStart = DateTime.UtcNow;

            // Инициализация
            _logger.LogInformation($"Инициализация");

            var search = new WordSearch(_pathConfig.DocumentsIndexes);

            // Удаление
            _logger.LogInformation($"Удаление всех документов");

            search.DeleteAllIndexes();

            // Применить изменения
            search.CommitChanges();

            _logger.LogInformation($"Удаление завершено за: {(DateTime.UtcNow - timeStart).TotalSeconds} секунд");
        }

        /// <summary>
        /// Обновление документа
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="guidFile">Id файла</param>
        /// <returns></returns>
        public void UpdateDocumentForIndex(string filePath, Guid guidFile)
        {
            _logger.LogInformation($"Начало индексирования документа.");

            // Для замера времени
            var timeStart = DateTime.UtcNow;

            var document = new TextDocument()
            {
                Guid = guidFile,
                Text = File.ReadAllText(filePath)
            };

            _logger.LogInformation($"Документ считан.");

            // Инициализация
            _logger.LogInformation($"Инициализация документа.");

            var search = new WordSearch(_pathConfig.DocumentsIndexes);

            // Индексирование
            _logger.LogInformation($"Индексирование документа.");

            search.UpdateIndex(document);

            // Применить изменения
            search.CommitChanges();

            _logger.LogInformation($"Индексирование завершено за: {(DateTime.UtcNow - timeStart).TotalSeconds} секунд.");
        }
    }
}