using BibliotecaMusicalBack.Domain.Entities;

namespace BibliotecaMusicalBack.Domain.Interfaces;

public interface IGeneroRepository
{
    Task<List<Genero>> ObtenerGenerosAsync(bool soloActivos, CancellationToken cancellationToken);
    Task<Genero?> ObtenerPorCodigoAsync(int codigo, CancellationToken cancellationToken);
    Task<Genero> CrearAsync(Genero genero, CancellationToken cancellationToken);
    Task<Genero?> ActualizarAsync(Genero genero, CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int codigo, CancellationToken cancellationToken);
}
