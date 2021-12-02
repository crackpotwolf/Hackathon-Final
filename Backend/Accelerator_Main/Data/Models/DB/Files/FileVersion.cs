using Data.Enum.Status;
using Data.Models.DB._BaseEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Files
{
    /// <summary>
    /// Версия файла документа
    /// </summary>
    public class FileVersion : BaseEntity
    {
        /// <summary>
        /// Guid типа документа
        /// </summary>
        [ForeignKey("DocumentInfo")]
        public Guid DocumentInfoGuid { get; set; }

        /// <summary>
        /// Тип документа 
        /// </summary>
        [JsonIgnore]
        public virtual DocumentInfo DocumentInfo { get; set; }

        /// <summary>
        /// Дата загрузки
        /// </summary>
        public DateTime DateUpload { get; set; }

        /// <summary>
        /// Оригинальное название файла
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Путь к исходному файлу
        /// </summary>
        public string PathName { get; set; }

        /// <summary>
        /// Путь к обработанному файлу
        /// </summary>
        public string PathNameParce { get; set; }

        /// <summary>
        /// Статус парсинга
        /// </summary>
        public CompleteStatus ParceStatus { get; set; }

        /// <summary>
        /// Статус индексирования
        /// </summary>
        public CompleteStatus IndexStatus { get; set; }
    }
}