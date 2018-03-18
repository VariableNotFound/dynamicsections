using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DynamicSections;
using Microsoft.AspNetCore.Mvc;
using DynamicSectionsDemo.Models;
using Microsoft.AspNetCore.Http;

namespace DynamicSectionsDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.RegisterBlock("", "controller", "\n    <!-- This code block is registered from the controller. See Controllers/Home.Index() -->");
            return View();
        }
    }
}
