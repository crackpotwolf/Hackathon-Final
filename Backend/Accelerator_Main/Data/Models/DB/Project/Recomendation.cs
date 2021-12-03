using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Project
{
    /// <summary>
    /// Рекомендация тегов
    /// </summary>
    public class Recomendation : ProjectData
    {
        public string RecomendedTag { get; set; }
    }
}
