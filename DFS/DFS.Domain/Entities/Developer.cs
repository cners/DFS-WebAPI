using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DFS.Domain.Entities
{
    [Table("dfs_developer")]
    public sealed class Developer
    {
        [Key]
        public string DevID { get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }
        //public string DevType { get; set; }
        //public DateTime RegTime { get; set; }
        //public int State { get; set; }
        //public string AccessKey { get; set; }
        //public string SecretKey { get; set; }
    }
}
