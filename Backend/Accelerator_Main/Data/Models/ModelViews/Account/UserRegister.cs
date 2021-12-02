using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models.ModelViews.Account
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    public class UserRegister
    {
        /// <summary>
        /// Почта
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [RegularExpression(@"^(\+[0-9]{11})$", ErrorMessage = "Invalid phone number")]
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Required]
        public virtual string MiddleName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        public virtual string LastName { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        [MinLength(6)]
        public virtual string Password { get; set; }
    }
}