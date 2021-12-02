using Data.Extensions;
using Data.Interfaces.Repositories;
using Data.Models.DB.Account;
using Data.Models.Services;
using Data.Services.Account;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using MoreLinq;
using Data.Extensions.Files;

namespace Data.Services.DB
{
    public class InitDB
    {
        protected IBaseEntityRepository<User> _usersRepository;
        protected IBaseEntityRepository<Role> _rolesRepository;
        protected IBaseEntityRepository<UserRoles> _userRolesRepository;
        protected UserManager _userManager;
        protected AcceleratorContext _db;

        public InitDB(IBaseEntityRepository<User> usersRepository,
            IBaseEntityRepository<Role> rolesRepository,
            IBaseEntityRepository<UserRoles> userRolesRepository,
            UserManager userManager,
            AcceleratorContext db)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _db = db;
            _userManager = userManager;
            _userRolesRepository = userRolesRepository;
        }

        public void InitAuth()
        {
            InitRoles();
            InitUsers();
        }

        /// <summary>
        /// Добавление в БД базовых пользователей из конфигурации
        /// </summary>
        /// <returns></returns>
        private void InitUsers()
        {
            var users = GetBaseUsers();
            var usersDb = _usersRepository.GetListQuery()
                .Where(p => users.Select(x => x.User.Email).Contains(p.Email))
                .ToList();

            users.ForEach(p =>
            {
                var userDb = usersDb.FirstOrDefault(x => x.Email == p.User.Email);
                if (userDb == null)
                {
                    _userManager.SignUp(p.User, p.Password);
                }
            });
        }

        /// <summary>
        /// Получение базовых пользователей из конфигурации
        /// </summary>
        /// <returns></returns>
        private List<ServiceUser> GetBaseUsers()
        {
            var json = new FileInfo("../Data/Configurations/Data/Users.json").ReadFile();

            var allRoles = _rolesRepository.GetList();

            return JsonConvert.DeserializeAnonymousType(json, new[]
            {
                new
                {
                    FirstName = "",
                    LastName = "",
                    MiddleName = "",
                    Email = "",
                    Password = "",
                    Phone =  "",
                    Roles = Array.Empty<string>()
                }
            })!.Select(p =>
            {
                var roles = allRoles.Where(r => p.Roles.Contains(r.Name)).ToList();

                return new ServiceUser()
                {
                    User = new User()
                    {
                        Email = p.Email,
                        FirstName = p.FirstName,
                        MiddleName = p.MiddleName,
                        LastName = p.LastName,
                        EmailConfirmed = true,
                        PhoneNumber = p.Phone,

                        UserRoles = roles.Select(x => new UserRoles()
                        {
                            Role = x,
                            RoleGuid = x.Guid
                        }).ToList(),
                    },
                    Password = p.Password
                };
            }).ToList();
        }

        /// <summary>
        /// Добавление в БД базовых ролей из конфигурации
        /// </summary>
        /// <returns>Роли</returns>
        private void InitRoles()
        {
            var json = new FileInfo("../Data/Configurations/Data/Roles.json").ReadFile();

            var roles = JsonConvert.DeserializeObject<List<Role>>(json);

            var rolesDb = _rolesRepository.GetList().AsEnumerable();

            var newRoles = roles.ExceptBy(rolesDb, p => p.Name);

            _rolesRepository.AddRange(newRoles);
        }
    }
}