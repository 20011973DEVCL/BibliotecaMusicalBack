using BibliotecaMusicalBack.Domain.Entities;

namespace BibliotecaMusicalBack.Domain.Interfaces;

public interface IGeneroRepository
{
    Task<List<Genero>> ObtenerGenerosAsync();
}