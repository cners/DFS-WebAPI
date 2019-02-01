using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DFS.Domain.ViewModels
{
    public class DfsFileUploadViewModel
    {
        /// <summary>
        /// 文件字节
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 上传服务器IP（Tracker）
        ///  new System.Net.IPEndPoint(IPAddress.Parse("192.168.230.144"),22122),
        /// </summary>
        public List<IPEndPoint> UploadServers { get; set; }

        /// <summary>
        /// 下载服务器（Nginx）
        /// </summary>
        public string DownloadServer { get; set; }
    }
}
