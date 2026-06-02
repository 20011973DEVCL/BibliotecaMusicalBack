using BibliotecaMusicalBack.Application.Interfaces;
using BibliotecaMusicalBack.Application.Services;
using BibliotecaMusicalBack.Domain.Interfaces;
using BibliotecaMusicalBack.Infrastructure.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Servicios base
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias
builder.Services.AddScoped<IGeneroService, GeneroService>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();

WebApplication app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Por ahora puedes dejarlo comentado para evitar warning HTTPS en desarrollo
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();