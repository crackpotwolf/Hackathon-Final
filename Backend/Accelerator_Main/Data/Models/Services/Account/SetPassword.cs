using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Services
{
    /// <summary>
    /// Сущность для задания пароля новым пользователем
    /// </summary>
    public class SetPassword
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Секретный отпечаток email
        /// </summary>
        public string SecurityStampEmail { get; set; }

        /// <summary>
        /// Новый пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}