using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFS.Domain.Converters;
using DFS.Domain.Entities;
using DFS.Domain.ViewModels;
using FastDFS.Client;

namespace DFS.Domain.Supervisor
{
    public partial class DfsSupervisor : IDfsSupervisor
    {

        public async Task<DfsFileViewModel> AddDfsFileAsync(DfsFileViewModel newDfsFileViewModel,
            CancellationToken ct = default(CancellationToken))
        {
            var model = new DfsFile
            {
                PathID = Guid.NewGuid().ToString("N"),
                DevID = newDfsFileViewModel.DevID,
                Filename = newDfsFileViewModel.Filename,
                FileType = newDfsFileViewModel.FileType,
                Path = newDfsFileViewModel.Path,
                StorageIP = newDfsFileViewModel.StorageIP,
                Suffix = newDfsFileViewModel.Suffix,
                UploadTime = newDfsFileViewModel.UploadTime,
                VisitIP = newDfsFileViewModel.VisitIP
            };

            model = await _fileRepository.AddAsync(model, ct);
            newDfsFileViewModel.PathID = model.Path;
            return newDfsFileViewModel;
        }


        public async Task<List<DfsFileViewModel>> GetAllDfsFileAsync(CancellationToken ct = default(CancellationToken))
        {
            var files = DfsFileConverter.ConvertList(await _fileRepository.GetAllAsync(ct));
            return files;
        }

        /// <summary>
        /// 上传一个文件（该方法不支持断点续传）
        /// </summary>
        /// <param name="uploadViewModel"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DfsFileDownloadViewModel> Upload(DfsFileUploadViewModel uploadViewModel,
            CancellationToken ct = default)
        {
            // 初始化 
            ConnectionManager.Initialize(uploadViewModel.UploadServers);

            // 获取存储服务器节点
            var storageNode = FastDFSClient.GetStorageNodeAsync("group1").Result;
            Console.WriteLine($"storage node : {storageNode.GroupName},{storageNode.EndPoint}");

            // 执行上传
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:uploading...");
            string filePath = FastDFSClient.UploadFileAsync(storageNode, uploadViewModel.Buffer, uploadViewModel.Suffix).Result;
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:Upload Success !");

            Console.WriteLine($"Visit : {filePath}");

            // 上传完成，返回结果
            var downloadViewModel = new DfsFileDownloadViewModel
            {
                DownloadServer = uploadViewModel.DownloadServer,
                FilePath = filePath,
                Suffix = uploadViewModel.Suffix,
                FileSize = uploadViewModel.FileSize,
                Download = uploadViewModel.DownloadServer + filePath,
                TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                FileUnits = "bytes",
                Guid = Guid.NewGuid().ToString("N"),
                Expir = "0"
            };

            return downloadViewModel;
        }

        /// <summary>
        /// 单个文件上传（支持断点续传）
        /// </summary>
        /// <param name="uploadViewModel"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DfsFileDownloadViewModel> UploadAppenderFileAsync(DfsFileUploadViewModel uploadViewModel,
         CancellationToken ct = default)
        {
            // 初始化 
            ConnectionManager.Initialize(uploadViewModel.UploadServers);

            // 获取存储服务器节点
            var storageNode = FastDFSClient.GetStorageNodeAsync("group1").Result;
            Console.WriteLine($"storage node : {storageNode.GroupName},{storageNode.EndPoint}");

            // 执行上传
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:UploadAppendFile First...");
            string filePath = FastDFSClient.UploadAppenderFileAsync(storageNode, uploadViewModel.Buffer, uploadViewModel.Suffix).Result;
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:UploadAppendFile First Success !");

            Console.WriteLine($"Visit Url: {filePath}");

            // 上传完成，返回结果
            var downloadViewModel = new DfsFileDownloadViewModel
            {
                DownloadServer = uploadViewModel.DownloadServer,
                FilePath = filePath,
                Suffix = uploadViewModel.Suffix,
                FileSize = uploadViewModel.FileSize,
                Download = uploadViewModel.DownloadServer + filePath,
                TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                FileUnits = "bytes",
                Guid = Guid.NewGuid().ToString("N"),
                Expir = "0"
            };

            return downloadViewModel;
        }

        /// <summary>
        /// 单个文件上传（断点续传的追加文件）
        /// </summary>
        /// <param name="uploadViewModel"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> AppendFileAsync(DfsFileUploadAppendViewModel uploadViewModel,
         CancellationToken ct = default)
        {
            // 初始化 
            //ConnectionManager.Initialize(uploadViewModel.UploadServers);

            // 获取存储服务器节点
            StorageNode storageNode = await FastDFSClient.GetStorageNodeAsync("group1");
            Console.WriteLine($"append storage node : {storageNode.GroupName},{storageNode.EndPoint}");
            FDFSFileInfo fileInfo = await FastDFSClient.GetFileInfoAsync(storageNode, uploadViewModel.FileName);
            if (fileInfo == null)
            {
                Console.WriteLine($"GetFileInfoAsync Fail, path:{uploadViewModel.FileName}");
                return false;
            }
            //Console.WriteLine("FileName:{0}", uploadViewModel.FileName);
            //Console.WriteLine("FileSize:{0}", fileInfo.FileSize);
            //Console.WriteLine("CreateTime:{0}", fileInfo.CreateTime);
            //Console.WriteLine("Crc32:{0}", fileInfo.Crc32);


            // 执行上传
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:UploadAppending...");
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:GropName:{uploadViewModel.GroupName}");//group1
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:FileName:{uploadViewModel.FileName}");//M00/00/00/wKjmhFx2NL6EXFwjAAAAAA79E7k517.zip
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:Buffer:{uploadViewModel.Buffer.LongLength}");//432644

            await FastDFSClient.AppendFileAsync(uploadViewModel.GroupName, uploadViewModel.FileName, uploadViewModel.Buffer);

            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:UploadAppendFile Success !");

            return true;
        }
    }
}
