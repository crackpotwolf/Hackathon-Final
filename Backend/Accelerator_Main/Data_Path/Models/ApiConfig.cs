using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Path.Models
{
    /// <summary>
    /// API конфигуратор
    /// </summary>
    public class ApiConfig
    {
        /// <summary>
        /// Парсинг документа
        /// </summary>
        public string Parsing { get; set; }

        /// <summary>
        /// Индексирование документов
        /// </summary>
        public string Indices { get; set; }

        /// <summary>
        /// Поиск по документам
        /// </summary>
        public string Search { get; set; }
    }
}