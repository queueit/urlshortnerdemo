using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortnerDemo.Controllers
{
    public class HealthcheckController : Controller
    {
        public IActionResult Index()
        {
            return Json(new { result = "ok" });
        }
    }
}
