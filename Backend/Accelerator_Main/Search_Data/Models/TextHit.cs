using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Search_Data.Models 
{
	/// <summary>
	/// Результат
	/// </summary>
	public class TextHit : TextDocument
	{
		/// <summary>
		/// Счет
		/// </summary>
		public float Score { get; set; }
	}
}