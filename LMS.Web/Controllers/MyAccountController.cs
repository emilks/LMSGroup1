using LMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LMS.Web.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ILogger<MyAccountController> _logger;

        public MyAccountController(ILogger<MyAccountController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}