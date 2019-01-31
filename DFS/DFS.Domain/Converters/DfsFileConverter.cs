using DFS.Domain.Entities;
using DFS.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFS.Domain.Converters
{
    public static class DfsFileConverter
    {
        public static DfsFileViewModel Convert(DfsFile dfsFile)
        {
            var dfsFileViewModel = new DfsFileViewModel();
            /// TODO:需要补充
            return dfsFileViewModel;
        }

        public static List<DfsFileViewModel> ConvertList(IEnumerable<DfsFile> dfsFiles)
        {
            return dfsFiles.Select(e =>
            {
                var model = new DfsFileViewModel();
                model.DevID = e.DevID;
                model.Filename = e.Filename;
                model.FileType = e.FileType;
                model.Path = e.Path;
                model.PathID = e.PathID;
                model.StorageIP = e.StorageIP;
                model.Suffix = e.Suffix;
                model.UploadTime = e.UploadTime;
                model.VisitIP = e.VisitIP;
                return model;
            }).ToList();
        }
    }
}
