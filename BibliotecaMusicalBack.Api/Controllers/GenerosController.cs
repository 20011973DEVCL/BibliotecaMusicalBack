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
    [ProducesResponseType<List<GeneroDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerGeneros([FromQuery] bool soloActivos = true, CancellationToken cancellationToken = default)
    {
        List<GeneroDto> resultado = await _generoService.ObtenerGenerosAsync(soloActivos, cancellationToken);
        return Ok(resultado);
    }

    [HttpGet("{codigo:int}")]
    public async Task<IActionResult> ObtenerPorCodigo(int codigo, CancellationToken cancellationToken)
    {
        GeneroDto? genero = await _generoService.ObtenerPorCodigoAsync(codigo, cancellationToken);
        return genero is null ? NotFound() : Ok(genero);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] GuardarGeneroDto dto, CancellationToken cancellationToken)
    {
        IActionResult? error = Validar(dto);
        if (error is not null) return error;
        GeneroDto creado = await _generoService.CrearAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorCodigo), new { codigo = creado.GenCodigo }, creado);
    }

    [HttpPut("{codigo:int}")]
    public async Task<IActionResult> Actualizar(int codigo, [FromBody] GuardarGeneroDto dto, CancellationToken cancellationToken)
    {
        IActionResult? error = Validar(dto);
        if (error is not null) return error;
        GeneroDto? actualizado = await _generoService.ActualizarAsync(codigo, dto, cancellationToken);
        return actualizado is null ? NotFound() : Ok(actualizado);
    }

    [HttpDelete("{codigo:int}")]
    public async Task<IActionResult> Eliminar(int codigo, CancellationToken cancellationToken) =>
        await _generoService.EliminarAsync(codigo, cancellationToken) ? NoContent() : NotFound();

    private IActionResult? Validar(GuardarGeneroDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre)) ModelState.AddModelError(nameof(dto.Nombre), "El nombre es obligatorio.");
        if (dto.Nombre?.Trim().Length > 100) ModelState.AddModelError(nameof(dto.Nombre), "El nombre admite hasta 100 caracteres.");
        if (dto.Descripcion?.Trim().Length > 300) ModelState.AddModelError(nameof(dto.Descripcion), "La descripción admite hasta 300 caracteres.");
        return ModelState.IsValid ? null : ValidationProblem(ModelState);
    }
}
