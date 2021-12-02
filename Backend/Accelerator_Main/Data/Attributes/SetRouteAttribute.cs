using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Attributes
{
    /// <summary>
    /// Реализация атрибута для задания маршрута до методов контроллера
    /// </summary>
    public class SetRouteAttribute : RouteAttribute
    {
        /// <inheritdoc />
        public SetRouteAttribute() : base(GetTemplate("v{version:ApiVersion}/[controller]"))
        {

        }

        /// <inheritdoc />
        public SetRouteAttribute(string template) : base(GetTemplate(template))
        {
        }

        /// <summary>
        /// Получение полного пути к методу API
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private static string GetTemplate(string template)
        {
            var templateReplace = new Regex(@"^/?api/").Replace(template, "");
            return $"/api/{Assembly.GetEntryAssembly()?.GetName()?.Name?.ToLower()}/{templateReplace}";
        }
    }
}