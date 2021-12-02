using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.En;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.NGram;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Util;

namespace Search_Data.Search
{
	/// <summary>
	/// Анализатор
	/// </summary>
	public class EnhEnglishAnalyzer : StopwordAnalyzerBase
	{
		private readonly bool _userNGram;
		private readonly int _ngramMin;
		private readonly int _ngramMax;

		#region Constructor

		/// <summary>
		/// Анализатор MTG
		/// </summary>
		/// <param name="useNGram">n-gram</param>
		/// <param name="ngramMin">Минимальный размер n-gram</param>
		/// <param name="ngramMax">Максимальный размер n-gram</param>
		public EnhEnglishAnalyzer(LuceneVersion matchVersion, bool useNGram = true, int ngramMin = 2, int ngramMax = 10) : base(matchVersion)
		{
			_userNGram = useNGram;
			_ngramMin = ngramMin;
			_ngramMax = ngramMax;
		}

		#endregion

		protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
		{
			Tokenizer source = new StandardTokenizer(m_matchVersion, reader);
			TokenStream result = new StandardFilter(m_matchVersion, source);

			result = new EnglishPossessiveFilter(m_matchVersion, result);

			// Преобразует é в e (и © в (c), и т. д.
			result = new ASCIIFoldingFilter(result);
			result = new LowerCaseFilter(m_matchVersion, result);
			result = new StopFilter(m_matchVersion, result, EnglishAnalyzer.DefaultStopSet);

			// Для отсечения суффиксов слов
			result = new PorterStemFilter(result);

			// Токенизатор ngram сначала разбивает текст на слова всякий раз, когда он встречает один из списка указанных символов,
			// затем он выдает N-грамм каждого слова указанной длины.
			if (_userNGram)
			{
				result = new EdgeNGramTokenFilter(m_matchVersion, result, _ngramMin, _ngramMax);
			}

			return new TokenStreamComponents(source, result);
		}
	}
}