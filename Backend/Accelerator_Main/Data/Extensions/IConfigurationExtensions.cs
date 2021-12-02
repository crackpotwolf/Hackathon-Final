using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    /// <summary>
    /// Расширение Configuration
    /// </summary>
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Получение полного URL адреса (HOST+PATH)
        /// </summary>
        /// <param name="conf">Конфигурация</param>
        /// <param name="key">Название пути в конфигурации</param>
        /// <returns></returns>
        public static string GetFullUrlFrontEnd(this IConfiguration conf, string key)
        {
            var host = conf.GetValue<string>("Urls:Frontend:HOST");
            return $"{host}{conf.GetValue<string>($"Urls:Frontend:PATHS:{key}")}";
        }
    }
}