using BibliotecaMusicalBack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

public sealed record CrearPlaylistRequest(string Nombre,string? Descripcion);
public sealed record AgregarCancionRequest(int CancionCodigo,int? Orden);

[ApiController, Route("api/[controller]")]
public sealed class PlaylistsController(ICatalogoRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obtener([FromQuery]bool soloActivos=true,CancellationToken ct=default)=>Ok(await repository.ObtenerPlaylistsAsync(soloActivos,ct));

    [HttpGet("{codigo:int}")]
    public async Task<IActionResult> ObtenerPorCodigo(int codigo,CancellationToken ct)
    {
        var playlist=await repository.ObtenerPlaylistAsync(codigo,ct);
        return playlist is null?NotFound():Ok(new { playlist, canciones=await repository.ObtenerCancionesPlaylistAsync(codigo,ct) });
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearPlaylistRequest request,CancellationToken ct)
    {
        if(string.IsNullOrWhiteSpace(request.Nombre)||request.Nombre.Trim().Length>150){ModelState.AddModelError(nameof(request.Nombre),"El nombre es obligatorio y admite hasta 150 caracteres.");return ValidationProblem(ModelState);}
        if(request.Descripcion?.Trim().Length>300){ModelState.AddModelError(nameof(request.Descripcion),"La descripción admite hasta 300 caracteres.");return ValidationProblem(ModelState);}
        var item=await repository.CrearPlaylistAsync(request.Nombre.Trim(),string.IsNullOrWhiteSpace(request.Descripcion)?null:request.Descripcion.Trim(),ct);
        return CreatedAtAction(nameof(ObtenerPorCodigo),new{codigo=item.Codigo},item);
    }

    [HttpPut("{codigo:int}/canciones")]
    public async Task<IActionResult> AgregarCancion(int codigo,AgregarCancionRequest request,CancellationToken ct)=>
        await repository.AgregarCancionPlaylistAsync(codigo,request.CancionCodigo,request.Orden,ct)?NoContent():NotFound();

    [HttpDelete("{codigo:int}/canciones/{cancionCodigo:int}")]
    public async Task<IActionResult> QuitarCancion(int codigo,int cancionCodigo,CancellationToken ct)=>
        await repository.QuitarCancionPlaylistAsync(codigo,cancionCodigo,ct)?NoContent():NotFound();

    [HttpDelete("{codigo:int}")]
    public async Task<IActionResult> Eliminar(int codigo,CancellationToken ct)=>await repository.EliminarPlaylistAsync(codigo,ct)?NoContent():NotFound();
}
