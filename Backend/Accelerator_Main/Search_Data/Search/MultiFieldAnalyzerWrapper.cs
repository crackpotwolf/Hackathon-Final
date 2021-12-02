using System.Collections.Generic;
using Lucene.Net.Analysis;

namespace Search_Data.Search
{
	/// <summary>
	/// Этот анализатор используется для облегчения сценариев, в которых разные
	/// поля требуют различных методов анализа. 
	/// </summary>
	public sealed class MultiFieldAnalyzerWrapper : AnalyzerWrapper
	{
		private readonly Analyzer _defaultAnalyzer;
		private readonly IDictionary<string, Analyzer> _fieldAnalyzers;

		public MultiFieldAnalyzerWrapper(Analyzer defaultAnalyzer)
			: this(defaultAnalyzer, null)
		{
		}

		public MultiFieldAnalyzerWrapper(Analyzer defaultAnalyzer, (string[] keys, Analyzer analyzer)[] analyzers)
			: base(PER_FIELD_REUSE_STRATEGY)
		{
			_defaultAnalyzer = defaultAnalyzer;
			_fieldAnalyzers = new Dictionary<string, Analyzer>();

			foreach (var (keys, analyzer) in analyzers)
			{
				foreach (var key in keys)
				{
					_fieldAnalyzers.Add(key, analyzer);
				}
			}
		}

		protected override Analyzer GetWrappedAnalyzer(string fieldName)
		{
			var analyzer = _fieldAnalyzers.ContainsKey(fieldName) ? _fieldAnalyzers[fieldName] : null;
			return analyzer ?? _defaultAnalyzer;
		}
	}
}