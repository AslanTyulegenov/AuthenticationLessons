using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Authorization.IdentityServer.Services;

public class ProfileService : IProfileService
{

    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.DateOfBirth, "19.11.1999")
        };
        context.IssuedClaims.AddRange(claims);
        
        return Task.CompletedTask;
    }
    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;

        return Task.CompletedTask;
    }
}
