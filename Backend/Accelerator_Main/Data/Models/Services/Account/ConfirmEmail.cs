using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Services
{
    /// <summary>
    /// Подтверждение Email
    /// </summary>
    public class ConfirmEmail
    {
        /// <summary>
        /// Guid пользователя
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Секретный отпечаток
        /// Значение необходимое для подтверждения Email
        /// </summary>
        public string SecurityStampEmail { get; set; }
    }
}