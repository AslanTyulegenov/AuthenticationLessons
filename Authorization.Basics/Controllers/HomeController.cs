using Microsoft.AspNetCore.Mvc;

namespace Authorization.Basics.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {

            return View();
        }
    }
}
