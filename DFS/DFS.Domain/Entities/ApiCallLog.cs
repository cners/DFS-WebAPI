using System;
using System.Collections.Generic;
using System.Text;

namespace DFS.Domain.Entities
{
   public sealed class ApiCallLog
    {
        public string LogID { get; set; }
        public string DevID { get; set; }

        public ICollection<Developer> Developers { get; set; }
    }
}
