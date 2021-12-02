using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Project
{
    public class Status : ProjectData
    {
        /// <summary>
        /// Статус
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ожидаемые результаты и мероприятия
        /// </summary>
        public string Expectations { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public string Date { get; set; }
    }
}
