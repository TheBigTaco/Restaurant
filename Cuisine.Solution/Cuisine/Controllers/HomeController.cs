using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Cuisine.Models;

namespace Cuisine.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        return View();
      }
    }
}
