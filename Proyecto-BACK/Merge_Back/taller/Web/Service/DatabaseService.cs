using Entity.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // 👈 importante

namespace Web.Service
{
    public static class DatabaseService
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            var sql = config.GetConnectionString("SqlServer");
            var pg = config.GetConnectionString("Postgres");
            var my = config.GetConnectionString("MySql");

            // Registrar AuditManager si lo usas en ApplicationDbContext
            //services.AddScoped<AuditService>();
            if (!string.IsNullOrWhiteSpace(sql))
            {
                services.AddDbContext<ApplicationDbContext>(opt =>
                    opt.UseSqlServer(sql, s =>
                    {
                        s.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        s.EnableRetryOnFailure();
                        s.CommandTimeout(60);
                    }));
            }

            if (!string.IsNullOrWhiteSpace(pg))
            {
                services.AddDbContext<PostgresDbContext>(opt =>
                    opt.UseNpgsql(pg, n =>
                    {
                        n.MigrationsAssembly(typeof(PostgresDbContext).Assembly.FullName);
                        n.EnableRetryOnFailure();
                        n.CommandTimeout(60);
                    }));
            }

            if (!string.IsNullOrWhiteSpace(my))
            {
                services.AddDbContext<MySqlApplicationDbContext>(opt =>
                    opt.UseMySql(my, ServerVersion.AutoDetect(my), m =>
                    {
                        m.MigrationsAssembly(typeof(MySqlApplicationDbContext).Assembly.FullName);

                        // 👇 Ignorar schemas (MySQL no soporta schemas)
                        m.SchemaBehavior(MySqlSchemaBehavior.Ignore);

            // Registrar AuditDbContext si es necesario, con proveedor fijo o dinámico
            //services.AddDbContext<AuditDbContext>(options =>
            // options.UseSqlServer(configuration.GetConnectionString("Audit")));

                        // Habilitar traducciones de comparación de strings
                        m.EnableStringComparisonTranslations();
                    })
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging() // ⚠️ solo en desarrollo
                );
            }

            return services;
        }
    }
}
