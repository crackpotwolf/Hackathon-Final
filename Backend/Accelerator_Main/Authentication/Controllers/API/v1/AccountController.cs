using AutoMapper;
using Data.Attributes;
using Data.Enum.Account;
using Data.Extensions.Type;
using Data.Interfaces.Repositories;
using Data.Models.DB.Account;
using Data.Models.ModelViews.Account;
using Data.Models.Services;
using Data.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Controllers.API.v1
{
    /// <summary>
    /// Управление аккаунтом
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [DisplayName("account")]
    [SetRoute]
#if RELEASE
    [Authorize(Roles = "Admin")]
#endif
    public class AccountController : Controller
    {
        protected IBaseEntityRepository<User> _usersRepository;
        protected IBaseEntityRepository<Role> _rolesRepository;
        protected readonly EmailService _emailService;
        private readonly ILogger<IndexModel> _logger;
        protected readonly UserManager _userManager;
        protected readonly IMapper _mapper;

        public AccountController(IBaseEntityRepository<User> usersRepository,
            IBaseEntityRepository<Role> rolesRepository,
            EmailService emailService,
            ILogger<IndexModel> logger,
            UserManager userManager,
            IMapper mapper)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _emailService = emailService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        protected IQueryable<User> _users => _usersRepository
                .GetListQuery()
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.Role);

        /// <summary>
        /// Авторизация - получение токена доступа
        /// </summary>
        /// <param name="model">Логин и пароль</param>
        /// <returns></returns>
        [HttpPost("token")]
        [AllowAnonymous]
        [SwaggerResponse(200, "Токен", typeof(Token))]
        [SwaggerResponse(400, "Пользователь не существует или неверный пароль")]
        public IActionResult Token(Login model)
        {
            try
            {
                var token = _userManager.GetToken(model.Email, model.Password);

                if (token == null)
                    return BadRequest(new { errorText = "Неправильный логин или пароль." });

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось получить токен. " +
                    $"Ошибка: {ex}");

                return StatusCode(500, $"Не удалось получить токен");
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userGuid">Идентификатор пользователя</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveUser(Guid userGuid)
        {
            try
            {
                return _userManager.RemoveUser(userGuid) ? Ok() : StatusCode(500);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось удалить пользователя. " +
                    $"Ошибка: {ex}");

                return StatusCode(500, $"Не удалось удалить пользователя");
            }
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="model">Информация о пользователе</param>
        /// <returns>Токен доступа</returns>
        [HttpPost("creation")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(200, "Токен", typeof(Token))]
        [SwaggerResponse(400, "Неверные данные или пользователь уже сущестует. Содержит информацию об ошибках.", typeof(List<AuthStatus>))]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public async Task<IActionResult> RegistrationAsync(UserRegister model)
        {
            try
            {
                var user = _mapper.Map<User>(model);

                var res = await _userManager.RegisterUserAsync(user, model.Password);

                if (res.Errors.Count != 0)
                    return BadRequest(res.Errors);

                return Ok(res.Token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось зарегистрировать пользователя. " +
                    $"Ошибка: {ex}");

                return StatusCode(500, $"Не удалось зарегистрировать пользователя");
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model">Информация о пользователе</param>
        /// <returns></returns>
        [HttpPost("registration")]
        [AllowAnonymous]
        [SwaggerResponse(200, "Guid нового пользователя, на его email отправлена ссылка для установки пароля", typeof(Guid))]
        [SwaggerResponse(400, "Неверные данные или пользователь уже существует. Содержит информацию об ошибках", typeof(List<AuthStatus>))]
        [SwaggerResponse(500, "Неизвестная ошибка")]
        public async Task<IActionResult> CreationAsync(UserCreation model)
        {
            try
            {
                // Название роли внешнего пользователя
                var nameExternalUser = "External";

                var roles = _rolesRepository.GetListQuery()
                    .Where(r => r.Name.Contains(nameExternalUser))
                    .ToList();

                var user = _mapper.Map<User>(model);

                var res = await _userManager.CreateUserAsync(user, roles);

                if (res.Errors.Count != 0)
                    return BadRequest(res.Errors);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось зарегистрировать пользователя. " +
                    $"Ошибка: {ex}");

                return StatusCode(500, $"Не удалось зарегистрировать пользователя");
            }
        }

        /// <summary>
        /// Пинг
        /// </summary>
        /// <returns></returns>
        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping() => Ok("Pong");

        /// <summary>
        /// Пинг
        /// </summary>
        /// <returns></returns>
        [HttpGet("ping-auth")]
        public IActionResult PingAuth() => Ok($"Hello {User.Identity.Name}");

        /// <summary>
        /// Сброс пароля
        /// </summary>
        /// <param name="email">Email адрес пользователя</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        [SwaggerResponse(200, "Ссылка на восстановление пароля успешно отправлена на Email пользователя")]
        [SwaggerResponse(400, "Не указан Email")]
        [SwaggerResponse(500, "Ошибка при сбросе пароля", typeof(ResultUserManager))]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return BadRequest();

                var res = await _userManager.ForgotPasswordAsync(email);

                if (res != ResultUserManager.Success)
                    return StatusCode(500, res.GetAttributeValue<DisplayAttribute, string>(p => p.Description));

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось сбросить пароль пользователя. " +
                    $"Ошибка: {ex}");

                return StatusCode(500, $"Не удалось сбросить пароль пользователя");
            }
        }

        /// <summary>
        /// Подтверждление адреса электронной почты
        /// </summary>
        /// <param name="model">Информация для подтверждения Email</param>
        /// <returns></returns>
        [HttpPost("confirm-email")]
        [AllowAnonymous]
        [SwaggerResponse(200, "Email успешно подтвержден")]
        [SwaggerResponse(400, "Пользователь не найден")]
        [SwaggerResponse(500, "Нередвиденная ошибка")]
        public IActionResult ConfirmEmail(ConfirmEmail model)
        {
            switch (_userManager.ConfirmEmail(model))
            {
                case ResultUserManager.Success:
                    return Ok();
                case ResultUserManager.UserNotFound:
                    return BadRequest(ResultUserManager.UserNotFound);
                default:
                    return StatusCode(500);
            }
        }

        /// <summary>
        /// Установка пароля для нового пользователя
        /// </summary>
        /// <param name="model">Данные для установки пароля</param>
        /// <returns></returns>
        [HttpPost("set-password")]
        [AllowAnonymous]
        [SwaggerResponse(200, "Пароль успешно установлен")]
        [SwaggerResponse(400, "Не удалось установить пароль. Содержит информацию об ошибке", typeof(string))]
        public IActionResult SetPassword(SetPassword model)
        {
            var res = _userManager.SetPassword(model);
            if (res == ResultUserManager.Success)
            {
                var email = _usersRepository.Get(model.UserGuid).Email;
                return Ok(_userManager.GetToken(email, model.Password));
            }

            return BadRequest(res);
        }

        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        /// <param name="model">Данные для смены пароля</param>
        /// <returns></returns>
        [HttpPost("change-password")]
        [AllowAnonymous]
        [SwaggerResponse(200, "Пароль успешно изменен")]
        [SwaggerResponse(400, "Не удалось изменить пароль. Содержит информацию об ошибке", typeof(string))]
        public IActionResult ChangePassword(ForgotPassword model)
        {
            var res = _userManager.ChangePassword(model);
            if (res == ResultUserManager.Success)
                return Ok();

            return BadRequest(res.GetAttributeValue<DisplayAttribute, string>(x => x.Description));
        }

        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        /// <param name="oldPassword">Текущий пароль пользваотеля</param>
        /// <param name="password">Новый пароль пользователя</param>
        /// <param name="confirmPassword">Подтверждение нового пароля</param>
        /// <returns></returns>
        [HttpPost("update-password")]
        [SwaggerResponse(200, "Пароль успешно изменен")]
        [SwaggerResponse(400, "Не удалось изменить пароль. Содержит информацию об ошибке", typeof(string))]
        [Authorize]
        public IActionResult ChangePassword(string oldPassword, string password, string confirmPassword)
        {
            var res = ResultUserManager.InvalidConfirmPassword;

            if (password == confirmPassword)
            {
                res = _userManager.ChangePassword(GetUserGuid(), oldPassword, password);
                if (res == ResultUserManager.Success)
                    return Ok();
            }

            return BadRequest(res.GetAttributeValue<DisplayAttribute, string>(x => x.Description));
        }

        /// <summary>
        /// Информация о текущем пользователе
        /// </summary>
        /// <returns></returns>
        [HttpGet("user-info")]
        [SwaggerResponse(200, "Информация о пользователе", typeof(User))]
        [Authorize(Roles = "Admin")]
        public IActionResult UserInfo()
        {
            return Ok(_users.FirstOrDefault(p => p.Guid == GetUserGuid()));
        }

        /// <summary>
        /// Информация о пользователях по списку Guid
        /// </summary>
        /// <param name="guids">Список Guid пользователей</param>
        /// <returns></returns>
        [HttpPost("users-info")]
        [SwaggerResponse(200, "Информация о пользователях", typeof(List<User>))]
        [Authorize(Roles = "Admin")]
        public IActionResult UsersInfo(List<Guid> guids)
        {
            return Ok(_users.Where(p => guids.Contains(p.Guid)));
        }

        /// <summary>
        /// Обновление информации
        /// </summary>
        /// <param name="model">Обновленная информация о пользователе</param>
        /// <returns></returns>
        [HttpPut("update-user-info")]
        [SwaggerResponse(200, "Обновленная информация о пользователе", typeof(User))]
        [SwaggerResponse(500, "Произошла ошибка")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUserInfo(UserUpdateInfo model)
        {
            var user = _mapper.Map(model, GetUser());

            if (_usersRepository.Update(user))
                return Ok(user);
            
            return StatusCode(500);
        }

        /// <summary>
        /// Присваивание ролей пользователю
        /// </summary>
        /// <param name="guid">Идентификатор пользователя</param>
        /// <param name="roles">Список ролей пользователя</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("set-roles/{guid}")]
        [SwaggerResponse(200, "Роли успешно обновлены")]
        [SwaggerResponse(400, "Пользователь не найден")]
        [SwaggerResponse(500, "Произошла ошибка")]
        public IActionResult ChangeRoleUser(Guid guid, [FromBody] List<string> roles)
        {
            switch (_userManager.ChangeRolesUser(guid, roles))
            {
                case ResultUserManager.Success:
                    return Ok();
                case ResultUserManager.UserNotFound:
                    return StatusCode(400, ResultUserManager.UserNotFound);
                default:
                    return StatusCode(500);
            }
        }


        /// <summary>
        /// Список ролей пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        [SwaggerResponse(200, "Список ролей пользователей", typeof(List<Role>))]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRolesList()
        {
            return Ok(_rolesRepository.GetListQuery()
                .Where(p => !p.IsSecret));
        }

        /// <summary>
        /// Список существующих пользователей
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("users")]
        [SwaggerResponse(200, "Список пользователей", typeof(List<User>))]
        public IActionResult GetUsers()
        {
            var users = _usersRepository.GetListQuery()
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.Role);

            return Ok(_users);
        }

        #region Вспомогательные методы

        /// <summary>
        /// Получение Guid текущего пользователя
        /// </summary>
        /// <returns></returns>
        [NonAction]
        protected Guid GetUserGuid()
        {
            return new Guid(User.Claims.FirstOrDefault(p => p.Type == "guid")?.Value);
        }

        /// <summary>
        /// Получение текущего пользователя из БД
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public User GetUser()
        {
            return _usersRepository.Get(GetUserGuid());
        }

        #endregion
    }
}
