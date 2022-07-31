var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddHttpClient();
services.AddControllersWithViews();

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(config => config.MapDefaultControllerRoute());
app.Run();
