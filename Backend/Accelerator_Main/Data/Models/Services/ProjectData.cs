using Data.Models.DB.Project;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Services
{
    /// <summary>
    /// Единая полная форма заявки
    /// </summary>
    public class ProjectData : Project
    {
        [JsonIgnore]
        public override Guid Guid { get => base.Guid; set => base.Guid = value; }

        [JsonIgnore]
        public override DateTime DateCreate { get => base.DateCreate; set => base.DateCreate = value; }

        [JsonIgnore]
        public override DateTime DateUpdate { get => base.DateUpdate; set => base.DateUpdate = value; }
    }
}
