using Data.Models.DB._BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Project
{
    public  class Team : ProjectData
    {
        /// <summary>
        /// ФИО
        /// </summary>
        public string Fio { get; set; }

        /// <summary>
        /// Должность/Организация
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Контакты
        /// </summary>
        public string Contacts { get; set; }
    }
}
