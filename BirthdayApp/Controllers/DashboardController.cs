using Microsoft.AspNetCore.Mvc;

namespace BirthdayApp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}