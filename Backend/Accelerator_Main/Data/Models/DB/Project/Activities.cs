using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Project
{
    public  class Activities : ProjectData
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public string Executor { get; set; }

        /// <summary>
        /// Срок
        /// </summary>
        public string Timing { get; set; }
    }
}
