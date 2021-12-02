using AutoMapper;
using Data.Models.DB.Account;
using Data.Models.DB.Project;
using Data.Models.ModelViews.Account;
using Data.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.DB
{
    /// <summary>
    /// Мапирование 
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Мапирование 
        /// </summary>
        public MappingProfile()
        {
            CreateMap<UserRegister, User>();
            CreateMap<User, UserRegister>();

            CreateMap<UserCreation, User>();
            CreateMap<UserUpdateInfo, User>();

            #region Проекты

            #endregion
        }
    }
}