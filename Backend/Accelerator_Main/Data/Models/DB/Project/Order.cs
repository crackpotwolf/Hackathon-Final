using Data.Attributes;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Project
{
    public class Order : ProjectData, ISearchable
    {
        /// <summary>
        /// Наименование команды/организации
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Стадия готовности продукта
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// Краткое описание продукта
        /// </summary>
        [Searchable]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Кейсы использования продукта
        /// </summary>
        [Searchable]
        public string Cases { get; set; }

        /// <summary>
        /// Польза продукта
        /// </summary>
        [Searchable]
        public string Benefit { get; set; }
        /// <summary>
        /// Организация Московского транспорта, интересная в первую очередь
        /// </summary>
        [Searchable]
        public string TransportOrganization { get; set; }

        /// <summary>
        /// Запрос к акселератору и видение пилотного проекта
        /// </summary>
        [Searchable]
        public string PilotVision { get; set; }

        /// <summary>
        /// Требуется ли сертификация продукта
        /// </summary>
        public string Sertification { get; set; }

        /// <summary>
        /// ФИО контактного лица по заявке
        /// </summary>
        public string ContactFio { get; set; }

        /// <summary>
        /// Должность контактного лица
        /// </summary>
        public string ContactPosition { get; set; }

        /// <summary>
        /// Контактный телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Контактная почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Наименование юридического лица
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// ИНН юридического лица
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// Сколько человек в организации
        /// </summary>
        public string PeopleCount { get; set; }

        /// <summary>
        /// Сайт 
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Откуда узнали про акселератор
        /// </summary>
        public string AcceleratorInfo { get; set; }

        /// <summary>
        /// Ссылка на презентацию
        /// </summary>
        public string Presentation { get; set; }
    }
}
