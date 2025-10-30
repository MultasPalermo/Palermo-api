namespace Web.Service
{
    public static class CorsService
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            // Soporta arreglo "AllowedOrigins" o string "OrigenesPermitidos"
            var fromArray = configuration.GetSection("AllowedOrigins").Get<string[]>();
            string? fromString = configuration.GetValue<string>("OrigenesPermitidos");

            string[] origins = Array.Empty<string>();
            if (fromArray is { Length: > 0 })
            {
                origins = fromArray;
            }
            else if (!string.IsNullOrWhiteSpace(fromString))
            {
                origins = fromString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCors", policy =>
                {
                    if (origins.Length > 0)
                        policy.WithOrigins(origins);
                    else
                        policy.AllowAnyOrigin(); // opcional si prefieres restringir siempre

                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }
    }
}

