using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models.ModelViews.Account
{
    /// <summary>
    /// Обновленная информация о пользователе
    /// </summary>
    public class UserUpdateInfo
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string MiddleName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string LastName { get; set; }
    }
}
