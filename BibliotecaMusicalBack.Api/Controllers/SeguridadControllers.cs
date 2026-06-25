using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

[ApiController, Route("api/usuarios-roles")]
public class UsuariosRolesController(ICrudService<UsuarioRolDto> service)
    : CrudControllerBase<UsuarioRolDto>(service)
{
}

[ApiController, Route("api/auditoria")]
public class AuditoriaController(ICrudService<AuditoriaDto> service)
    : CrudControllerBase<AuditoriaDto>(service)
{
}

[ApiController]
[Route("api/usuarios")]
public class UsuariosController(IUsuarioService service) : ControllerBase
{
    private readonly IUsuarioService _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UsuarioDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _service.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioDto>> GetById(int id, CancellationToken cancellationToken)
    {
        UsuarioDto? usuario = await _service.GetByIdAsync(id, cancellationToken);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Create(
        UsuarioRequest request,
        CancellationToken cancellationToken)
    {
        UsuarioDto created = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.UsuCodigo }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UsuarioRequest request,
        CancellationToken cancellationToken)
    {
        return await _service.UpdateAsync(id, request, cancellationToken) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await _service.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
    }
}
