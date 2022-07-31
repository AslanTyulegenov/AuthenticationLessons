using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddAuthentication(options => {
    options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
})
    .AddIdentityServerAuthentication(options => {
        options.ApiName = "SwaggerAPI";
        options.Authority = "https://localhost:5003";
        options.RequireHttpsMetadata = false;
    });

services.AddAuthorization();

services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1",
    new OpenApiInfo
    {
        Description = "Demo Swagger API v1",
        Title = "Swagger with IdentityServer4",
        Version = "1.0.0"
    });
    
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost:5003/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    {"SwaggerAPI", "Swagger API Demo"}
                }
            }
        }
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Scheme = "oauth2",
                Reference = new OpenApiReference
                {
                    Id = "oauth2",
                    Type = ReferenceType.SecurityScheme
                },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
services.AddControllers();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger UI demo");
    options.DocumentTitle = "Title";
    options.RoutePrefix = "docs";
    options.DocExpansion(DocExpansion.List);
    options.OAuthClientId("client_id_swagger");
    options.OAuthClientSecret("client_secret_swagger");
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoint => endpoint.MapControllers());
app.Run();
