using Data.Models.DB._BaseEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB.Project
{
    public class ProjectData : BaseEntity
    {
        [ForeignKey("Project")]
        [JsonIgnore]
        public Guid ProjectGuid { get; set; }
        
        [JsonIgnore]
        public Project Project { get; set; }

        [JsonIgnore]
        public override DateTime DateCreate { get => base.DateCreate; set => base.DateCreate = value; }

        [JsonIgnore]
        public override DateTime DateUpdate { get => base.DateUpdate; set => base.DateUpdate = value; }

        [JsonIgnore]
        public override Guid Guid { get => base.Guid; set => base.Guid = value; }
    }
}
