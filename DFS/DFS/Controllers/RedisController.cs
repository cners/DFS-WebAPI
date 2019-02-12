using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFS.CacheRedis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DFS.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private ICacheService _cacheService;
        public RedisController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet]
        [Route("SetValue")]
        public IActionResult Get(string key, string value)
        {
            _cacheService.Set(key, value, 600);
            return StatusCode(200, _cacheService.Get(key));
        }

        [HttpGet]
        [Route("GetValue")]
        public IActionResult Get(string key)
        {
            object cacheValue = _cacheService.Get(key);
            if (cacheValue != null)
                return StatusCode(200, cacheValue);
            else
                return StatusCode(404, "未找到对应的值");
        }
    }
}