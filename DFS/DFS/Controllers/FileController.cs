using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFS.API.Configurations;
using DFS.API.Filters;
using DFS.CacheRedis;
using DFS.Domain.Supervisor;
using DFS.Domain.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;


namespace DFS.API.Controllers
{
    /// <summary>
    /// 文件控制器
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IDfsSupervisor _dfsSupervisor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<AppSettings> _settings;
        private ICacheService _cacheService;

        public FileController(IDfsSupervisor dfsSupervisor,
            IHostingEnvironment hostingEnvironment,
            IOptions<AppSettings> settings,
            ICacheService cacheService)
        {
            _dfsSupervisor = dfsSupervisor;
            _hostingEnvironment = hostingEnvironment;
            _settings = settings;
            _cacheService = cacheService;
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = "v2")]
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
        [HttpPost(), Produces(typeof(DfsFileDownloadViewModel)), MapToApiVersion("1.0"), Route("Upload")]
        //[RequestSizeLimit(300_000_000)]
        //[RequestFormSizeLimitAttribute(100_000_000)]
        public async Task<IActionResult> Post(IFormFile file, CancellationToken ct = default)
        {
            try
            {
                //HashSet<object> result = new HashSet<object>();
                //var files = Request.Form.Files;
                //if (files.Count == 0)
                //    return BadRequest("请指定上传一个文件");
                if (file == null)
                    return BadRequest();

                var upload = new DfsFileUploadViewModel();
                long fileSize = file.Length;

                // 文件上传大小限制（非服务器的文件大小限制）
                long kb = 1024;
                long M = kb * 1024;
                long G = M * 1024;
                if (fileSize <= 0)
                    return BadRequest("该文件已损坏，不支持上传");
                else if (fileSize > 100 * M)
                    return BadRequest("超出文件最大限制");

                upload.Buffer = new byte[fileSize];
                var stream = file.OpenReadStream();
                await stream.ReadAsync(upload.Buffer, 0, upload.Buffer.Length);

                upload.Suffix = Path.GetExtension(file.FileName).TrimStart('.');
                upload.Suffix = (upload.Suffix.Length == 0) ? "unknown" : upload.Suffix;
                upload.FileSize = fileSize;
                upload.UploadServers = _settings.Value.FastDFS.GetUploadServers();
                upload.DownloadServer = _settings.Value.FastDFS.DownloadServer ?? "http://192.168.230.144/group1/";
                //result.Add();
                return new ObjectResult(await _dfsSupervisor.Upload(upload, ct));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return Content($"{ex.Message},{ex.StackTrace}");
            }
        }

        [HttpPost, MapToApiVersion("1.0"), Route("UploadAppend")]
        public async Task<IActionResult> UploadAppend(IFormFile file, CancellationToken ct = default)
        {
            try
            {
                if (file == null)
                    return BadRequest();

                var upload = new DfsFileUploadViewModel();
                long fileSize = file.Length;

                // 文件上传大小限制（非服务器的文件大小限制）
                long kb = 1024;
                long M = kb * 1024;
                long G = M * 1024;
                if (fileSize <= 0)
                    return BadRequest("该文件已损坏，不支持上传");
                else if (fileSize > 1 * G)
                    return BadRequest("超出文件最大限制");

                upload.Buffer = new byte[fileSize];
                var stream = file.OpenReadStream();
                await stream.ReadAsync(upload.Buffer, 0, upload.Buffer.Length);

                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:Buffer Size:{upload.Buffer.LongLength}");
                upload.Suffix = Path.GetExtension(Request.Form["name"]).TrimStart('.');
                upload.Suffix = (upload.Suffix.Length == 0) ? "unknown" : upload.Suffix;

                string md5File = Request.Form["md5"];
                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:file md5:{md5File}");

                upload.FileSize = fileSize;
                upload.UploadServers = _settings.Value.FastDFS.GetUploadServers();
                upload.DownloadServer = _settings.Value.FastDFS.DownloadServer ?? "http://192.168.230.144/group1/";


                DfsFileDownloadViewModel result = null;
                if (Request.Form["chunk"] == "0")//第一个使用 upload_appender_file()上传
                {
                    result = await _dfsSupervisor.UploadAppenderFileAsync(upload, ct);
                    _cacheService.Set<DfsFileDownloadViewModel>(md5File, result, 60 * 5);//5分钟内，单chunk需要上传完成
                }
                else
                {//非第1个文件chunk，使用append_file()追加
                    if (_cacheService.HasKey(md5File))
                    {
                        result = _cacheService.Get<DfsFileDownloadViewModel>(md5File);
                        _cacheService.Set<DfsFileDownloadViewModel>(md5File, result, 60 * 5);

                        var appendViewModel = new DfsFileUploadAppendViewModel();
                        appendViewModel.UploadServers = upload.UploadServers;
                        appendViewModel.GroupName = "group1";
                        appendViewModel.FileName = result.FilePath;
                        appendViewModel.Buffer = upload.Buffer;
                        appendViewModel.Suffix = upload.Suffix;
                        appendViewModel.FileSize = upload.FileSize;
                        appendViewModel.DownloadServer = upload.DownloadServer;
                        bool appendRetsult=await _dfsSupervisor.AppendFileAsync(appendViewModel, ct);
                        if (appendRetsult)
                        {
                            return Ok("ok");
                        }
                        else
                        {
                            return BadRequest("fail");
                        }
                    }
                }

                if (_cacheService.HasKey(md5File))
                {
                    result = _cacheService.Get<DfsFileDownloadViewModel>(md5File);
                    return new ObjectResult(result);
                }
                else
                {
                    return Ok("chunk upload fail");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "" + ex.StackTrace);
                return BadRequest($"{ex.Message}");
            }
        }

        /// <summary>
        /// 断点续传，测试专用，看到横杠说明没问题。
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet,MapToApiVersion("1.0"),Route("UploadAppendTest")]
        public async Task<IActionResult> UploadAppendTest(CancellationToken ct = default)
        {
            var testBytes = Encoding.UTF8.GetBytes("我是前半部分数据\r\n");
            var upload = new DfsFileUploadViewModel();
            upload.Buffer = testBytes.Take(6).ToArray();

            upload.Suffix = "txt";
            upload.FileSize = testBytes.Take(6).ToArray().Length;
            upload.UploadServers = _settings.Value.FastDFS.GetUploadServers();
            upload.DownloadServer = _settings.Value.FastDFS.DownloadServer ?? "http://192.168.230.144/group1/";


            var filename = await _dfsSupervisor.UploadAppenderFileAsync(upload, ct);


            var appendViewModel = new DfsFileUploadAppendViewModel();
            appendViewModel.UploadServers = upload.UploadServers;
            appendViewModel.GroupName = "group1";
            appendViewModel.FileName = filename.FilePath;
            appendViewModel.Buffer = Encoding.UTF8.GetBytes("我是后半部分数据\r\n----------\r\n如果看到这里，说明断点续传功能没问题").ToArray();
            appendViewModel.Suffix = upload.Suffix;
            appendViewModel.FileSize = upload.FileSize;
            appendViewModel.DownloadServer = upload.DownloadServer;
            _dfsSupervisor.AppendFileAsync(appendViewModel, ct).Wait();
            
            Console.WriteLine("UploadAppendFile Success" + filename);
            return Content(filename.FilePath);
        }

    }
}