using System.Diagnostics;
using Leave_Mangement_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Leave_Mangement_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
           
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("HR"))
                {
                    return RedirectToAction("Index", "Employee");
                }
                else if (User.IsInRole("Manager"))
                {
                    return RedirectToAction("Index", "LeaveRequest");
                }
                else if (User.IsInRole("Employee"))
                {
                    return RedirectToAction("Index", "LeaveRequest");
                }
            }

           
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
