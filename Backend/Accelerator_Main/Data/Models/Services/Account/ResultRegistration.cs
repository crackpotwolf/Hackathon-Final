using Data.Enum.Account;
using Data.Models.DB.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Services
{
    /// <summary>
    /// Результат Registration
    /// </summary>
    public class ResultRegistration
    {
        /// <summary>
        /// Токен
        /// </summary>
        public Token Token { get; set; }

        /// <summary>
        /// Ошибки
        /// </summary>
        public List<AuthStatus> Errors { get; set; } = new List<AuthStatus>();
    }
}