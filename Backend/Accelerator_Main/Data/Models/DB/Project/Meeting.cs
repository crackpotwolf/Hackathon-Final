using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Project
{
    public class Meeting : ProjectData
    {
        public string Comment { get; set; }

        public string Date { get; set; }
    }
}
