using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions.Files
{
    /// <summary>
    /// Расширения для работы с файлами
    /// </summary>
    public static class FileInfoExtension
    {
        /// <summary>
        /// Чтение из файла
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        public static string ReadFile(this FileInfo file)
        {
            if (!file.Exists) return null;

            // Чтение из файла
            using FileStream fstream = File.OpenRead(file.FullName);

            // Преобразуем строку в байты
            byte[] array = new byte[fstream.Length];

            // Считываем данные
            fstream.Read(array, 0, array.Length);

            // Декодируем байты в строку
            return System.Text.Encoding.Default.GetString(array);
        }

        /// <summary>
        /// Запись в файл
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="text">Текст</param>
        /// <param name="mode">Режим</param>
        /// <returns></returns>
        private static bool WriteToFile(this FileInfo file, string text, FileMode mode = FileMode.Create)
        {
            try
            {
                // Если папка отсутсвует
                if (!file.Directory.Exists)
                    file.Directory.Create();

                using FileStream fstream = new(file.FullName, mode, FileAccess.Write);

                // Преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(text);

                // Запись массива байтов в файл
                fstream.Write(array, 0, array.Length);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Перезапись файла
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="text">Текст</param>
        /// <returns></returns>
        public static bool WriteToFile(this FileInfo file, string text) => WriteToFile(file, text);

        /// <summary>
        /// Добавление текста в конце файла
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="text">Текст</param>
        /// <returns></returns>
        public static bool AppendToFile(this FileInfo file, string text) => WriteToFile(file, text, FileMode.Append);
    }
}