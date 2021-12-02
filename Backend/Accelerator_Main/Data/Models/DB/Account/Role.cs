using Data.Models.DB._BaseEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Data.Models.DB.Account
{
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public virtual ICollection<UserRoles> UserRoles { get; set; }

        public bool IsSecret { get; set; } = false;
    }
}