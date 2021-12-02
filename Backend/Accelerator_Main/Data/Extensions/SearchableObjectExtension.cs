using Data.Attributes;
using Data.Interfaces;
using Lucene.Net.Documents;
using Search_Data.Models;
using Search_Data.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{

    public static class SearchableObjectExtension
    {
        /// <summary>
        /// Получение строкового представления объекта для поиска
        /// </summary>
        //public static string GetSearchablePropertiesStringifiedValue(this ISearchable data)
        //{
        //    var res = new List<string>();

        //    foreach (PropertyInfo property in data.GetType().GetProperties()
        //        .Where(p => p.CustomAttributes.Select(m => m.AttributeType).Contains(typeof(SearchableAttribute))))
        //    {
        //        if(property.GetValue(data)!=null)
        //            res.Add(property.GetValue(data).ToString());
        //    }
        //    return String.Join(", ", res);
        //}

        /// <summary>
        /// Индексирование объекта
        /// </summary>
        /// <param name="data">объект</param>
        /// <param name="key">ключ объекта</param>
        /// <param name="search">объект поиска</param>
        //public static void AddSearchableObjectToIndex(this ISearchable data, Guid key, WordSearch search)
        //{
        //    TextDocument document = new TextDocument()
        //    {
        //        Guid = key,
        //        Text = data.GetSearchablePropertiesStringifiedValue()
        //    };

        //    search.AddIndex(document);
        //}

        /// <summary>
        /// Индексирование объекта (каждое поле отдельно)
        /// </summary>
        /// <param name="data">объект</param>
        /// <param name="key">ключ объекта</param>
        /// <param name="search">объект поиска</param>
        public static void AddSearchableObjectToIndexSeparately(this ISearchable data, Guid key, WordSearch search)
        {
            var document = new Document() 
            { 
                new StringField("Guid", $"{key}", Field.Store.YES),
                new StringField("ObjectType", $"{data.GetType().Name}", Field.Store.YES),
            };
            foreach (PropertyInfo property in data.GetType().GetProperties()
                .Where(p => p.CustomAttributes.Select(m => m.AttributeType).Contains(typeof(SearchableAttribute))))
            {
                var valueStr = property.GetValue(data);
                if (valueStr != null)
                {
                    document.Add(new TextField(property.Name, valueStr.ToString(), Field.Store.YES));
                }
            }

            search.AddIndex(document);
        }

        /// <summary>
        /// Получение массива полей, по которым может выполняться поиск
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string[] GetSerachableFieldsNames(this System.Type type)
        {
            var result = new List<string>();
            foreach (PropertyInfo property in type.GetProperties()
                .Where(p => p.CustomAttributes.Select(m => m.AttributeType).Contains(typeof(SearchableAttribute))))
            {
                result.Add(property.Name);
            }
            return result.ToArray();
        }
    }
}
