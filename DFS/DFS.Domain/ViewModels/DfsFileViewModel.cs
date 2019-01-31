using System;
using System.Collections.Generic;
using System.Text;

namespace DFS.Domain.ViewModels
{
    public class DfsFileViewModel
    {

        public string PathID { get; set; }

        public string StorageIP { get; set; }

        public string VisitIP { get; set; }

        public string Path { get; set; }

        public string Suffix { get; set; }

        public string Filename { get; set; }

        public string FileType { get; set; }

        public DateTime? UploadTime { get; set; }

        public string DevID { get; set; }
    }
}
