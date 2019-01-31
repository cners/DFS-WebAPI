using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DFS.Domain.Entities
{
   public sealed class ApiCallLog
    {
        [Key]
        public string LogID { get; set; }
        public string DevID { get; set; }

        public ICollection<Developer> Developers { get; set; }
    }
}
