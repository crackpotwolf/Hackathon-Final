using Data.Extensions.Type;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions.DataTransfer
{
    /// <summary>
    /// Информация о запросе
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Вывод информации о запросе
        /// </summary>
        /// <param name="httpContext">Данные запроса</param>
        /// <returns>Информация о запросе. Содержит: метод, Path, параметры запроса</returns>
        public static string GetInfoAboutRequest(this HttpContext httpContext)
        {
            var req = httpContext.Request;
            var _params = req.Query.Select(p =>
            {
                return $"   {p.Key} = \"{p.Value}\"";
            }).Join('\n');

            return $"[{req.Method}] {req.Path}\n{_params}";
        }
    }
}