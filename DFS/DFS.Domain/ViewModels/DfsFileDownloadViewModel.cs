using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DFS.Domain.ViewModels
{
    public sealed class DfsFileDownloadViewModel
    {
        /// <summary>
        /// 文件下载地址
        /// </summary>
        [DisplayName("文件下载地址")]
        public string Download { get; set; }

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 下载服务器地址
        /// </summary>
        public string DownloadServer { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 上传完成时间戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 文件单位
        /// </summary>
        public string FileUnits { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 过期时间
        /// （单位：分钟）
        /// 默认：0不过期
        /// </summary>
        public string Expir { get; set; }
    }
}
