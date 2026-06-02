using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenerosController(IGeneroService generoService) : ControllerBase
{
    private readonly IGeneroService _generoService = generoService;

    [HttpGet]
    public async Task<IActionResult> ObtenerGeneros()
    {
        List<GeneroDto> resultado = await _generoService.ObtenerGenerosAsync();
        return Ok(resultado);
    }
}