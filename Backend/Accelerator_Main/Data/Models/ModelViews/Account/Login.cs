using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models.ModelViews.Account
{
    /// <summary>
    /// Данные входа
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Почта или логин
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}