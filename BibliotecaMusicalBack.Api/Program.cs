using BibliotecaMusicalBack.Application.Interfaces;
using BibliotecaMusicalBack.Application.Services;
using BibliotecaMusicalBack.Domain.Interfaces;
using BibliotecaMusicalBack.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Servicios base
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Inyección de dependencias
builder.Services.AddScoped<IGeneroService, GeneroService>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();