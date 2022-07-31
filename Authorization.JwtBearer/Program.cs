using System.Text;
using Authorization.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddAuthentication("OAuth")
    .AddJwtBearer("OAuth",
    config => {
        var key = Encoding.UTF8.GetBytes(Constants.SecretKey);
        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = Constants.Issuer,
            ValidAudience = Constants.Audience,
            IssuerSigningKey= new SymmetricSecurityKey(key)
        };
    });


services.AddControllersWithViews();


var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(configure => configure.MapDefaultControllerRoute());

app.Run();
