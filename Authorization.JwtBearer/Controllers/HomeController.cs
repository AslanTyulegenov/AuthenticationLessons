using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.JwtBearer.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {
        
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }
    
    [Authorize]
    public async Task<IActionResult> Secret()
    {
        return View();
    }
    
    public async Task<IActionResult> Authenticate()
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "Aslan"),
            new Claim(JwtRegisteredClaimNames.Email, "aslan@mail.ru")
        };

        byte[] secretBytes = Encoding.UTF8.GetBytes(Constants.SecretKey);
        var key = new SymmetricSecurityKey(secretBytes);
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.Sha256);

        var token = new JwtSecurityToken(
        Constants.Issuer, 
        Constants.Audience,
        claims,
        notBefore: DateTime.Now, 
        expires: DateTime.Now.AddMinutes(60),
        signingCredentials);
        return View();
    }
}
