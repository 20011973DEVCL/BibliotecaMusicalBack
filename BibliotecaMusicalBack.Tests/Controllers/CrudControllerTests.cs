using BibliotecaMusicalBack.Api.Controllers;
using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Shouldly;

namespace BibliotecaMusicalBack.Tests.Controllers;

public class CrudControllerTests
{
    [Fact]
    public async Task GetAll_DebeRetornarOkConLosRecursos()
    {
        GeneroDto expected = new() { GenCodigo = 1, GenNombre = "Rock" };
        FakeCrudService<GeneroDto> service = new() { Items = [expected] };
        GenerosController controller = new(service);

        ActionResult<IReadOnlyList<GeneroDto>> response = await controller.GetAll(default);

        OkObjectResult result = response.Result.ShouldBeOfType<OkObjectResult>();
        result.Value.ShouldBe(service.Items);
    }

    [Fact]
    public async Task GetById_CuandoExiste_DebeRetornarOk()
    {
        GeneroDto expected = new() { GenCodigo = 1, GenNombre = "Rock" };
        FakeCrudService<GeneroDto> service = new() { Item = expected };
        GenerosController controller = new(service);

        ActionResult<GeneroDto> response = await controller.GetById(1, default);

        OkObjectResult result = response.Result.ShouldBeOfType<OkObjectResult>();
        result.Value.ShouldBe(expected);
    }

    [Fact]
    public async Task GetById_CuandoNoExiste_DebeRetornarNotFound()
    {
        GenerosController controller = new(new FakeCrudService<GeneroDto>());

        ActionResult<GeneroDto> response = await controller.GetById(99, default);

        response.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_DebeRetornarCreatedAtAction()
    {
        GeneroDto dto = new() { GenCodigo = 8, GenNombre = "Ambient" };
        FakeCrudService<GeneroDto> service = new() { Item = dto };
        GenerosController controller = new(service);

        ActionResult<GeneroDto> response = await controller.Create(dto, default);

        CreatedAtActionResult result = response.Result.ShouldBeOfType<CreatedAtActionResult>();
        result.ActionName.ShouldBe(nameof(controller.GetById));
        result.RouteValues!["id"].ShouldBe(8);
        result.Value.ShouldBe(dto);
    }

    [Theory]
    [InlineData(true, typeof(NoContentResult))]
    [InlineData(false, typeof(NotFoundResult))]
    public async Task Update_DebeRetornarElEstadoCorrespondiente(bool exists, Type expectedType)
    {
        FakeCrudService<GeneroDto> service = new() { OperationResult = exists };
        GenerosController controller = new(service);

        IActionResult response = await controller.Update(1, new GeneroDto(), default);

        response.GetType().ShouldBe(expectedType);
    }

    [Theory]
    [InlineData(true, typeof(NoContentResult))]
    [InlineData(false, typeof(NotFoundResult))]
    public async Task Delete_DebeRetornarElEstadoCorrespondiente(bool exists, Type expectedType)
    {
        FakeCrudService<GeneroDto> service = new() { OperationResult = exists };
        GenerosController controller = new(service);

        IActionResult response = await controller.Delete(1, default);

        response.GetType().ShouldBe(expectedType);
    }
}

internal sealed class FakeCrudService<TDto> : ICrudService<TDto>
    where TDto : class, new()
{
    public IReadOnlyList<TDto> Items { get; set; } = [];
    public TDto? Item { get; set; }
    public bool OperationResult { get; set; }

    public Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Items);
    }

    public Task<TDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Item);
    }

    public Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Item ?? dto);
    }

    public Task<bool> UpdateAsync(int id, TDto dto, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(OperationResult);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(OperationResult);
    }
}
