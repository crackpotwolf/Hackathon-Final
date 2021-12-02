using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Search_Data.Extensions;
using Search_Data.Models;

namespace Search_Data.Search
{
	/// <summary>
	/// Поиск
	/// </summary>
	public class WordSearch
	{
		private const LuceneVersion MATCH_LUCENE_VERSION = LuceneVersion.LUCENE_48;

		private readonly SearcherManager _searchManager;
		private readonly QueryParser _queryParser;
		private readonly IndexWriter _writer;
		private readonly Analyzer _analyzer;
		private const string keyField = "Guid";

		public WordSearch(string indexPath)
		{
			//_analyzer = new EnhEnglishAnalyzer(MATCH_LUCENE_VERSION);

			_analyzer = new MultiFieldAnalyzerWrapper(
				defaultAnalyzer: new EnhEnglishAnalyzer(MATCH_LUCENE_VERSION, true),
				new[]
				{
					(
						new[] { "" },
						Analyzer.NewAnonymous(createComponents: (fieldName, reader) =>
						{
							var source = new KeywordTokenizer(reader);
							TokenStream result = new ASCIIFoldingFilter(source);
							result = new LowerCaseFilter(MATCH_LUCENE_VERSION, result);
							return new TokenStreamComponents(source, result);
						})
					)
				});

			//_writer = new IndexWriter(indexPath, new IndexWriterConfig(MATCH_LUCENE_VERSION, _analyzer));
			_writer = new IndexWriter(FSDirectory.Open(indexPath), new IndexWriterConfig(MATCH_LUCENE_VERSION, _analyzer));

			_searchManager = new SearcherManager(_writer, true, null);

			// создаем псевдоним для полей: "t" -> "Text"; 
			_queryParser = new AliasMultiFieldQueryParser(MATCH_LUCENE_VERSION, new[] { "Text", "Name" }, _analyzer,
				new Dictionary<string, string>()
				{
					{ "t", "Text" }
				});
		}

		/// <summary>
		/// Поиск по списку параметров
		/// </summary>
		/// <param name="indexPath"></param>
		public WordSearch(string indexPath, string[] fields)
		{
			//_analyzer = new EnhEnglishAnalyzer(MATCH_LUCENE_VERSION);

			_analyzer = new MultiFieldAnalyzerWrapper(
				defaultAnalyzer: new EnhEnglishAnalyzer(MATCH_LUCENE_VERSION, true),
				new[]
				{
					(
						new[] { "" },
						Analyzer.NewAnonymous(createComponents: (fieldName, reader) =>
						{
							var source = new KeywordTokenizer(reader);
							TokenStream result = new ASCIIFoldingFilter(source);
							result = new LowerCaseFilter(MATCH_LUCENE_VERSION, result);
							return new TokenStreamComponents(source, result);
						})
					)
				});

			//_writer = new IndexWriter(indexPath, new IndexWriterConfig(MATCH_LUCENE_VERSION, _analyzer));
			_writer = new IndexWriter(FSDirectory.Open(indexPath), new IndexWriterConfig(MATCH_LUCENE_VERSION, _analyzer));

			_searchManager = new SearcherManager(_writer, true, null);

			// создаем псевдоним для полей: "t" -> "Text"; 
			_queryParser = new AliasMultiFieldQueryParser(MATCH_LUCENE_VERSION, fields, _analyzer,
				new Dictionary<string, string>()
				{
					{ "t", "Text" }
				});
		}

		/// <summary>
		/// Создаем индексы
		/// </summary>
		public void Index(List<TextDocument> textDocument)
		{
			foreach (var text in textDocument)
			{
				// Подготовить новый документ
				var doc = new Document
				{
					new StringField(keyField, $"{text.Guid}", Field.Store.YES),
					new TextField("Text", text.Text, Field.Store.YES),
				};

				// Обновление документа
				_writer.UpdateDocument(new Term(keyField, doc.GetField(keyField).GetStringValue()), doc);

				// Отладка токенов
				//PrintTokens(_writer.Analyzer, "Name", text.Text);
			}

			// Удаление документа
			//_writer.DeleteDocuments(new Term(keyField, "3"));

			_writer.Flush(true, true);
			_writer.Commit();
			_writer.Dispose();
		}

		/// <summary>
		/// Добавление документа
		/// </summary>
		/// <param name="document">Документ</param>
		public void AddIndex(TextDocument document)  
		{
			// Подготовить новый документ
			var doc = new Document
			{
				new StringField(keyField, $"{document.Guid}", Field.Store.YES),
				new TextField("Text", document.Text, Field.Store.YES),
			};

			// Добавление документа
			_writer.AddDocument(doc);

			_writer.ForceMerge(_writer.NumDocs);
		}

		/// <summary>
		/// Добавление документа (Lucene)
		/// </summary>
		/// <param name="document">Документ</param>
		public void AddIndex(Document document)
		{
			// Добавление документа
			_writer.AddDocument(document);

			_writer.ForceMerge(_writer.NumDocs);
		}

