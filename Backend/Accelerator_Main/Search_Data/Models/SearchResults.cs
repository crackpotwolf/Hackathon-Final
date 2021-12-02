using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Search_Data.Models 
{
	/// <summary>
	/// Результаты после выполнения поиска
	/// </summary>
	public class SearchResults
	{
		/// <summary>
		/// Запрос
		/// </summary>
		public string Query { get; set; }

		/// <summary>
		/// Кол-во совпадений
		/// </summary>
		public int TotalHits { get; set; }

		/// <summary>
		/// Совпадения
		/// </summary>
		public IList<TextHit> Hits { get; set; }

		public SearchResults() => Hits = new List<TextHit>();
	}
}