using BibliotecaMusicalBack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class AlbumsController(ICatalogoRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obtener([FromQuery]string? buscar,[FromQuery]int? artistaCodigo,[FromQuery]int? generoCodigo,[FromQuery]int? anio,[FromQuery]bool soloActivos=true,[FromQuery]int pagina=1,[FromQuery]int tamanoPagina=20,CancellationToken ct=default)
    { (pagina,tamanoPagina)=Paginacion.Normalizar(pagina,tamanoPagina); return Ok(await repository.ObtenerAlbumsAsync(buscar,artistaCodigo,generoCodigo,anio,soloActivos,pagina,tamanoPagina,ct)); }

    [HttpGet("{codigo:int}")]
    public async Task<IActionResult> ObtenerPorCodigo(int codigo,CancellationToken ct){var item=await repository.ObtenerAlbumAsync(codigo,ct);return item is null?NotFound():Ok(item);}
}
