using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions.Files
{
    /// <summary>
    /// Работа с документами
    /// </summary>
    public static class DocumentManage
    {
        /// <summary>
        /// Проверка расширения файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns></returns>
        public static bool DocumentExtensionCheck(string fileName)
        {
            return Path.GetExtension(fileName) == ".pdf" || Path.GetExtension(fileName) == ".docx" ||
                   Path.GetExtension(fileName) == ".xlsx" || Path.GetExtension(fileName) == ".pptx";
        }

        /// <summary>
        /// Получить уникальное имя с временным штампом
        /// </summary>
        /// <returns></returns>
        public static string GenerateUniqueName()
        {
            const string format = "yyyy-MM-dd-HH-mm";
            return $"{Guid.NewGuid()}_{DateTime.UtcNow.ToString(format)}";
        }

        /// <summary>
        /// Получить уникальное имя с расширением
        /// </summary>
        /// <param name="name">Название файла</param>
        /// <returns></returns>
        public static string GenerateDocumentName(string name)
        {
            return $"{Guid.NewGuid()}{Path.GetExtension(name)}";
        }
    }
}