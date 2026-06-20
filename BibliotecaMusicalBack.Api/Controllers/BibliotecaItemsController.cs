using BibliotecaMusicalBack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController,Route("api/biblioteca-items")]
public sealed class BibliotecaItemsController(ICatalogoRepository repository):ControllerBase
{
    [HttpGet] public async Task<IActionResult> Obtener([FromQuery]string? estado,[FromQuery]bool? esFisico,[FromQuery]bool soloActivos=true,CancellationToken ct=default)=>Ok(await repository.ObtenerItemsAsync(estado,esFisico,soloActivos,ct));
}
