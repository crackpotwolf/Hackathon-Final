using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions.Reflection
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Получение имя сборки, разделенной тире
        /// Например: 
        ///     AssemblyExtensions -> assembly-extensions
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetNameDashCase(this AssemblyName assembly)
        {
            var assemblyName = assembly.Name.Replace("_", "");
            assemblyName = string.Concat((assemblyName ?? string.Empty).Select((x, i) => i > 0 && char.IsUpper(x) && !char.IsUpper(assemblyName[i - 1]) ? $"-{x}" : x.ToString())).ToLower();
            return assemblyName;
        }
    }
}
