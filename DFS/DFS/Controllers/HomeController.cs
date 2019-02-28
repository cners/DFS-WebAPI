using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DFS.API.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            try
            {
                var uri = new Uri("/swagger", UriKind.Relative);
                return Redirect(uri.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        public IActionResult Upload()
        {
            return View();
        }
    }
}