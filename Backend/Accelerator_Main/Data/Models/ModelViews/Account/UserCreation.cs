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
    public class UserCreation
    {
        /// <summary>
        /// Почта
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        public virtual string LastName { get; set; }
    }
}