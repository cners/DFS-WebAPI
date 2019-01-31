using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DFS.Domain.Supervisor;
using DFS.Domain.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DFS.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IDfsSupervisor _dfsSupervisor;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileController(IDfsSupervisor dfsSupervisor,
            IHostingEnvironment hostingEnvironment)
        {
            _dfsSupervisor = dfsSupervisor;
            _hostingEnvironment = hostingEnvironment;
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

        [HttpPost]
        [MapToApiVersion("1.1")]
        //[Produces(typeof(DfsFileUploadViewModel))]
        public async Task<IActionResult> Upload( CancellationToken ct = default)
        {
            try
            {
                var files = Request.Form.Files;
                //long size = files.Sum(f => f.Length);
                //string webRootPath = _hostingEnvironment.WebRootPath;
                //string contentRootPath = _hostingEnvironment.ContentRootPath;
                foreach (var item in files)
                {
                    var input = new DfsFileUploadViewModel();
                    long fileSize = item.Length;
                    byte[] buffer = new byte[1024 * 100];
                    var stream = new MemoryStream(buffer);
                    await item.CopyToAsync(stream, ct);
                    input.Buffer = buffer;
                    return new ObjectResult(await _dfsSupervisor.Upload(input, ct));
                }


                return BadRequest();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}