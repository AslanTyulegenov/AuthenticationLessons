using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Authorization.Client.Mvc.Requirements;

public class OlderThanRequirement : IAuthorizationRequirement
{
    public int Years { get; }
    public OlderThanRequirement(int years)
    {
        Years = years;
    }
}

public class OlderThanRequirementHandler : AuthorizationHandler<OlderThanRequirement>
{

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OlderThanRequirement requirement)
    {
        var hasClaim = context.User.HasClaim(x => x.Type is ClaimTypes.DateOfBirth);
        
        if(!hasClaim) return Task.CompletedTask;
        
        var dateOfBirth = context.User.FindFirst(x => x.Type is ClaimTypes.DateOfBirth)?.Value;
        var date = DateTime.Parse(dateOfBirth, new CultureInfo("ru-RU"));
        var currentAge = DateTime.Now.Year - date.Year;

        if (currentAge >= requirement.Years)
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}