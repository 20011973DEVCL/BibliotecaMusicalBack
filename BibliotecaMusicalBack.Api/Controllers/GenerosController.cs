using BibliotecaMusicalBack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenerosController : ControllerBase
{
    private readonly IGeneroService _generoService;

    public GenerosController(IGeneroService generoService)
    {
        _generoService = generoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerGeneros()
    {
        var resultado = await _generoService.ObtenerGenerosAsync();
        return Ok(resultado);
    }
}