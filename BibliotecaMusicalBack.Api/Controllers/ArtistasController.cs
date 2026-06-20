using BibliotecaMusicalBack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class ArtistasController(ICatalogoRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obtener([FromQuery] string? buscar, [FromQuery] int? paisCodigo, [FromQuery] bool soloActivos=true, [FromQuery] int pagina=1, [FromQuery] int tamanoPagina=20, CancellationToken ct=default)
    {
        (pagina,tamanoPagina)=Paginacion.Normalizar(pagina,tamanoPagina);
        return Ok(await repository.ObtenerArtistasAsync(buscar,paisCodigo,soloActivos,pagina,tamanoPagina,ct));
    }

    [HttpGet("{codigo:int}")]
    public async Task<IActionResult> ObtenerPorCodigo(int codigo,CancellationToken ct)
    {
        var item=await repository.ObtenerArtistaAsync(codigo,ct); return item is null?NotFound():Ok(item);
    }
}
