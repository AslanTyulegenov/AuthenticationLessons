using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authorization.Client.Mvc.ViewModels;

public class ClaimViewer
{
    public string Name { get; }
    public string Token { get; init; } = "n/a";
    public List<Claim> Claims { get; } 
    
    public ClaimViewer(string name, IEnumerable<Claim> claims)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Claims = claims.ToList();
    }
    
    public ClaimViewer(string name, string tokenJson, bool skipParsing = false)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        if(!skipParsing)
            Claims = ((JwtSecurityToken) new JwtSecurityTokenHandler().ReadToken(tokenJson)).Claims.ToList();
        Token = tokenJson;
    }
}
