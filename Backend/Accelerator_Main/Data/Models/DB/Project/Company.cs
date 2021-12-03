using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Project
{
    /// <summary>
    /// Компании, проявившие интерес к проекту
    /// </summary>
    public class Company : ProjectData
    {
        /// <summary>
        /// Наименование компании
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Контактное лицо
        /// </summary>
        public string ContactFace { get; set; }

    }
}
