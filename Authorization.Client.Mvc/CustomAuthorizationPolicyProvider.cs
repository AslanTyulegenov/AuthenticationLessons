using Authorization.Client.Mvc.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

public class CustomAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;

    public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
        _options = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policyExists = await base.GetPolicyAsync(policyName);

        if (policyExists is null)
        {
            policyExists = new AuthorizationPolicyBuilder().AddRequirements(new OlderThanRequirement(20)).Build();
            _options.AddPolicy(policyName, policyExists);
        }
        
        return policyExists;
    }
}