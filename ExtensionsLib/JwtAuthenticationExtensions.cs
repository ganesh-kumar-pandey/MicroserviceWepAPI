using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace ExtensionsLib
{
    public static class JwtAuthenticationExtensions
    {
        // Notice the 'this IServiceCollection' - this makes it an extension method
        public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Dynamically pull values from the appsettings.json of the project calling this method
            var secretKey = configuration["Jwt:SecretKey"];
            var issuer = configuration["Jwt:Issuer"] ?? "gatewayapi";
            var audience = configuration["Jwt:Audience"] ?? "clientapp";
            //var secretKey = "MySuperSecretAndSecureKeyThatIsAtLeast32BytesLong!";
            //var issuer = "gatewayapi";
            //var audience = "clientapp";

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("Jwt:SecretKey", "JWT Secret Key is missing in appsettings.json");
            }

            // 2. The explicit configuration to prevent the 500 Default Challenge Error
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
  .AddJwtBearer(options =>  // ← no explicit name
  {
      options.RequireHttpsMetadata = false;
      options.SaveToken = true;
      options.IncludeErrorDetails = true;

      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = issuer,
          ValidAudience = audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
          ClockSkew = TimeSpan.FromMinutes(5)  // ← safer default
      };

      options.Events = new JwtBearerEvents
      {
          // OnMessageReceived removed — middleware handles extraction natively
          OnAuthenticationFailed = context =>
          {
              Console.ForegroundColor = ConsoleColor.Red;
              Console.WriteLine($"\n--- JWT AUTH FAILED: {context.Exception.Message} ---\n");
              Console.ResetColor();
              return Task.CompletedTask;
          }
      };
  });

            return services;
        }
    }
}
