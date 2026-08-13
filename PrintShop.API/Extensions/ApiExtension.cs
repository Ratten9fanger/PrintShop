using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PrintShop.Infrastructure;
using System.Text;

namespace PrintShop.API.Extensions
{
    static class ApiExtension
    {
        public static void AddApiAuthentication(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // указывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = false,
                        // будет ли валидироваться потребитель токена
                        ValidateAudience = false,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,
                        // установка ключа безопасности
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["burmalda"];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }
    }
}
