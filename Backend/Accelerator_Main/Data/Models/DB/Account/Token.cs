using Data.Enum.Account;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Data.Models.DB.Account
{
    /// <summary>
    /// Токен пользователя
    /// </summary>
    public class Token
    {
        public Token()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="status"></param>
        public Token(AuthStatus status) => Status = status;

        /// <summary>
        /// Токен
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; }

        /// <summary>
        /// Код ошибки при получении токена или регистрации
        /// 0  - успешно
        /// -1 - не правильный пароль или логин
        /// -2 - пользователь с таким Email уже существует 
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        private AuthStatus Status { get; set; }
    }
}