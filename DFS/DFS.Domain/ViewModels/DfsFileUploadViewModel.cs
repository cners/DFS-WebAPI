using System;
using System.Collections.Generic;
using System.Text;

namespace DFS.Domain.ViewModels
{
    public class DfsFileUploadViewModel
    {
        public string FilePath { get; set; }

        public byte[] Buffer { get; set; }
    }
}
