using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    [Route("Def")]
    public class DefaultController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}