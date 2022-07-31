using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Authorization.Database.Data;
using Authorization.Database.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Database.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
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
            await _signInManager.SignOutAsync();

            return Redirect("/home/index");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.UserName);
            
            if (user is null)
            {
                ModelState.AddModelError("UserName", "User not found");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            
            if(result.Succeeded) 
                return Redirect(model.ReturnUrl);

            return View(model);
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
