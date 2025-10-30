using Business.validaciones.Entities.DocumentInfraction;
using Business.validaciones.Entities.InspectoraReport;
using Entity.Domain.Models.Implements.Entities;
using Entity.Domain.Models.Implements.ModelSecurity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Web.Service;
using Web.Infrastructure;
using Business.Mensajeria.Email.implements;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Servicios
// --------------------
builder.Services.AddControllers();

builder.Services.AddSignalR();


// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   // ✅ necesario para que funcione Swagger

// FluentValidation
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddCustomValidators();

// DI de tu aplicación (incluye reCAPTCHA + sesión + servicios Business)
builder.Services.AddApplicationServices(builder.Configuration);

// Configuración de opciones
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<CookieSettings>(builder.Configuration.GetSection("CookieSettings"));
builder.Services.AddSingleton<ISystemClock, SystemClock>();

// DB dinámica (usa tu DbContextFactory)
builder.Services.AddDatabase(builder.Configuration);

// Auth JWT leyendo JwtSettings (options pattern)
builder.Services.AddJwtAuthentication(builder.Configuration);

// Autorización
builder.Services.AddAuthorization();

builder.Services.AddMemoryCache();


// CORS
builder.Services.AddCustomCors(builder.Configuration);

var app = builder.Build();

app.MapHub<Web.Hubs.InfractionHub>("/infractionHub");


// --------------------
// Middleware
// --------------------
app.UseStaticFiles();

// Swagger (en Dev/Prod según tu lógica)
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = "swagger";
    });
}
app.UseMiddleware<ExceptionMiddleware>();


//app.UseHttpsRedirection(); // habilítalo si lo necesitas

app.UseCors("DefaultCors");   // ✅ usa solo la política configurada

app.UseAuthentication();      // primero autenticación
app.UseAuthorization();       // luego autorización

app.MapControllers();

// Migraciones
MigrationManager.MigrateAllDatabases(app.Services, builder.Configuration);

app.Run();
