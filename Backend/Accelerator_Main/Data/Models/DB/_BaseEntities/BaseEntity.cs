using Data.Interfaces._BaseEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Data.Models.DB._BaseEntities
{
    /// <summary>
    /// Базовый класс
    /// </summary>
    public class BaseEntity : Entity, IBaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public virtual Guid Guid { get; set; }

        /// <summary>
        /// Флаг удаления
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public bool IsDelete { get; set; }
    }
}