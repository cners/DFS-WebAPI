using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DFS.API.Configurations;
using DFS.Domain.Supervisor;
using DFS.Domain.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFS.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IDfsSupervisor _dfsSupervisor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<AppSettings> _settings;

        public FileController(IDfsSupervisor dfsSupervisor,
            IHostingEnvironment hostingEnvironment,
            IOptions<AppSettings> settings)
        {
            _dfsSupervisor = dfsSupervisor;
            _hostingEnvironment = hostingEnvironment;
            _settings = settings;
        }

        [HttpPost]
        [Produces(typeof(DfsFileViewModel))]
        public async Task<IActionResult> Post([FromBody] DfsFileViewModel input,
            CancellationToken ct = default)
        {
            try
            {
                if (input == null)
                    return BadRequest();

                return StatusCode(201, await _dfsSupervisor.AddDfsFileAsync(input, ct));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpGet]
        [Produces(typeof(List<DfsFileViewModel>))]
        public async Task<IActionResult> Get(CancellationToken ct = default)
        {
            try
            {
                return new ObjectResult(await _dfsSupervisor.GetAllDfsFileAsync(ct));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        /// <summary>
        /// 单文件上传
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        //[MapToApiVersion("1.1")]
        [Produces(typeof(DfsFileDownloadViewModel))]
        [MapToApiVersion("1.0")]
        [Route("Upload")]
        //[RequestSizeLimit(10_000_000)] // 最大100M左右
        //[DisableRequestSizeLimit]     //取消最大文件限制
        public async Task<IActionResult> Post(IFormFile file, CancellationToken ct = default)
        {
            try
            {
                if (file == null)
                    return BadRequest();
                //files = Request.Form.Files;
                //long size = files.Sum(f => f.Length);

                var upload = new DfsFileUploadViewModel();
                long fileSize = file.Length;

                // 文件上传大小限制（非服务器的文件大小限制）
                long kb = 1024;
                long M = kb * 1024;
                long G = M * 1024;
                if (fileSize <= 0)
                    return BadRequest("该文件已损坏，不支持上传");
                else if (fileSize > 10 * M)
                    return BadRequest("超出文件最大限制");

                upload.Buffer = new byte[fileSize];
                var stream = file.OpenReadStream();
                await stream.ReadAsync(upload.Buffer, 0, upload.Buffer.Length);

                upload.Suffix = Path.GetExtension(file.FileName).TrimStart('.');
                upload.Suffix = (upload.Suffix.Length == 0) ? "unknown" : upload.Suffix;
                upload.FileSize = fileSize;
                upload.UploadServers = _settings.Value.FastDFS.GetUploadServers();
                upload.DownloadServer = _settings.Value.FastDFS.DownloadServer ?? "http://192.168.230.144/group1/";

                return new ObjectResult(await _dfsSupervisor.Upload(upload, ct));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}