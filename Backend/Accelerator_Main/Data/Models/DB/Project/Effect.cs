using Data.Interfaces;
using Data.Models.DB._BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Project
{
    public class Effect : ProjectData, ISearchable
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение показателя
        /// </summary>
        public string Value { get; set; }
    }
}
