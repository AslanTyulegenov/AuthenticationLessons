using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;

namespace Authorization.IdentityServer;

public static class Configurations
{

    public static IEnumerable<IdentityResource> GetIdentityResources() => new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    };

    public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
    {
        new ApiResource("OrdersAPI", "Orders API")
        {
            Scopes = { "OrdersAPI" }
        },
        new ApiResource("SwaggerAPI", "Swagger API")
        {
            Scopes = { "SwaggerAPI" }
        }
    };

    public static IEnumerable<Client> GetClients() => new List<Client>
    {
        new Client
        {
            ClientId = "client_id",
            ClientSecrets = { new Secret("client_secret".ToSha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes =
            {
                "OrdersAPI"
            },
        },
        new Client
        {
            ClientId = "client_id_mvc",
            ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            AllowedScopes =
            {
                "OrdersAPI",
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile
            },
            
            RedirectUris = { "https://localhost:5004/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:5004/signout-callback-oidc" },
            RequireConsent = false,
            AccessTokenLifetime = 5,
            AllowOfflineAccess = true
            //AlwaysIncludeUserClaimsInIdToken = true
        },
        new Client
        {
            ClientId = "client_id_swagger",
            ClientSecrets = {new Secret("client_secret_swagger".ToSha256())},
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AllowedScopes =
            {
                "SwaggerAPI",
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile
            },
            AllowedCorsOrigins = new List<string>
            {
                "https://localhost:5005"
            }
        }
    };
    public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>
    {
        new ApiScope("OrdersAPI"),
        new ApiScope("SwaggerAPI")
    };
}
