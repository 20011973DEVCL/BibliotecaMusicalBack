using BibliotecaMusicalBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace BibliotecaMusicalBack.Api.Controllers;

public abstract class CrudControllerBase<TDto>(ICrudService<TDto> service) : ControllerBase
    where TDto : class, new()
{
    private readonly ICrudService<TDto> _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TDto>>> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<TDto> result = await _service.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TDto>> GetById(int id, CancellationToken cancellationToken)
    {
        TDto? result = await _service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TDto>> Create(TDto dto, CancellationToken cancellationToken)
    {
        TDto created = await _service.CreateAsync(dto, cancellationToken);
        int id = ResourceKeyReader.GetId(created);
        return CreatedAtAction(nameof(GetById), new { id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TDto dto, CancellationToken cancellationToken)
    {
        bool updated = await _service.UpdateAsync(id, dto, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        bool deleted = await _service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
