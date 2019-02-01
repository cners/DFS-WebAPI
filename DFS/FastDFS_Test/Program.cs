using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using FastDFS;
using FastDFS.Client;

namespace FastDFS_Test
{
    class Program
    {
    static void Main(string[] args)
    {
        string imgFile = @"3.jpg";

        ConnectionManager.Initialize(new System.Collections.Generic.List<System.Net.IPEndPoint>() {
            new System.Net.IPEndPoint(IPAddress.Parse("192.168.230.144"),22122)
        });
        var storageNode = FastDFSClient.GetStorageNodeAsync("group1").Result;
        Console.WriteLine($"storage node : \r\n{storageNode.GroupName}\r\n{storageNode.EndPoint}\r\n\r\n");

        var buffer = File.ReadAllBytes(imgFile);

        Stopwatch swatch = new Stopwatch();
        swatch.Start();
        Console.WriteLine("Uploading...");
        string filename = FastDFSClient.UploadFileAsync(storageNode, buffer, "jpg").Result;
        swatch.Stop();

        Console.WriteLine($"\r\nUpload Success !\r\nElapsed:{swatch.ElapsedMilliseconds} ms");

        Console.WriteLine($"Visit : {filename}");
        Console.ReadKey();
    }


    }
 
}
