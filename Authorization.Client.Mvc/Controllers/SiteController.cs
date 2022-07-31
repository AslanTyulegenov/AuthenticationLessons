using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Authorization.Client.Mvc.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Authorization.Client.Mvc.Controllers;

[Route("[controller]")]
public class SiteController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    public SiteController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [Route("[action]")]
    public async Task<IActionResult> GoodBye()
    {
        return View();
    }
    
    [Authorize]
    [Route("[action]")]
    public async Task<IActionResult> Logout()
    {
        var parameters = new AuthenticationProperties
        {
            RedirectUri = "/Site/GoodBye"
        };
        return SignOut(parameters,
        CookieAuthenticationDefaults.AuthenticationScheme,
        OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Route("[action]")]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [Authorize]
    [Route("[action]")]
    public async Task<IActionResult> Secret()
    {
        var model = new ClaimManager(HttpContext, User);

        try
        {
            ViewBag.Message = await GetSecretAsync(model);
        }
        catch(Exception e)
        {
            await RefreshTokenIdentityModel(model.RefreshToken);
            var model2 = new ClaimManager(HttpContext, User);
            ViewBag.Message = await GetSecretAsync(model2);
        }


        return View(model);
    }

    [Authorize(Policy = "HasDateOfBirth")]
    [Route("[action]")]
    public async Task<IActionResult> Secret1()
    {
        var model = new ClaimManager(HttpContext, User);

        return View("Secret", model);
    }

    [Authorize(Policy = "OlderThan")]
    [Route("[action]")]
    public async Task<IActionResult> Secret2()
    {
        var model = new ClaimManager(HttpContext, User);

        return View("Secret", model);
    }

    private async Task<string> GetSecretAsync(ClaimManager model)
    {
        var client = _clientFactory.CreateClient();
        client.SetBearerToken(model.AccessToken);

        return await client.GetStringAsync("https://localhost:5001/Site/Secret");
    }

    private async Task RefreshTokenIdentityModel(string refreshToken)
    {
        var refreshClient = _clientFactory.CreateClient();
        var result = await refreshClient.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            ClientId = "client_id_mvc",
            ClientSecret = "client_secret_mvc",
            Address = "https://localhost:5003/connect/token",
            RefreshToken = refreshToken,
            Scope = "openid OrdersAPI offline_access",
            GrantType = "refresh_token"
        });

        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        authenticateResult.Properties.UpdateTokenValue("access_token", result.AccessToken);
        authenticateResult.Properties.UpdateTokenValue("refresh_token", result.RefreshToken);

        await HttpContext.SignInAsync(authenticateResult.Principal, authenticateResult.Properties);
    }

    private async Task RefreshToken(string refreshToken)
    {
        var refreshClient = _clientFactory.CreateClient();

        var parameters = new Dictionary<string, string>
        {
            ["refresh_token"] = refreshToken,
            ["grant_type"] = "refresh_token",
            ["client_id"] = "client_id_mvc",
            ["client_secret"] = "client_secret_mvc",
        };
        var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5003/connect/token")
        {
            Content = new FormUrlEncodedContent(parameters)
        };
        var basic = "client_id_mvc:client_secret_mvc";
        var encodedData = Encoding.UTF8.GetBytes(basic);
        var encodeData64Base = Convert.ToBase64String(encodedData);

        request.Headers.Add("Authorization", $"Bearer {encodeData64Base}");
        var response = await refreshClient.SendAsync(request);

        var tokenData = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenData);
        var accessTokenNew = tokenResponse.GetValueOrDefault("access_token");
        var refreshTokenNew = tokenResponse.GetValueOrDefault("refresh_token");

        var authenticate = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        authenticate.Properties.UpdateTokenValue("access_token", accessTokenNew);
        authenticate.Properties.UpdateTokenValue("refresh_token", refreshTokenNew);

        await HttpContext.SignInAsync(authenticate.Principal, authenticate.Properties);
    }
}
