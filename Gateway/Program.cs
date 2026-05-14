using ExtensionsLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("Router.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomJwtAuthentication(builder.Configuration);


builder.Services.AddAuthorization();

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseRouting();

//app.UseAuthentication();
app.Use(async (context, next) =>
{
    Console.WriteLine(context.Request.Headers.Authorization);

    await next();
});
//app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());
//app.MapControllers();
await app.UseOcelot();

app.Run();