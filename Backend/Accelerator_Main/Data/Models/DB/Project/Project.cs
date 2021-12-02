using Data.Attributes;
using Data.Extensions;
using Data.Interfaces;
using Data.Models.DB._BaseEntities;
using Data.Models.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Project
{
    public class Project : BaseEntity, ISearchable
    {
        /// <summary>
        /// Наименование проекта
        /// </summary>
        [Searchable]
        public string Name { get; set; }

        /// <summary>
        /// Организация транспортного комплекса Москвы
        /// </summary>
        public string TransportComplexOrganization { get; set; }

        /// <summary>
        /// Участник программы пилотирования «Транспортные инновации Москвы»
        /// </summary>
        public string PilotMember { get; set; }

        /// <summary>
        /// Руководитель проекта
        /// </summary>
        public string Leader { get; set; }

        /// <summary>
        /// Координатор от участника программы пилотирования «Транспортные инновации Москвы»
        /// </summary>
        public string PilotCoordinator { get; set; }

        /// <summary>
        /// Координатор от организации транспортного комплекса Москвы
        /// </summary>
        public string TransportComplexCoordinator { get; set; }

        /// <summary>
        /// Краткое описание продукта
        /// </summary>
        [Searchable]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Сроки реализации проекта
        /// </summary>
        public string Timing { get; set; }

        /// <summary>
        /// КОНТЕКСТ И ПОТРЕБНОСТИ
        /// </summary>
        [Searchable]
        public string Context { get; set; }

        /// <summary>
        /// Имя пути
        /// </summary>
        public string PathName { get; set; }

        /// <summary>
        /// Тэги
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Список тэгов в строковом представлении
        /// </summary>
        [Searchable]
        [JsonIgnore]
        [NotMapped]
        public string TagsString { get => Tags != null ? String.Join(", ", Tags) : ""; }

        /// <summary>
        /// заявка
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Поле для индексации заявки (описание)
        /// </summary>
        [Searchable]
        [NotMapped]
        [JsonIgnore]
        public string OrderStringData { get => Order.GetSearchablePropertiesStringifiedValue(); }

        /// <summary>
        /// Поле для индексации заявки (описание)
        /// </summary>
        [Searchable]
        [NotMapped]
        [JsonIgnore]
        public string OrderTeam { get => Order.TeamName; }

        public List<Effect> Effects { get; set; }

        public List<Stage> Stages { get; set; }

        public List<Team> Teams { get; set; }

        public List<Budget> Budget { get; set; }

        public List<Status> Statuses { get; set; }

        public List<Activities> Activities { get; set; }

        public List<Meeting> Meetings { get; set; }

        public List<Material> Materials { get; set; }
    }
}
