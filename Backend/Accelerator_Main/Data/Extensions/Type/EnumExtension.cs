using System;
using System.Linq;
using System.Reflection;

namespace Data.Extensions.Type
{
    /// <summary>
    /// Enum расширения
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Получение значения атрибута у Enum
        /// </summary>
        /// <typeparam name="T">Атрибут</typeparam>
        /// <typeparam name="Expected">Ожидаемое значение</typeparam>
        /// <param name="enumeration">Enum</param>
        /// <param name="expression">expression для получения значения атрибута</param>
        /// <returns>Значение атрибута или дефолтное значение</returns>
        public static Expected GetAttributeValue<T, Expected>(this System.Enum enumeration, Func<T, Expected> expression)
    where T : Attribute
        {
            T? attribute =
              enumeration
                .GetType()
                .GetMember(enumeration.ToString())
                .Where(member => member.MemberType == MemberTypes.Field)
                .FirstOrDefault()
                ?.GetCustomAttributes(typeof(T), false)
                .Cast<T>()
                .SingleOrDefault();

            if (attribute == null)
                return default(Expected);

            return expression(attribute);
        }
    }
}