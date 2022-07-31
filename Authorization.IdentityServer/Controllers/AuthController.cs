using Authorization.IdentityServer.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.IdentityServer.Controllers;

[Route("[controller]")]
public class AuthController : Controller 
{
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    
    public AuthController(
        IIdentityServerInteractionService interactionService,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager)
    {
        _interactionService = interactionService;
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
    {
        return View(new LoginViewModel
        {
            UserName = "Aslan",
            Password = "qwe123",
            ReturnUrl = returnUrl
        });
    }
    
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user is null )
        {
            ModelState.AddModelError("UserName", "User not found");
            return View();
        }
       
        var result =  await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        
        if(result.Succeeded)
            return Redirect(model.ReturnUrl);

        ModelState.AddModelError("UserName", "Something  went wrong ");
        return View(model);
    }

    [Route("[action]")]
    public async Task<IActionResult> Logout(string logoutId)
    {
        await _signInManager.SignOutAsync();
        var result = await _interactionService.GetLogoutContextAsync(logoutId);

        if (string.IsNullOrEmpty(result.PostLogoutRedirectUri))
            return RedirectToAction("Login", "Auth");

        return Redirect(result.PostLogoutRedirectUri);
    }
}
