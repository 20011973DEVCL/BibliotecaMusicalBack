using BibliotecaMusicalBack.Application.DTOs;

namespace BibliotecaMusicalBack.Application.Interfaces;

public interface IGeneroService
{
    Task<List<GeneroDto>> ObtenerGenerosAsync();
}