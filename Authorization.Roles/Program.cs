using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
    options => {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });
builder.Services.AddAuthorization(options => {
    options.AddPolicy("Administrator",
    builder => {
        builder.RequireClaim(ClaimTypes.Role, "Administrator");
    });
    options.AddPolicy("Manager",
    builder => {
        builder.RequireAssertion(x =>
            x.User.HasClaim(ClaimTypes.Role, "Manager")
            || x.User.HasClaim(ClaimTypes.Role, "Administrator"));
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(configure => {
    configure.MapDefaultControllerRoute();
});

app.Run();
