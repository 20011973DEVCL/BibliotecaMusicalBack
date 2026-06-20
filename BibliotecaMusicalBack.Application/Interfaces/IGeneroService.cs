using BibliotecaMusicalBack.Application.DTOs;

namespace BibliotecaMusicalBack.Application.Interfaces;

public interface IGeneroService
{
    Task<List<GeneroDto>> ObtenerGenerosAsync(bool soloActivos, CancellationToken cancellationToken);
    Task<GeneroDto?> ObtenerPorCodigoAsync(int codigo, CancellationToken cancellationToken);
    Task<GeneroDto> CrearAsync(GuardarGeneroDto dto, CancellationToken cancellationToken);
    Task<GeneroDto?> ActualizarAsync(int codigo, GuardarGeneroDto dto, CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int codigo, CancellationToken cancellationToken);
}
