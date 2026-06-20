using BibliotecaMusicalBack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController,Route("api/[controller]")]
public sealed class ColeccionesController(ICatalogoRepository repository):ControllerBase
{
    [HttpGet] public async Task<IActionResult> Obtener([FromQuery]bool soloActivos=true,CancellationToken ct=default)=>Ok(await repository.ObtenerColeccionesAsync(soloActivos,ct));
    [HttpGet("{codigo:int}")] public async Task<IActionResult> ObtenerPorCodigo(int codigo,CancellationToken ct){var item=await repository.ObtenerColeccionAsync(codigo,ct);return item is null?NotFound():Ok(new{coleccion=item,albums=await repository.ObtenerAlbumsColeccionAsync(codigo,ct)});}
}
