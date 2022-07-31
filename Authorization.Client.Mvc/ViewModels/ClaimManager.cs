using System.Runtime.Serialization.Formatters;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Authorization.Client.Mvc.ViewModels;

public class ClaimManager
{
    public List<ClaimViewer> Items { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public ClaimManager(HttpContext httpContext, ClaimsPrincipal user)
    {
        if (httpContext is null) throw new ArgumentNullException(nameof(httpContext));
        Items = new List<ClaimViewer>();
        var claims = user.Claims;

        var idTokenJson =  httpContext.GetTokenAsync("id_token").GetAwaiter().GetResult();
        var accessTokenJson =  httpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
        var refreshToken = httpContext.GetTokenAsync("refresh_token").GetAwaiter().GetResult();
        
        AccessToken = accessTokenJson;
        RefreshToken = refreshToken;
        
        AddTokenInfo("Refresh token", refreshToken, true);
        AddTokenInfo("Identity token", idTokenJson);
        AddTokenInfo("Access token", accessTokenJson);
        AddTokenInfo("User Claims", claims);
    }

    public void AddTokenInfo(string tokenName, string idTokenJson, bool skipParsing = false)
    {
        Items.Add(new ClaimViewer(tokenName, idTokenJson, skipParsing));
    }
    
    public void AddTokenInfo(string tokenName, IEnumerable<Claim> claims)
    {
        Items.Add(new ClaimViewer(tokenName, claims));
    }
}
