using Data.Models.DB._BaseEntities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.DB.Files
{
    /// <summary>
    /// Информация о документе
    /// </summary>
    public class DocumentInfo : BaseEntity
    {
        /// <summary>
        /// Guid заявки
        /// </summary>
        [ForeignKey("Project")]
        public Guid ProjectGuid { get; set; }

        /// <summary>
        /// Заявка
        /// </summary>
        [JsonIgnore]
        public virtual Project.Project Project { get; set; }

        /// <summary>
        /// Оригинальное название файла
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Имя пути
        /// </summary>
        public string PathName { get; set; }
    }
}