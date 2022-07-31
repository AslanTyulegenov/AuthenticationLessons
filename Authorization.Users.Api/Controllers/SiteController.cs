using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Users.Api.Controllers;

[Route("[controller]")]
public class SiteController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SiteController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    [Route("[action]")]
    public async Task<IActionResult> Index()
    {
        return View();
    }
    
    [Route("[action]")]
    public async Task<IActionResult> GetOrders()
    {
        var authClient = _httpClientFactory.CreateClient();
        var discoveryDoc = await authClient.GetDiscoveryDocumentAsync("https://localhost:5003");
        var tokenResponse = await authClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryDoc.TokenEndpoint,
            
            ClientId = "client_id",
            ClientSecret = "client_secret",
            Scope = "OrdersAPI"
        });


        var ordersClient = _httpClientFactory.CreateClient();
        ordersClient.SetBearerToken(tokenResponse.AccessToken);
        
        var response = await ordersClient.GetAsync("https://localhost:5001/site/secret");
        var message = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Message = response.StatusCode.ToString();
            return View();
        }

        ViewBag.Message = message;
        return View();
    }
}
