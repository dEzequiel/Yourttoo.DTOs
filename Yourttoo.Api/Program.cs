using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Yourttoo.Api;
using Yourttoo.Api.DataAccess;
using Yourttoo.Api.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlite(connectionString));

builder.Services.AddControllers();

// Registrar servicios de autenticación
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    // 🔹 Añadir el header Accept-Language globalmente
    c.AddSecurityDefinition("Accept-Language", new OpenApiSecurityScheme
    {
        Name = "Accept-Language",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Idioma de la petición, por ejemplo 'es-ES' o 'en-US'"
    });

    c.OperationFilter<AcceptLanguageHeaderOperationFilter>();

});
builder.Services.AddAutoMapper(cfg => {
    cfg.AddMaps(typeof(Program));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{;
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
