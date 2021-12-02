using Data.Enum.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Services
{
    /// <summary>
    /// Результат Creation
    /// </summary>
    public class ResultCreation
    {
        /// <summary>
        /// Guid пользователя
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Ошибки
        /// </summary>
        public List<AuthStatus> Errors { get; set; } = new List<AuthStatus>();
    }
}