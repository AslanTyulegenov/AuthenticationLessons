using System.Reflection;
using Authorization.IdentityServer;
using Authorization.IdentityServer.Data;
using Authorization.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var assemblyName = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

services.AddDbContext<ApplicationDbContext>(config => {
    config.UseNpgsql(connectionString);
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
    .AddConfigurationStore(options => {
        options.ConfigureDbContext = b => b.UseNpgsql(connectionString,
        sql => sql.MigrationsAssembly(assemblyName));
    })
    .AddOperationalStore(options => {
        options.ConfigureDbContext = b => b.UseNpgsql(connectionString,
        sql => sql.MigrationsAssembly(assemblyName));
    })
    // .AddInMemoryIdentityResources(IdentityServerConfigurations.GetIdentityResources())
    // .AddInMemoryApiResources(IdentityServerConfigurations.GetApiResources())
    // .AddInMemoryApiScopes(IdentityServerConfigurations.GetApiScopes())
    // .AddInMemoryClients(IdentityServerConfigurations.GetClients())    
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
