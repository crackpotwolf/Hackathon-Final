using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Services
{
    /// <summary>
    /// Сущность для восстановления пароля
    /// </summary>
    public class ForgotPassword
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Секретный отпечаток
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// Новый пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}