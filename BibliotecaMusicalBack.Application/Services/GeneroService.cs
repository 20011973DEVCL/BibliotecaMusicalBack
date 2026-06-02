using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;
using BibliotecaMusicalBack.Domain.Entities;
using BibliotecaMusicalBack.Domain.Interfaces;

namespace BibliotecaMusicalBack.Application.Services;

public class GeneroService(IGeneroRepository generoRepository) : IGeneroService
{
    private readonly IGeneroRepository _generoRepository = generoRepository;

    public async Task<List<GeneroDto>> ObtenerGenerosAsync()
    {
        List<Genero> generos = await _generoRepository.ObtenerGenerosAsync();

        return [.. generos.Select(x => new GeneroDto
        {
            GenCodigo = x.GenCodigo,
            GenNombre = x.GenNombre
        })];
    }
}