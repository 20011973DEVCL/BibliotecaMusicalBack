using System.Security.Cryptography;

using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;
using BibliotecaMusicalBack.Domain.Entities;
using BibliotecaMusicalBack.Domain.Interfaces;

namespace BibliotecaMusicalBack.Application.Services;

public class UsuarioService(ICrudRepository<Usuario> repository) : IUsuarioService
{
    private readonly ICrudRepository<Usuario> _repository = repository;

    public async Task<IReadOnlyList<UsuarioDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Usuario> usuarios = await _repository.GetAllAsync(cancellationToken);
        return [.. usuarios.Select(ToDto)];
    }

    public async Task<UsuarioDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Usuario? usuario = await _repository.GetByIdAsync(id, cancellationToken);
        return usuario is null ? null : ToDto(usuario);
    }

    public async Task<UsuarioDto> CreateAsync(
        UsuarioRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("La contraseña es obligatoria para crear un usuario.", nameof(request));
        }

        Usuario usuario = ToEntity(request);
        usuario.UsuPasswordHash = HashPassword(request.Password);
        Usuario created = await _repository.CreateAsync(usuario, cancellationToken);
        return ToDto(created);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UsuarioRequest request,
        CancellationToken cancellationToken = default)
    {
        Usuario? current = await _repository.GetByIdAsync(id, cancellationToken);
        if (current is null)
        {
            return false;
        }

        Usuario usuario = ToEntity(request);
        usuario.UsuPasswordHash = string.IsNullOrWhiteSpace(request.Password)
            ? current.UsuPasswordHash
            : HashPassword(request.Password);

        return await _repository.UpdateAsync(id, usuario, cancellationToken);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(id, cancellationToken);
    }

    private static Usuario ToEntity(UsuarioRequest request)
    {
        return new Usuario
        {
            UsuNombres = request.UsuNombres,
            UsuApellidos = request.UsuApellidos,
            UsuEmail = request.UsuEmail,
            UsuActivo = request.UsuActivo
        };
    }

    private static UsuarioDto ToDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            UsuCodigo = usuario.UsuCodigo,
            UsuNombres = usuario.UsuNombres,
            UsuApellidos = usuario.UsuApellidos,
            UsuEmail = usuario.UsuEmail,
            UsuActivo = usuario.UsuActivo,
            UsuFechaCreacion = usuario.UsuFechaCreacion
        };
    }

    private static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            100_000,
            HashAlgorithmName.SHA256,
            32);

        return $"PBKDF2-SHA256$100000${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }
}
