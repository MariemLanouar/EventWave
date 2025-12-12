using Microsoft.AspNetCore.Mvc;

namespace EventWave.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
