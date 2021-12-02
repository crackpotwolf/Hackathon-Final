using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Data.Interfaces._BaseEntities
{
    /// <summary>
    /// Базовый интерфейс
    /// </summary>
    public interface IBaseEntity : IEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        Guid Guid { get; set; }

        /// <summary>
        /// Флаг удаления
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        bool IsDelete { get; set; }
    }
}