using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Yourttoo.Api;
using Yourttoo.Api.DataAccess;
using Yourttoo.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔸 Forzar Kestrel a escuchar SÓLO en el puerto que Render expone
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseKestrel()
    .UseUrls($"http://0.0.0.0:{port}"); // un único puerto, nada de 80/8081

// DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlite(connectionString));

builder.Services.AddControllers();

// Servicios
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Swagger (puedes dejarlo también en Production si te interesa)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Accept-Language", new OpenApiSecurityScheme
    {
        Name = "Accept-Language",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Idioma de la petición, p. ej. 'es-ES' o 'en-US'"
    });

    c.OperationFilter<AcceptLanguageHeaderOperationFilter>();
});

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program)));

const string CorsPolicy = "frontend";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(CorsPolicy, p =>
        p.WithOrigins(
            "http://localhost:5173",                       // dev local
            "https://web-deploy-bve1.onrender.com"  // cambia por tu dominio del frontend
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
    // si algún día usas cookies/token en cabecera y necesitas credenciales:
    //.AllowCredentials()
    );
});

var app = builder.Build();

// 🔸 Procesa los headers del proxy de Render (Host / Proto)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor
                     | ForwardedHeaders.XForwardedProto
                     | ForwardedHeaders.XForwardedHost
});

// Swagger (si quieres sólo en Dev, deja tu condición)
app.UseSwagger();
app.UseSwaggerUI();

// Routing + endpoints
app.UseRouting();

// 🔸 Health check simple para Render
app.MapGet("/health", () => Results.Ok("OK"));

app.MapControllers();

app.Run();
