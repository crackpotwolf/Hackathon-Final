using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations.Account
{
    /// <summary>
    /// Параметры токена
    /// </summary>
    public static class AuthOptions
    {
        /// <summary>
        /// Роль
        /// </summary>
        public const string BaseRole = "user";

        /// <summary>
        /// Издатель токена
        /// </summary>
        public const string ISSUER = "Accelerator";

        /// <summary>
        /// Потребитель токена
        /// </summary>
        public const string AUDIENCE = "API";

        /// <summary>
        /// Ключ для шифрации
        /// </summary>
        const string KEY = "234kjs^%&*@&$ogh234joKLGasfhjgijbflvjbarfvdfgjhl;hkjhkhskjf$";

        /// <summary>
        /// Время жизни токена в минутах
        /// </summary>
        public const int LIFETIME = 60 * 24 * 365;

        /// <summary>
        /// Получить ключ
        /// </summary>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}