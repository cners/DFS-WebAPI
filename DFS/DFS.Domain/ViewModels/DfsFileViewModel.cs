using System;
using System.Collections.Generic;
using System.Text;

namespace DFS.Domain.ViewModels
{
    public class DfsFileViewModel
    {
        /// <summary>
        /// 路径主键
        /// </summary>
        public string PathID { get; set; }

        /// <summary>
        /// 存储服务器地址
        /// </summary>
        public string StorageIP { get; set; }

        /// <summary>
        /// 访问服务器地址
        /// </summary>
        public string VisitIP { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime? UploadTime { get; set; }

        /// <summary>
        /// 开发主键
        /// </summary>
        public string DevID { get; set; }
    }
}
