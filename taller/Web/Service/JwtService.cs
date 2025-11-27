using Business.Custom;
using Business.Interfaces.IJWT;
using Entity.Domain.Models.Implements.ModelSecurity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Web.Service
{
    public static class JwtService
    {

            public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
            {
                // Validar configuración JWT
                var jwtKey = configuration["Jwt:key"];
                var jwtIssuer = configuration["Jwt:Issuer"];
                var jwtAudience = configuration["Jwt:Audience"];

                if (string.IsNullOrEmpty(jwtKey))
                {
                    throw new InvalidOperationException("La configuración 'Jwt:key' no está definida en appsettings.json");
                }

                if (string.IsNullOrEmpty(jwtIssuer))
                {
                    throw new InvalidOperationException("La configuración 'Jwt:Issuer' no está definida en appsettings.json");
                }

                if (string.IsNullOrEmpty(jwtAudience))
                {
                    throw new InvalidOperationException("La configuración 'Jwt:Audience' no está definida en appsettings.json");
                }

                services.AddAuthentication(config =>
                {
                    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey)
                        )
                    };

                    // ✅ Leer desde la cookie
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Cookies["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
