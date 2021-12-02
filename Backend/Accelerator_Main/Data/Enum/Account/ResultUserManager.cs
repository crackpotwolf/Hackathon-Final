using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Enum.Account
{
    /// <summary>
    /// Результат смены пароля
    /// </summary>
    public enum ResultUserManager
    {
        /// <summary>
        /// Смена пароля прошла успешно
        /// </summary>
        [Display(Description = "Password successfully changed")]
        Success = 0,

        /// <summary>
        /// Пользователь с указанным идентификатором не найден
        /// </summary>
        [Display(Description = "User not found")]
        UserNotFound = 1,

        /// <summary>
        /// Неправильный секретный отпечаток
        /// </summary>
        [Display(Description = "Security stamp incorrect")]
        SecurityStampIncorrect = 2,

        /// <summary>
        /// Неверный пароль
        /// </summary>
        [Display(Description = "Invalid password")]
        InvalidPassword = 3,

        /// <summary>
        /// Неверный пароль для подтверждения
        /// </summary>
        [Display(Description = "Invalid confirm password")]
        InvalidConfirmPassword = 4,

        /// <summary>
        /// Неверный текущий пароль
        /// </summary>
        [Display(Description = "Invalid current password")]
        InvalidCurrentPassword = 5,

        /// <summary>
        /// Неверный текущий пароль
        /// </summary>
        [Display(Description = "Email is not confirmed")]
        EmailNotConfirmed = 6,

        /// <summary>
        /// Непредвиденная ошибка
        /// </summary>
        [Display(Description = "Error")]
        Error = 500,
    }
}