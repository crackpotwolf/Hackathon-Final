using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.ModelViews.Files
{
    /// <summary>
    /// Для возврата после загрузки документов
    /// </summary>
    public class DocumentView
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Название документа
        /// </summary>
        public string OriginalName { get; set; }
    }
}