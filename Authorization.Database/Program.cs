using System.Security.Claims;
using Authorization.Database.Data;
using Authorization.Database.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(config => {
        config.UseInMemoryDatabase("MEMORY");
    })
    .AddIdentity<ApplicationUser, ApplicationRole>(config => {
        config.Password.RequireDigit = false;
        config.Password.RequireLowercase = false;
        config.Password.RequireUppercase = false;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
//     options => {
//         options.LoginPath = "/Admin/Login";
//         options.AccessDeniedPath = "/Home/AccessDenied";
//     });

builder.Services.ConfigureApplicationCookie(options => {
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

using(var scope = app.Services.CreateScope())
{
    DbInitializer.Init(scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>());
}

app.Run();


static class DbInitializer
{
    public static void Init(UserManager<ApplicationUser> userManager)
    {
        var user = new ApplicationUser
        {
            UserName = "Aslan",
            LastName = "LastName",
            FirstName = "FirstName"
        };

        var result = userManager.CreateAsync(user, "123qwe").GetAwaiter().GetResult();

        if (result.Succeeded)
        {
            userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Administrator")).GetAwaiter().GetResult();
        }
    }
}
