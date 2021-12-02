using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Extensions.Type
{
    /// <summary>
    /// Расширения для работы с строкой
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Получение ХЭШа строки
        /// </summary>
        /// <param name="s">Строка</param>
        /// <returns></returns>
        public static string Hash(this string s)
        {
            // Используем входную строку для вычисления хеша MD5
            using MD5 md5 = MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(s);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Преобразуем массив байтов в шестнадцатеричную строку
            StringBuilder sb = new();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Получение всех цифр в строке
        /// </summary>
        /// <param name="s">Строка из которой необходимо выбрать цифры</param>
        /// <returns>Все цифры в строке</returns>
        public static string OnlyDigits(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            return ExceptDigits.Replace(s, "");
        }

        /// <summary>
        /// Вставить подстроку в строку, если есть возможность
        /// </summary>
        /// <param name="s">Исходная строка</param>
        /// <param name="startIndex">Индекс для вставки</param>
        /// <param name="value">Подстрока</param>
        /// <returns></returns>
        public static string InsertIfPossible(this string s, int startIndex, String value)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= startIndex) return s;
            return s.Insert(startIndex, value);
        }

        /// <summary>
        /// Вырезать подстроку, если это возможно
        /// </summary>
        /// <param name="s">Исходная строка</param>
        /// <param name="startIndex">Индек начала вырезки подстроки</param>
        /// <param name="length">Длина подстроки</param>
        /// <returns>Подстрока или пустая строка</returns>
        public static string SubstringIfPossible(this string s, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= startIndex + length) return "";
            return s.Substring(startIndex, length);
        }

        /// <summary>
        /// Camelcase - первая буква строчного регистра
        /// </summary>
        /// <param name="s">Исходная строка</param>
        /// <returns>Преобразованная строка</returns>
        public static string FirstLower(this string s)
        {
            if (!string.IsNullOrWhiteSpace(s) || char.IsLower(s[0]))
                return s;
            return char.ToLower(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// Кроме цифр
        /// </summary>
        private static Regex ExceptDigits = new("[^0-9]+");
    }
}