using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DFS.Domain.Supervisor;
using DFS.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DFS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperController : ControllerBase
    {
        private readonly IDfsSupervisor _dfsSupervisor;

        public DeveloperController(IDfsSupervisor dfsSupervisor)
        {
            _dfsSupervisor = dfsSupervisor;
        }

        [HttpPost]
        [Produces(typeof(DeveloperViewModel))]
        public async Task<IActionResult> Post([FromBody]DeveloperViewModel input,
            CancellationToken ct = default)
        {
            try
            {
                if (input == null)
                    return new BadRequestObjectResult("无效参数");

                return StatusCode(201, await _dfsSupervisor.AddDeveloperAsync(input, ct));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Produces(typeof(List<DeveloperViewModel>))]
        public async Task<IActionResult> Get(CancellationToken ct = default)
        {
            try
            {
                return new ObjectResult(await _dfsSupervisor.GetAllDeveloperAsync(ct));
            }
            catch ( Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}