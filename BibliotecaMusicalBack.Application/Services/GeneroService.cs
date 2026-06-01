using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;
using BibliotecaMusicalBack.Domain.Interfaces;

namespace BibliotecaMusicalBack.Application.Services;

public class GeneroService : IGeneroService
{
    private readonly IGeneroRepository _generoRepository;

    public GeneroService(IGeneroRepository generoRepository)
    {
        _generoRepository = generoRepository;
    }

    public async Task<List<GeneroDto>> ObtenerGenerosAsync()
    {
        var generos = await _generoRepository.ObtenerGenerosAsync();

        return generos.Select(x => new GeneroDto
        {
            GenCodigo = x.GenCodigo,
            GenNombre = x.GenNombre
        }).ToList();
    }
}