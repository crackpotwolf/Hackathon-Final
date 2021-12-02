using DocumentFormat.OpenXml.Packaging;

namespace Parse_Documents.MSOfficeParsers
{
    /// <summary>
    /// Парсинг Docx
    /// </summary>
    public class DocxParser
    {
        private string _text;

        private int _termIdc = 1;
        private int _defIdc = 2;
        private string[] _words = { "№", "номер" };

        private void Recursive(DocumentFormat.OpenXml.OpenXmlElement element)
        {
            var type = element.GetType().ToString();

            try
            {
                if (type == "DocumentFormat.OpenXml.Wordprocessing.Paragraph")
                {
                    _text += " " + element.InnerText;
                }
                else
                {
                    foreach (var child in element.ChildElements)
                    {
                        Recursive(child);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Ошибка: {ex}");
            }
        }

        /// <summary>
        /// Чтение документа
        /// </summary>
        /// <param name="filepath">Путь к исходному файлу</param>
        /// <returns></returns>
        public void Read(string filepath)
        {
            _text = "";

            try
            {
                var doc = WordprocessingDocument.Open(filepath, false);

                foreach (var child in doc.MainDocumentPart.Document.Body.ChildElements)
                {
                    Recursive(child);
                }
                doc.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Central Directory corrupt.")
                    throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Получение текста документа
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public string Text()
        {
            return _text;
        }
    }
}