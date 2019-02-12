using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFS.API.Configurations
{
    public class AppSettings
    {
        /// <summary>
        /// FastDFS 配置文件
        /// </summary>
        public AppSettingsFastDFS FastDFS { get; set; }

        public AppSettingsRedisConfig Redis { get; set; }
    }

    public class AppSettingsFastDFS
    {
        /// <summary>
        /// 下载服务器地址
        /// </summary>
        public string DownloadServer { get; set; }

        /// <summary>
        /// 上传服务器
        /// 地址:端口
        /// </summary>
        public string[] UploadServers { get; set; }


        /// <summary>
        /// 获取上传服务器地址组
        /// </summary>
        /// <returns></returns>
        public List<IPEndPoint> GetUploadServers()
        {
            var tackers = new List<IPEndPoint>();
            IPAddress ipAddress;
            int port;

            foreach (string server in this.UploadServers)
            {
                if (server.Trim().Length < 16) continue;
                if (server.Contains(':'))// 使用了xxx.xxx.xxx.xxx:port格式
                {
                    var ip_port = server.Split(':');
                    if (ip_port.Length != 2) continue;

                    if (IPAddress.TryParse(ip_port[0].ToString(), out ipAddress) &&
                        int.TryParse(ip_port[1].ToString(), out port))
                    {
                        var tracker = new IPEndPoint(ipAddress, port);
                        tackers.Add(tracker);
                    }
                }
                else// 使用了xxx.xxx.xxx.xxx格式，则默认22122端口
                {
                    if (IPAddress.TryParse(server, out ipAddress))
                    {
                        var tracker = new IPEndPoint(ipAddress, 22122);
                        tackers.Add(tracker);
                    }
                }
            }
            return tackers;
        }

    }

    public class AppSettingsRedisConfig
    {

        public AppSettingsRedis Master { get; set; }
    }

    public class AppSettingsRedis
    {
        public string Connection { get; set; }

        public string Password { get; set; }

        public string InstanceName { get; set; }
    }
}
