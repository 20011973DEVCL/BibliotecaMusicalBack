using BibliotecaMusicalBack.Api.Controllers;
using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Shouldly;

namespace BibliotecaMusicalBack.Tests.Controllers;

public class UsuariosControllerTests
{
    [Fact]
    public async Task GetAll_DebeRetornarOk()
    {
        FakeUsuarioService service = new() { Items = [CreateDto()] };
        UsuariosController controller = new(service);

        ActionResult<IReadOnlyList<UsuarioDto>> response = await controller.GetAll(default);

        response.Result.ShouldBeOfType<OkObjectResult>();
    }

    [Theory]
    [InlineData(true, typeof(OkObjectResult))]
    [InlineData(false, typeof(NotFoundResult))]
    public async Task GetById_DebeRetornarElEstadoCorrespondiente(bool exists, Type expectedType)
    {
        FakeUsuarioService service = new() { Item = exists ? CreateDto() : null };
        UsuariosController controller = new(service);

        ActionResult<UsuarioDto> response = await controller.GetById(1, default);

        response.Result!.GetType().ShouldBe(expectedType);
    }

    [Fact]
    public async Task Create_DebeRetornarCreatedAtAction()
    {
        FakeUsuarioService service = new() { Item = CreateDto() };
        UsuariosController controller = new(service);

        ActionResult<UsuarioDto> response = await controller.Create(new UsuarioRequest(), default);

        CreatedAtActionResult result = response.Result.ShouldBeOfType<CreatedAtActionResult>();
        result.RouteValues!["id"].ShouldBe(1);
    }

    [Theory]
    [InlineData(true, typeof(NoContentResult))]
    [InlineData(false, typeof(NotFoundResult))]
    public async Task Update_DebeRetornarElEstadoCorrespondiente(bool exists, Type expectedType)
    {
        FakeUsuarioService service = new() { OperationResult = exists };
        UsuariosController controller = new(service);

        IActionResult response = await controller.Update(1, new UsuarioRequest(), default);

        response.GetType().ShouldBe(expectedType);
    }

    [Theory]
    [InlineData(true, typeof(NoContentResult))]
    [InlineData(false, typeof(NotFoundResult))]
    public async Task Delete_DebeRetornarElEstadoCorrespondiente(bool exists, Type expectedType)
    {
        FakeUsuarioService service = new() { OperationResult = exists };
        UsuariosController controller = new(service);

        IActionResult response = await controller.Delete(1, default);

        response.GetType().ShouldBe(expectedType);
    }

    private static UsuarioDto CreateDto()
    {
        return new UsuarioDto { UsuCodigo = 1, UsuEmail = "usuario@local.test" };
    }
}

internal sealed class FakeUsuarioService : IUsuarioService
{
    public IReadOnlyList<UsuarioDto> Items { get; set; } = [];
    public UsuarioDto? Item { get; set; }
    public bool OperationResult { get; set; }

    public Task<IReadOnlyList<UsuarioDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Items);
    }

    public Task<UsuarioDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Item);
    }

    public Task<UsuarioDto> CreateAsync(
        UsuarioRequest request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Item ?? new UsuarioDto());
    }

    public Task<bool> UpdateAsync(
        int id,
        UsuarioRequest request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(OperationResult);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(OperationResult);
    }
}
