using Data.Enum.Account;
using Data.Extensions.Type;
using Data.Models.DB._BaseEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Data.Models.DB.Account
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public User()
        {
            // Инициализация серкетных отпечатков
            UpdateSecurityStamp();
            SecurityStampEmail = Guid.NewGuid().ToString().Hash();
        }

        #region Security

        private string _email;

        /// <summary>
        /// Email 
        /// </summary>
        public virtual string Email
        {
            get => _email;
            set
            {
                _emailNormalized = value.ToLower();
                _email = value;
            }
        }

        private string _emailNormalized;

        /// <summary>
        /// Нормализированный Email
        /// </summary>
        public virtual string EmailNormalized { get => _emailNormalized; set { } }

        /// <summary>
        /// Флаг подтвержден ли Email 
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// ХЭШ пароля
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// Секретный отпечаток
        /// Значение необходимое для смены пароля
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// Секретный отпечаток
        /// Значение необходимое для подтверждения Email
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public virtual string SecurityStampEmail { get; set; }

        /// <summary>
        /// Обновление секретного отпечатка (пароля)
        /// </summary>
        private void UpdateSecurityStamp()
        {
            SecurityStamp = Guid.NewGuid().ToString().Hash();
        }

        #endregion

        #region General Info

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

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

        /// <summary>
        /// Роли пользователя
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<UserRoles> UserRoles { get; set; }

        [NotMapped]
        public virtual IEnumerable<Role> Roles { get => UserRoles?.Select(p => p.Role); }

        #endregion

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AuthStatus> Validate()
        {
            var errors = new List<AuthStatus>();

            if (!new EmailAddressAttribute().IsValid(Email)) errors.Add(AuthStatus.IvalidEmail);

            return errors;
        }
    }
}