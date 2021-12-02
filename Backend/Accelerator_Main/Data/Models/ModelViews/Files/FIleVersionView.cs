using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.ModelViews.Files
{
    /// <summary>
    /// Представление информации о версии документа
    /// </summary>
    public class FIleVersionView
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Название документа
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Дата загрузки
        /// </summary>
        public DateTime DateUpload { get; set; }
    }
}