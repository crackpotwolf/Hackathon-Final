using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Path.Models
{
    /// <summary>
    /// Пути
    /// </summary>
    public class PathConfig
    {
        /// <summary>
        /// Папка с документами
        /// </summary>
        public string Documentation { get; set; }

        /// <summary>
        /// Папка с индексами
        /// </summary>
        public string DocumentsIndexes { get; set; }

        /// <summary>
        /// Папка с фото пользователями
        /// </summary>
        public string UserPhotos { get; set; }
    }
}