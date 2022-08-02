using System.Security.Claims;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorization.IdentityServer.Data;

public static class DbInitializer
{
    public static void Init(IServiceProvider provider)
    {
        var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
        var user = new IdentityUser
        {
            UserName = "Aslan"
        };
        var result = userManager.CreateAsync(user, "qwe123").GetAwaiter().GetResult();

        if (result.Succeeded)
        {
            userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "administrator")).GetAwaiter().GetResult();
        }
        
        provider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
    
        var context = provider.GetRequiredService<ConfigurationDbContext>();
        context.Database.Migrate();
        if (!context.Clients.Any())
        {
            foreach (var client in IdentityServerConfigurations.GetClients())
            {
                context.Clients.Add(client.ToEntity());
            }
            context.SaveChanges();
        }
    
        if (!context.IdentityResources.Any())
        {
            foreach (var resource in IdentityServerConfigurations.GetIdentityResources())
            {
                context.IdentityResources.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }
    
        if (!context.ApiResources.Any())
        {
            foreach (var resource in IdentityServerConfigurations.GetApiResources())
            {
                context.ApiResources.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }
        
        if (!context.ApiScopes.Any())
        {
            foreach (var resource in IdentityServerConfigurations.GetApiScopes())
            {
                context.ApiScopes.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }
    } 
}
