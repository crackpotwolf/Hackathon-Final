using Data.Extensions.DataTransfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Data.Middleware
{
    /// <summary>
    /// Логирование начала выполнения запроса и его завершение
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public LoggingMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            _next = next;
            _logger = logger.CreateLogger<LoggingMiddleware>();
        }

        /// <summary>
        /// Запрос
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var req = context.GetInfoAboutRequest();
            _logger.LogInformation(req);

            try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {
                // Произошло исключение на этапе выполнения запроса
                _logger.LogError($"{req}\n{GetInformationError(context, ex)}");
                ExceptionDispatchInfo.Capture(ex).Throw();
            }

            // Если не было исключения и запрос отработал нормально
            if (context.Response.StatusCode == 200)
            {
                _logger.LogInformation($"{req}\nЗапрос успешно завершен");
            }
            else
            {
                _logger.LogError($"{req}\n{GetInformationError(context)}");
            }
        }

        /// <summary>
        /// Ошибка получения информации
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string GetInformationError(HttpContext context, Exception ex = null)
        {
            if (ex == null)
            {
                return $"Запрос завершился с [{context.Response.StatusCode}] кодом ошибки.";
            }
            else
            {
                return $"Запрос завершился с исключением. Информация об ошибке:\n{ex}";
            }
        }
    }
}