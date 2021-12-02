using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Enum.Account
{
    /// <summary>
    /// Результат авторизации/регистрации
    /// </summary>
    public enum AuthStatus
    {
        /// <summary>
        /// Авторизация/регистрация успешно пройдена
        /// </summary>
        [Display(Name = "Успешно")]
        Success = 0, // успешно

        /// <summary>
        /// Не правильный логин или пароль
        /// </summary>
        ErrorAuth = -1, // не правильный пароль или логин

        /// <summary>
        /// Email уже существет 
        /// </summary>
        ExistEmail = -2,

        /// <summary>
        /// Некорректный Email
        /// </summary>
        IvalidEmail = -3,

        /// <summary>
        /// Некорректный телефонный номер
        /// </summary>
        IvalidPhone = -4,

        /// <summary>
        /// Ошибка отправки Email
        /// </summary>
        ErrorSendEmail = -5,

        /// <summary>
        /// Неизвестная ошибка
        /// </summary>
        Error = -500,
    }
}