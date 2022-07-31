using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Authorization.Roles.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Manager()
        {
            return View();
        }

        [Authorize(Policy = "Administrator")]
        public async Task<IActionResult> Administrator()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/home/index");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var claims = new List<Claim>
            {
                new Claim("Demo", "Value"), 
                new Claim(ClaimTypes.Name, loginViewModel.UserName),
                new Claim(ClaimTypes.Role, "Administrator")
            };
            var ci = new ClaimsIdentity(claims, "Cookie");
            var cp = new ClaimsPrincipal(ci);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cp);

            return Redirect(loginViewModel.ReturnUrl);
        }
    }

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ReturnUrl { get; set; }
    }
}
