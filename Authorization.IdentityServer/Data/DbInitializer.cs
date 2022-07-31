using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Authorization.IdentityServer.Data;

public static class DbInitializer
{
    public static void Init(IServiceProvider provider)
    {
        var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
        var user = new IdentityUser
        {
            UserName = "Aslan"
        };
        var result = userManager.CreateAsync(user, "qwe123").GetAwaiter().GetResult();

        if (result.Succeeded)
        {
            userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "administrator")).GetAwaiter().GetResult();
        }
    } 
}
