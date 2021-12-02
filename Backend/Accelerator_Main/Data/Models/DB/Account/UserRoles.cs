using Data.Models.DB._BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Account
{
    /// <summary>
    /// Связь пользователей и ролей
    /// </summary>
    public class UserRoles : BaseEntity
    {
        /// <summary>
        /// Guid роли
        /// </summary>
        public Guid RoleGuid { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Guid пользователя
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; }
    }
}