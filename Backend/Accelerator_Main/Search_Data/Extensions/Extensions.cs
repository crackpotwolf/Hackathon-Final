using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace Search_Data.Extensions 
{
	/// <summary>
	/// Расширения
	/// </summary>
	internal static class Extensions
    {
        /// <summary>
        /// Проверка на лист
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

		/// <summary>
		/// Для каждого индексного документа 
		/// </summary>
		public static void ForEachTermDocs(this IndexWriter writer, Term whereTerm, string[] fields, Action<Document> todo)
		{
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (whereTerm == null)
				throw new ArgumentNullException(nameof(whereTerm));
			if (fields == null)
				throw new ArgumentNullException(nameof(fields));
			if (todo == null)
				throw new ArgumentNullException(nameof(todo));

			using (var reader = writer.GetReader(true))
			{
				var docs = MultiFields.GetTermDocsEnum(reader, null, whereTerm.Field, whereTerm.Bytes, DocsFlags.NONE);
				if (docs != null)
				{
					var docId = 0;
					while ((docId = docs.NextDoc()) != DocIdSetIterator.NO_MORE_DOCS)
					{
						var visitor = new DocumentStoredFieldVisitor(fields);
						reader.Document(docId, visitor);

						todo(visitor.Document);
					}
				}
			}
		}
	}
}