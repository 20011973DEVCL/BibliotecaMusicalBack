using BibliotecaMusicalBack.Application.Interfaces;
using BibliotecaMusicalBack.Application.Services;
using BibliotecaMusicalBack.Domain.Interfaces;
using BibliotecaMusicalBack.Infrastructure.Repositories;
using Npgsql;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGeneroService, GeneroService>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<ICatalogoRepository, CatalogoRepository>();

string connectionString = builder.Configuration.GetConnectionString("BibliotecaMusical")
    ?? throw new InvalidOperationException("Configure ConnectionStrings:BibliotecaMusical mediante User Secrets o variable de entorno.");
builder.Services.AddSingleton(NpgsqlDataSource.Create(connectionString));

WebApplication app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok", utc = DateTimeOffset.UtcNow }))
    .WithName("Health")
    .WithTags("Health");

app.Run();

public partial class Program;
