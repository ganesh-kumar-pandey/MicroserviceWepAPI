using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExtensionsLib
{
    public static class JwtAuthenticationExtensions
    {
        public static IServiceCollection AddCustomJwtAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            //var secretKey = "MySuperSecretAndSecureKeyThatIsAtLeast32BytesLongg";
            //var issuer    = "gatewayapi";
            //var audience  = "clientapp";

            services
       .AddAuthentication(options =>
       {
           options.DefaultAuthenticateScheme = "Bearer";
           options.DefaultChallengeScheme = "Bearer";
       })
       .AddJwtBearer("Bearer", options =>
       {
           options.RequireHttpsMetadata = false;
           options.SaveToken = true;

           options.TokenValidationParameters =
               new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,

                   ValidIssuer = "gatewayapi",
                   ValidAudience = "clientapp",

                   IssuerSigningKey =
                       new SymmetricSecurityKey(
                           Encoding.UTF8.GetBytes(
                               "MySuperSecretAndSecureKeyThatIsAtLeast32BytesLongg")),

                   ClockSkew = TimeSpan.Zero
               };

           options.Events = new JwtBearerEvents
           {
               OnAuthenticationFailed = context =>
               {
                   Console.WriteLine("AUTH FAILED");
                   Console.WriteLine(context.Exception);

                   return Task.CompletedTask;
               },

               OnTokenValidated = context =>
               {
                   Console.WriteLine("TOKEN VALIDATED");

                   return Task.CompletedTask;
               }
           };
       });

            return services;
        }
    }
}
