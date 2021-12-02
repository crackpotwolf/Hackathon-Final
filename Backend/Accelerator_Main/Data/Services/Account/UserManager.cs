using Data.Configurations.Account;
using Data.Enum.Account;
using Data.Extensions;
using Data.Extensions.Type;
using Data.Models.DB.Account;
using Data.Models.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services.Account
{
    /// <summary>
    /// Менеджер управления пользователям
    /// </summary>
    public class UserManager
    {
        private readonly AcceleratorContext db;
        private readonly ILogger<UserManager> _logger;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        private IQueryable<User> _users => db.Users.Where(p => !p.IsDelete);

        public UserManager(AcceleratorContext db,
            ILogger<UserManager> logger,
            EmailService emailService,
            IConfiguration configuration)
        {
            this.db = db;
            _logger = logger;
            _emailService = emailService;
            _configuration = configuration;
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="guid">Идентификатор пользователя</param>
        /// <returns></returns>
        public bool RemoveUser(Guid guid)
        {
            try
            {
                var user = _users.FirstOrDefault(p => p.Guid == guid);
                if (user == null) return true;

                user.IsDelete = true;

                db.Update(user);
                db.SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка при удалении пользователя, Guid: {guid}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// Изменение ролей пользователя
        /// </summary>
        /// <param name="guid">Идентификатор пользователя</param>
        /// <param name="roles">Полный список ролей</param>
        /// <returns></returns>
        public ResultUserManager ChangeRolesUser(Guid guid, IEnumerable<Role> roles)
        {
            try
            {
                if (!_users.Any(p => p.Guid == guid)) 
                    return ResultUserManager.UserNotFound;

                var user = _users
                    .Include(p => p.UserRoles)
                    .FirstOrDefault(p => p.Guid == guid);

                db.UserRoles.RemoveRange(user.UserRoles);
                db.SaveChanges();

                db.UserRoles.AddRange(roles.Where(p => !p.IsSecret)
                    .Select(p => new UserRoles()
                    {
                        RoleGuid = p.Guid,
                        UserGuid = user.Guid
                    }));
                
                db.SaveChanges();
                return ResultUserManager.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка при присвоении ролей ([{roles.Select(p => p.Name).Join(", ")}]) пользователю, Guid: {guid}\n{ex}");
            }

            return ResultUserManager.Error;
        }

        /// <summary>
        /// Изменение ролей пользователя
        /// </summary>
        /// <param name="guid">Идентификатор пользователя</param>
        /// <param name="roles">Полный список ролей</param>
        /// <returns></returns>
        public ResultUserManager ChangeRolesUser(Guid guid, List<string> roles)
        {
            return ChangeRolesUser(guid, db.Roles
                .Where(p => (roles.Contains(p.Name) || roles.Contains(p.Guid.ToString())) && !p.IsSecret)
                .ToList());
        }

        /// <summary>
        /// Регистрация пользователя в системе
        /// </summary>
        /// <param name="user">Информация о пользователе</param>
        /// <param name="password">Пароль</param>
        /// <returns>
        /// Информация о пользователе - в случае успеха
        /// null - если пользователь с таким Email уже существует
        /// </returns>
        public User SignUp(User user, string password)
        {
            using var transaction = db.Database.BeginTransaction();

            try
            {
                // Если пользователь уже существует - вернуть null
                if (_users.Any(p => p.Email == user.Email)) 
                    return null;

                // Подготовка
                var roles = user.UserRoles.ToList();
                user.UserRoles = null;
                user.PasswordHash = GetPasswordHash(password);

                //Добавление пользователя в БД
                db.Add(user);
                db.SaveChanges();

                // Назначение ролей пользователю
                roles.ForEach(p =>
                {
                    p.Role = null;
                    p.User = null;
                    p.UserGuid = user.Guid;
                });

                db.AddRange(roles);
                db.SaveChanges();

                user.UserRoles = roles;

                transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError($"При регистрации пользователя произошла ошибка: {ex}");
                transaction.Rollback();
            }

            return user;
        }

        /// <summary>
        /// Получение информации о пользователе, необходимой для формирования токена
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        private ClaimsIdentity GetIdentity(string email, string password)
        {
            var passwordHash = GetPasswordHash(password);

            // Информация о пользователе
            var user = _users
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.Role)
                .FirstOrDefault(x => x.Email == email && x.PasswordHash == passwordHash);

            if (user == null) 
                return null;

            // Параметры токена
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim("guid", user.Guid.ToString())
            };

            // Добавить роли в токен
            const string? typeRole = "Role";

            user.UserRoles.Select(p => p.Role.Name)
                .ToList()
                .ForEach(p => claims.Add(new Claim(typeRole, p)));

            ClaimsIdentity claimsIdentity = new(claims, "Token", ClaimsIdentity.DefaultNameClaimType, typeRole);

            return claimsIdentity;
        }

        /// <summary>
        /// Получение токена доступа
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public Token GetToken(string email, string password)
        {
            var identity = GetIdentity(email, password);

            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;

            // Создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new Token()
            {
                AccessToken = encodedJwt,
                Login = identity.Name
            };
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="user">Информация о пользователе</param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<ResultCreation> CreateUserAsync(User user, List<Role> roles = null)
        {
            using var transaction = db.Database.BeginTransaction();

            try
            {

                #region Проверка данных пользователя

                var res = new ResultCreation();

                if (_users.Any(p => p.Email == user.Email)) 
                    res.Errors.Add(AuthStatus.ExistEmail);

                if (res.Errors.Count != 0) 
                    return res;

                #endregion
                //user.PasswordHash = GetPasswordHash("fsdhgsdfkghdlsjkhjdih1298ry187vh39ugh1239gh931rnjg-79342rhg3j280gu34bn49hb942bj94732hb");
                db.Add(user);
                db.SaveChanges();

                #region Присваивание базовой роли

                if (roles == null) 
                    roles = new List<Role>();
                    
                roles.Add(db.Roles.FirstOrDefault(p => p.Name == AuthOptions.BaseRole));
                    
                var userRoles = roles.DistinctBy(p => p.Name)
                    .Select(p => new UserRoles()
                    {
                        RoleGuid = p.Guid,
                        UserGuid = user.Guid
                    })
                    .ToList();

                db.AddRange(userRoles);
                db.SaveChanges();

                #endregion

                #region Отправка Email для подтверждения почты

                var uri = QueryHelpers.AddQueryString(_configuration.GetFullUrlFrontEnd("RegPassword"), new Dictionary<string, string>()
                {
                    {nameof(Models.Services.SetPassword.SecurityStampEmail).FirstLower(), user.SecurityStampEmail },
                    {nameof(Models.Services.SetPassword.UserGuid).FirstLower(), user.Guid.ToString() },
                });

                // TODO: Исправление сообщений, в виде: "Вас зарегистрировали в системе..."
                var resultEmailSend = await EmailService.SendEmailAsync(user.Email,
                    "Создание учетной записи",
                    $"Вы создали учетную запись. Для установки пароля перейдите по ссылке: <a href=\"{uri}\">Установка пароля</a>");

                #endregion

                if (resultEmailSend.IsSuccess)
                {
                    transaction.Commit();
                }
                else
                {
                    _logger.LogError($"При создании пользователя произошла ошибка: {resultEmailSend.Exception}");
                    transaction.Rollback();
                    res.Errors.Add(AuthStatus.ErrorSendEmail);
                }
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"При создании пользователя произошла неизвестная ошибка: {ex}");
                return new ResultCreation() { Errors = { AuthStatus.Error } };
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="user">Информация о пользователе</param>
        /// <param name="password">Пароль</param>
        /// <returns>Токен доступа</returns>
        public async Task<ResultRegistration> RegisterUserAsync(User user, string password, List<Role> roles = null)
        {
            using (var transaction = db.Database.BeginTransaction())
                
            {
                try
                {
                    #region Проверка на ошибки

                    var res = new ResultRegistration();

                    res.Errors.AddRange(user.Validate());

                    if (_users.Any(p => p.Email == user.Email)) 
                        res.Errors.Add(AuthStatus.ExistEmail);

                    if (res.Errors.Count != 0) 
                        return res;
                    
                    #endregion

                    user.PasswordHash = GetPasswordHash(password);
                    db.Add(user);
                    db.SaveChanges();

                    #region Присваивание базовой роли

                    if (roles == null) roles = new List<Role>();
                        roles.Add(db.Roles.FirstOrDefault(p => p.Name == AuthOptions.BaseRole));

                    var userRoles = roles.DistinctBy(p => p.Name)
                        .Select(p => new UserRoles()
                        {
                            RoleGuid = p.Guid,
                            UserGuid = user.Guid
                        })
                        .ToList();

                    db.AddRange(userRoles);
                    db.SaveChanges();

                    #endregion

                    #region Отправка Email для подтверждения почты
                    
                    var uri = QueryHelpers.AddQueryString(_configuration.GetFullUrlFrontEnd("ConfirmEmail"), new Dictionary<string, string>()
                    {
                        {nameof(Models.Services.ConfirmEmail.SecurityStampEmail).FirstLower(), user.SecurityStampEmail },
                        {nameof(Models.Services.ConfirmEmail.UserGuid).FirstLower(), user.Guid.ToString() },
                    });

                    var resultEmailSend = await EmailService.SendEmailAsync(user.Email,
                        "Подтверждение почты",
                        $"Для смены пароля перейдите по ссылке: <a href=\"{uri}\">Сброс пароля</a>");

                    //res.Token = GetToken(user.Email, password);

                    if (resultEmailSend.IsSuccess)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        _logger.LogError($"При отправке письма произошла ошибка: {resultEmailSend.Exception}");
                        transaction.Rollback();

                        res.Errors.Add(AuthStatus.ErrorSendEmail);
                    }

                    #endregion

                    return res;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"При регистрации пользователя произошла неизвестная ошибка: {ex}");
                    transaction.Rollback();

                    return new ResultRegistration() { Errors = { AuthStatus.Error } };
                }
            }
        }

        /// <summary>
        /// Отправка сообщения на Email с ссылкой для сброса пароля
        /// </summary>
        /// <param name="email">Email адрес пользователя</param>
        /// <returns></returns>
        public async Task<ResultUserManager> ForgotPasswordAsync(string email)
        {
            try
            {
                // Получение пользователя
                var user = _users
                    .FirstOrDefault(p => p.EmailNormalized == email.ToLower());

                if (user == null) 
                    return ResultUserManager.UserNotFound;

                if (!user.EmailConfirmed) 
                    return ResultUserManager.EmailNotConfirmed;

                var uri = QueryHelpers.AddQueryString(_configuration.GetFullUrlFrontEnd("PasswordRecovery"), new Dictionary<string, string>()
                {
                    {nameof(ForgotPassword.SecurityStamp).FirstLower(), user.SecurityStamp },
                    {nameof(ForgotPassword.UserGuid).FirstLower(), user.Guid.ToString() },
                });

                await EmailService.SendEmailAsync(user.Email,
                    "Сброс пароля",
                    $"Для смены пароля перейдите по ссылке: <a href=\"{uri}\">Сброс пароля</a>");

                return ResultUserManager.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{email}] Произошла ошибка при сбросе пароля: {ex}");
                return ResultUserManager.Error;
            }
        }

        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        /// <param name="confirm">Данные для подтверждения Email</param>
        public ResultUserManager ConfirmEmail(ConfirmEmail confirm)
        {
            try
            {
                // Получение пользователя
                var user = _users
                    .FirstOrDefault(p => p.Guid == confirm.UserGuid && p.SecurityStampEmail == confirm.SecurityStampEmail);

                if (user == null) 
                    return ResultUserManager.UserNotFound;

                user.EmailConfirmed = true;
                db.SaveChanges();

                return ResultUserManager.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"При подтверждении Email [{confirm.UserGuid}] произошла неизвестная ошибка: \n{ex}");
                return ResultUserManager.Error;
            }
        }

        /// <summary>
        /// Установка пароля нового пользователя
        /// </summary>
        /// <param name="model">Данные для подтверждения пользователя и установки пароля</param>
        /// <returns></returns>
        public ResultUserManager SetPassword(SetPassword model)
        {
            try
            {
                // Получение пользователя
                var user = _users
                    .FirstOrDefault(p => p.Guid == model.UserGuid && p.SecurityStampEmail == model.SecurityStampEmail);

                if (user == null) 
                    return ResultUserManager.UserNotFound;

                user.EmailConfirmed = true;
                user.PasswordHash = GetPasswordHash(model.Password);

                db.SaveChanges();

                return ResultUserManager.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"При установке пароля (Guid: \"{model.UserGuid}\") произошла ошибка: \n{ex}");
                return ResultUserManager.Error;
            }
        }

        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        /// <param name="guid">Идентификатор пользователя</param>
        /// <param name="oldPassword">Старый пароль</param>
        /// <param name="newPassword">Новый пароль</param>
        public ResultUserManager ChangePassword(Guid guid, string oldPassword, string newPassword)
        {
            try
            {
                // Получение пользователя
                var user = _users
                    .FirstOrDefault(p => p.Guid == guid);

                if (user == null || !user.EmailConfirmed) 
                    return ResultUserManager.UserNotFound;

                // Проверка старого пароля
                if (user.PasswordHash != GetPasswordHash(oldPassword)) 
                    return ResultUserManager.InvalidCurrentPassword;

                user.PasswordHash = GetPasswordHash(newPassword);
                //user.UpdateSecurityStamp();

                db.SaveChanges();

                return ResultUserManager.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"При смене пароля (Guid: \"{guid}\") произошла ошибка: \n{ex}");
                return ResultUserManager.Error;
            }
        }

        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        /// <param name="model">Информация для смены пароля</param>
        public ResultUserManager ChangePassword(ForgotPassword model)
        {
            try
            {
                // Получение пользователя
                var user = _users
                    .FirstOrDefault(p => p.Guid == model.UserGuid);

                if (user == null || !user.EmailConfirmed) 
                    return ResultUserManager.UserNotFound;

                // Проверка старого пароля
                if (user.SecurityStamp != model.SecurityStamp) 
                    return ResultUserManager.SecurityStampIncorrect;

                user.PasswordHash = GetPasswordHash(model.Password);
                //user.UpdateSecurityStamp();

                db.SaveChanges();

                return ResultUserManager.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"При смене пароля (Guid: \"{model.UserGuid}\") произошла ошибка: \n{ex}");
                return ResultUserManager.Error;
            }
        }

        #region Вспомогательные функции

        /// <summary>
        /// Возвращает хэш от пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        private string GetPasswordHash(string password)
        {
            return password.Hash();
        }

        /// <summary>
        /// Генерация пароля
        /// </summary>
        /// <param name="length">Длина пароля</param>
        /// <returns></returns>
        public string CreatePassword(int length = 10)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new();
            Random rnd = new();

            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }

        #endregion
    }
}
