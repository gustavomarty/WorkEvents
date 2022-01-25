using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkEvents.BackgroundServices;
using WorkEvents.Models;

namespace WorkEvents.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Myhub()
        {
            return View();
        }

    }
}