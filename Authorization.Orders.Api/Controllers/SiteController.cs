using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Orders.Api.Controllers;

[Route("[controller]")]
public class SiteController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [Authorize]
    [Route("[action]")]
    public async Task<string> Secret()
    {
        return "Secret string from Orders API";
    }
}
