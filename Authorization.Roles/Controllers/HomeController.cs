using Microsoft.AspNetCore.Mvc;

namespace Authorization.Roles.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {
            ViewBag.Name = User.Identity.Name;
            ViewBag.IsAuthenticated  = User.Identity.IsAuthenticated;

            return View();
        }
        
        public async Task<IActionResult> AccessDenied()
        {
            ViewBag.Name = User.Identity.Name;
            ViewBag.IsAuthenticated  = User.Identity.IsAuthenticated;

            return View();
        }
    }
}
