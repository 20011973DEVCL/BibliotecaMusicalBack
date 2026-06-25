using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Services;
using BibliotecaMusicalBack.Domain.Entities;
using BibliotecaMusicalBack.Domain.Interfaces;

using Shouldly;

namespace BibliotecaMusicalBack.Tests.Services;

public class UsuarioServiceTests
{
    [Fact]
    public async Task Create_DebeGuardarHashYNoExponerloEnLaRespuesta()
    {
        FakeUsuarioRepository repository = new();
        UsuarioService service = new(repository);
        UsuarioRequest request = new()
        {
            UsuNombres = "Ana",
            UsuApellidos = "Prueba",
            UsuEmail = "ana@local.test",
            Password = "Clave-Segura-123"
        };

        UsuarioDto result = await service.CreateAsync(request);

        repository.Saved!.UsuPasswordHash.ShouldStartWith("PBKDF2-SHA256$");
        repository.Saved.UsuPasswordHash.ShouldNotContain(request.Password);
        result.UsuEmail.ShouldBe(request.UsuEmail);
        typeof(UsuarioDto).GetProperty(nameof(Usuario.UsuPasswordHash)).ShouldBeNull();
    }

    [Fact]
    public async Task Create_SinPassword_DebeRechazarLaSolicitud()
    {
        UsuarioService service = new(new FakeUsuarioRepository());

        await Should.ThrowAsync<ArgumentException>(
            () => service.CreateAsync(new UsuarioRequest { Password = null }));
    }
}

internal sealed class FakeUsuarioRepository : ICrudRepository<Usuario>
{
    public Usuario? Saved { get; private set; }

    public Task<IReadOnlyList<Usuario>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Usuario>>([]);
    }

    public Task<Usuario?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Usuario?>(Saved);
    }

    public Task<Usuario> CreateAsync(Usuario entity, CancellationToken cancellationToken = default)
    {
        entity.UsuCodigo = 1;
        Saved = entity;
        return Task.FromResult(entity);
    }

    public Task<bool> UpdateAsync(
        int id,
        Usuario entity,
        CancellationToken cancellationToken = default)
    {
        Saved = entity;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}
