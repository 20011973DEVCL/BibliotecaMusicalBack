using BibliotecaMusicalBack.Application.DTOs;

namespace BibliotecaMusicalBack.Application.Interfaces;

public interface IUsuarioService
{
    Task<IReadOnlyList<UsuarioDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UsuarioDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UsuarioDto> CreateAsync(UsuarioRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UsuarioRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
