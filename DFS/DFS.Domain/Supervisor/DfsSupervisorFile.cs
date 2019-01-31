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

        public async Task<string> Upload(DfsFileUploadViewModel uploadViewModel,
            CancellationToken ct = default)
        {
            ConnectionManager.Initialize(new List<System.Net.IPEndPoint>()
            {
                new System.Net.IPEndPoint(IPAddress.Parse("192.168.230.132"),2122)
            });

            var storageNode = FastDFSClient.GetStorageNodeAsync("group1").Result;
            Console.WriteLine($"storage node : \r\n{storageNode.GroupName}\r\n{storageNode.EndPoint}\r\n\r\n");

            //var buffer = File.ReadAllBytes(uploadViewModel.FilePath);
           
            Console.WriteLine("uploading...");
            string FilePath = FastDFSClient.UploadFileAsync(storageNode, uploadViewModel.Buffer, "jpg").Result;

            Console.WriteLine($"\r\nUpload Success !");

            Console.WriteLine($"Visit : {uploadViewModel.FilePath}");

            return FilePath;
        }
    }
}
