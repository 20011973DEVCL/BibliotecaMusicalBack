using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;
using BibliotecaMusicalBack.Application.Services;
using BibliotecaMusicalBack.Domain.Entities;
using BibliotecaMusicalBack.Domain.Interfaces;
using BibliotecaMusicalBack.Infrastructure.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(CrudRepository<>));
AddCrudServices(builder.Services);
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseAuthorization();
app.MapControllers();
app.Run();

static void AddCrudServices(IServiceCollection services)
{
    AddCrud<Pais, PaisDto>(services);
    AddCrud<Genero, GeneroDto>(services);
    AddCrud<TipoArtista, TipoArtistaDto>(services);
    AddCrud<SelloDiscografico, SelloDiscograficoDto>(services);
    AddCrud<FormatoMusical, FormatoMusicalDto>(services);
    AddCrud<UbicacionFisica, UbicacionFisicaDto>(services);
    AddCrud<Rol, RolDto>(services);
    AddCrud<Artista, ArtistaDto>(services);
    AddCrud<ArtistaTipo, ArtistaTipoDto>(services);
    AddCrud<Album, AlbumDto>(services);
    AddCrud<Cancion, CancionDto>(services);
    AddCrud<CancionArtista, CancionArtistaDto>(services);
    AddCrud<Compositor, CompositorDto>(services);
    AddCrud<CancionCompositor, CancionCompositorDto>(services);
    AddCrud<Coleccion, ColeccionDto>(services);
    AddCrud<ColeccionAlbum, ColeccionAlbumDto>(services);
    AddCrud<BibliotecaItem, BibliotecaItemDto>(services);
    AddCrud<Playlist, PlaylistDto>(services);
    AddCrud<PlaylistCancion, PlaylistCancionDto>(services);
    AddCrud<UsuarioRol, UsuarioRolDto>(services);
    AddCrud<Auditoria, AuditoriaDto>(services);
}

static void AddCrud<TEntity, TDto>(IServiceCollection services)
    where TEntity : class, new()
    where TDto : class, new()
{
    services.AddScoped<ICrudService<TDto>, CrudService<TEntity, TDto>>();
}

public partial class Program
{
}
