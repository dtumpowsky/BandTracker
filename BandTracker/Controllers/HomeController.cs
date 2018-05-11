using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("/success")]
        public ActionResult Success()
        {
            return View("Success");
        }
    }
}
