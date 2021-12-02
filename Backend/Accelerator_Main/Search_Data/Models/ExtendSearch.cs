using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search_Data.Models
{
    /// <summary>
    /// Для передачи данных расширенного поиска
    /// </summary>
    public class ExtendSearch
    {
        public string Team { get; set; }

        public string Project { get; set; }

        public string Description { get; set; }
    }
}