		/// <summary>
		/// Добавление документов
		/// </summary>
		/// <param name="documents">Документы</param>
		public void AddIndexes(List<TextDocument> documents)
		{
			List<Document> docs = new List<Document>();

			foreach (var text in documents)
			{
				// Подготовить новый документ
				var doc = new Document
				{
					new StringField(keyField, $"{text.Guid}", Field.Store.YES),
					new TextField("Text", text.Text, Field.Store.YES),
				};

				docs.Add(doc);
			}

			// Добавление документов
			_writer.AddDocuments(docs);
		}

		/// <summary>
		/// Удаление документа
		/// </summary>
		/// <param name="guidFile">Документ</param>
		public void DeleteIndex(Guid guidFile) 
		{
			// Удаление документа
			_writer.DeleteDocuments(new Term(keyField, guidFile.ToString()));
		}

		/// <summary>
		/// Удаление всех документов
		/// </summary>
		public void DeleteAllIndexes() 
		{
			// Удаление
			_writer.DeleteAll();
		}

		/// <summary>
		/// Обновление документа
		/// </summary>
		/// <param name="document">Документ</param>
		public void UpdateIndex(TextDocument document)
		{
			// Подготовить новый документ
			var doc = new Document
			{
				new StringField(keyField, $"{document.Guid}", Field.Store.YES),
				new TextField("Text", document.Text, Field.Store.YES),
			};

			// Обновление документа
			_writer.UpdateDocument(new Term(keyField, doc.GetField(keyField).GetStringValue()), doc);
		}

		/// <summary>
		/// Фиксирует изменения
		/// </summary>
		public void CommitChanges()
		{
			// Удалить не используемые файлы
			_writer.DeleteUnusedFiles();
			// Сбрасывает все буферизованные обновления в памяти (добавление и удаление) в каталог
			_writer.Flush(true, true);
			// Фиксирует все ожидающие изменения
			_writer.Commit();
			// Закрывает все связанные файлы
			_writer.Dispose();
		}

		/// <summary>
		/// Фиксирует изменения
		/// </summary>
		public void DiscardChanges()
		{
			// Закрывает все связанные файлы
			_writer.Dispose();
		}

		/// <summary>
		/// Поиск по индексам
		/// </summary>
		public SearchResults Search(string query)
		{
			_searchManager.MaybeRefreshBlocking();

			var searcher = _searchManager.Acquire();
			try
			{
				if (searcher.IndexReader.MaxDoc > 0)
				{
					var q = _queryParser.Parse(query);

					var topDocs = searcher.Search(q, searcher.IndexReader.MaxDoc);

					var result = new SearchResults()
					{
						Query = query,
						TotalHits = topDocs.TotalHits,
					};

					//float min = topDocs.ScoreDocs.Select(p => p.Score).Min();
					//float max = topDocs.ScoreDocs.Select(p => p.Score).Max();

					foreach (var scoreDoc in topDocs.ScoreDocs)
					{
						var document = searcher.Doc(scoreDoc.Doc);

						var hit = new TextHit
						{
							Guid = Guid.Parse(document.GetField("Guid")?.GetStringValue()),
							Text = document.GetField("Text")?.GetStringValue(),

							// Результаты автоматически сортируются по релевантности
							//Score = (scoreDoc.Score - min) / (max - min) * 100
							Score = scoreDoc.Score * 10
						};

						result.Hits.Add(hit);
					}

					return result;
				}
				else
					return new SearchResults();
			}
			finally
			{
				_searchManager.Release(searcher);
				searcher = null;
				_writer.Dispose();
			}
		}

		/// <summary>
		/// Чтение индексного документа
		/// </summary>
		public void WorkOnIndex()
		{
			// I. по идентификатору документа
			var reader = _writer.GetReader(true);

			for (var x = 0; x < reader.NumDocs; x++)
			{
				var doc = reader.Document(x);
				// ...
			}

			// II. использовать термин и выбранные поля
			_writer.ForEachTermDocs(new Term("year", "1194"), new[] { "title" }, d =>
			{
				var title = d.GetField("title").GetStringValue();
				// ...
			});
		}

		/// <summary>
		/// Печатать токенов
		/// </summary>
		protected void PrintTokens(Analyzer analyzer, string fieldName, string text)
		{
			if (analyzer == null)
				throw new ArgumentNullException(nameof(analyzer));

			var tokenStream = analyzer.GetTokenStream(fieldName, text);
			var termAttr = tokenStream.GetAttribute<ICharTermAttribute>();

			tokenStream.Reset();

			var str = new StringBuilder();
			while (tokenStream.IncrementToken())
			{
				if (str.Length > 0)
				{
					str.Append(" ");
				}
				str.Append(termAttr.ToString());
			}
			Console.WriteLine($"field: {fieldName} '{text}' -> '{str.ToString()}'");

			tokenStream.Reset();
		}
	}
}