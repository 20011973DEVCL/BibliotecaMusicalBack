using BibliotecaMusicalBack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class CancionesController(ICatalogoRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obtener([FromQuery]string? buscar,[FromQuery]int? albumCodigo,[FromQuery]int? artistaCodigo,[FromQuery]int? generoCodigo,[FromQuery]bool soloActivos=true,[FromQuery]int pagina=1,[FromQuery]int tamanoPagina=20,CancellationToken ct=default)
    { (pagina,tamanoPagina)=Paginacion.Normalizar(pagina,tamanoPagina);return Ok(await repository.ObtenerCancionesAsync(buscar,albumCodigo,artistaCodigo,generoCodigo,soloActivos,pagina,tamanoPagina,ct)); }

    [HttpGet("{codigo:int}")]
    public async Task<IActionResult> ObtenerPorCodigo(int codigo,CancellationToken ct){var item=await repository.ObtenerCancionAsync(codigo,ct);return item is null?NotFound():Ok(item);}
}
