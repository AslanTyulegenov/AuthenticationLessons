using System.Security.Claims;
using Authorization.Client.Mvc.Requirements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddAuthentication(config => {
        config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
    config => {
        config.Authority = "https://localhost:5003";
        config.ClientId = "client_id_mvc";
        config.ClientSecret = "client_secret_mvc";
        config.SaveTokens = true;

        config.ResponseType = "code";
        config.GetClaimsFromUserInfoEndpoint = true;
        config.Scope.Add("OrdersAPI");
        config.Scope.Add("offline_access");

        config.ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, ClaimTypes.DateOfBirth);
    });

services.AddAuthorization(config => {
    config.AddPolicy("HasDateOfBirth",
    builder => {
        builder.RequireClaim(ClaimTypes.DateOfBirth);
    });

    // config.AddPolicy("OlderThan",
    // builder => {
    //     builder.AddRequirements(new OlderThanRequirement(20));
    // });
});

services.AddSingleton<IAuthorizationHandler, OlderThanRequirementHandler>();
services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
services.AddHttpClient();
services.AddControllersWithViews();

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(config =>
    config.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Site}/{action=Index}/{id?}"));
app.Run();
