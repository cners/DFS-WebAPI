using DFS.Domain.Supervisor;
using DFS.Domain.ViewModels;
using DFS.Domain.ViewModels.Developer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DFS.API.Controllers
{

    /// <summary>
    /// 开发者控制器
    /// </summary>
    [ApiVersion("1.0")]
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
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [Route("Auth")]
        public async Task<IActionResult> Post([FromBody]DeveloperAuthViewModel input,
            CancellationToken ct = default)
        {
            try
            {
                if (input == null)
                    return BadRequest();
                var user = new
                {
                    AppID = "appid",
                    SecretKey = "secretkey"
                };
                if (input.AppID == user.AppID && input.SecretKey == user.SecretKey)
                {
                    var identity =new ClaimsIdentity();

                }
                else
                {
                    return StatusCode(401, "登录失败，用户授权失败");
                }

                return StatusCode(201, "");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}