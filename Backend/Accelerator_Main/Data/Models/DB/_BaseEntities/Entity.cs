using AutoMapper;
using Data.Interfaces._BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB._BaseEntities
{
    /// <summary>
    /// Базовый класс
    /// </summary>
    public class Entity : IEntity
    {
        /// <inheritdoc />
        public Entity()
        {
            DateCreate = DateTime.UtcNow;
            DateUpdate = DateTime.UtcNow;
        }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        [ReadOnly(true)]
        [IgnoreMap]
        public virtual DateTime DateCreate { get; set; }

        /// <summary>
        /// Дата обновления записи
        /// </summary>
        [ReadOnly(true)]
        [IgnoreMap]
        public virtual DateTime DateUpdate { get; set; }

        /// <summary>
        /// Обновление дат перед сохранением в БД
        /// </summary>
        /// <param name="now"></param>
        public void UpdateBeforeSave(DateTime now)
        {
            DateUpdate = now;
        }
    }
}