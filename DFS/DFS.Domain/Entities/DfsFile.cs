using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DFS.Domain.Entities
{
    [Table("dfs_file")]
    public sealed class DfsFile
    {
        [Key]
        [Column("path_id")]
        public string PathID { get; set; }

        [Column("storage_ip")]
        public string StorageIP { get; set; }

        [Column("visit_ip")]
        public string VisitIP { get; set; }

        public string Path { get; set; }

        public string Suffix { get; set; }

        public string Filename { get; set; }

        public string FileType { get; set; }

        [Column("upload_time")]
        public DateTime? UploadTime { get; set; }

        [Column("dev_id")]
        public string DevID { get; set; }
    }
}
