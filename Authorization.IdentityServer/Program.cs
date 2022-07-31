using Authorization.IdentityServer;
using Authorization.IdentityServer.Data;
using Authorization.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<ApplicationDbContext>(config => {
    config.UseInMemoryDatabase("MEMORY");
}).AddIdentity<IdentityUser, IdentityRole>(config => {
    config.Password.RequireDigit = false;
    config.Password.RequireLowercase = false;
    config.Password.RequireUppercase = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<ApplicationDbContext>();

services.ConfigureApplicationCookie(configuration => {
    configuration.Cookie.Name = "IdentityServer.Cookies";
    configuration.LoginPath = "/Auth/Login";
    configuration.LogoutPath = "/Auth/Logout";
});

services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddInMemoryIdentityResources(Configurations.GetIdentityResources())
    .AddInMemoryApiResources(Configurations.GetApiResources())
    .AddInMemoryApiScopes(Configurations.GetApiScopes())
    .AddInMemoryClients(Configurations.GetClients())    
    .AddProfileService<ProfileService>()
    .AddDeveloperSigningCredential();

services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();
app.UseRouting();
app.UseIdentityServer();
app.UseEndpoints(config => config.MapDefaultControllerRoute());

using(var scope = app.Services.CreateScope())
{
    DbInitializer.Init(scope.ServiceProvider);
}

app.Run();
