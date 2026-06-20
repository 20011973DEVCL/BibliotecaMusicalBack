using BibliotecaMusicalBack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController,Route("api/[controller]")]
public sealed class CatalogosController(ICatalogoRepository repository):ControllerBase
{
    private static readonly HashSet<string> Permitidos=["paises","formatos","sellos","tipos-artista","ubicaciones"];
    [HttpGet("{catalogo}")]
    public async Task<IActionResult> Obtener(string catalogo,[FromQuery]bool soloActivos=true,CancellationToken ct=default)
    {
        catalogo=catalogo.ToLowerInvariant();
        return Permitidos.Contains(catalogo)?Ok(await repository.ObtenerCatalogoAsync(catalogo,soloActivos,ct)):NotFound();
    }
}
