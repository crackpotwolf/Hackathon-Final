using DocumentFormat.OpenXml.Packaging;

namespace Parse_Documents.MSOfficeParsers
{
    /// <summary>
    /// Парсинг pptx
    /// </summary>
    public class PptxParser
    {
        private string _text;

        private void Recursive(DocumentFormat.OpenXml.OpenXmlElement element)
        {
            var type = element.GetType().ToString();

            try
            {
                if (type == "DocumentFormat.OpenXml.Drawing.Paragraph")
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
                var presDoc = PresentationDocument.Open(filepath, false);

                foreach (var slide in presDoc.PresentationPart.SlideParts)
                {
                    Recursive(slide.Slide);
                }

                presDoc.Close();
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