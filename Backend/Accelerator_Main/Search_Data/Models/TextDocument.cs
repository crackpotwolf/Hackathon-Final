using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Search_Data.Models 
{
    /// <summary>
    /// Структура документа
    /// </summary>
    public class TextDocument
    {
        /// <summary>
        /// Guid документа
        /// </summary>
        public Guid Guid { get; set; } 

        /// <summary>
        /// Текст документа
        /// </summary>
        public string Text { get; set; }
    }
}