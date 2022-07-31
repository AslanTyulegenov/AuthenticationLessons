using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
    config => {
        config.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.FromSeconds(5)
        };
        config.Authority = "https://localhost:5003";
        config.Audience = "OrdersAPI";
    });
services.AddControllersWithViews();

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(config => config.MapDefaultControllerRoute());
app.Run();
