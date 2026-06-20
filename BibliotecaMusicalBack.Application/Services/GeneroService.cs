using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Interfaces;
using BibliotecaMusicalBack.Domain.Entities;
using BibliotecaMusicalBack.Domain.Interfaces;

namespace BibliotecaMusicalBack.Application.Services;

public class GeneroService(IGeneroRepository generoRepository) : IGeneroService
{
    private readonly IGeneroRepository _generoRepository = generoRepository;

    public async Task<List<GeneroDto>> ObtenerGenerosAsync(bool soloActivos, CancellationToken cancellationToken)
    {
        List<Genero> generos = await _generoRepository.ObtenerGenerosAsync(soloActivos, cancellationToken);

        return [.. generos.Select(Mapear)];
    }

    public async Task<GeneroDto?> ObtenerPorCodigoAsync(int codigo, CancellationToken cancellationToken) =>
        MapearOpcional(await _generoRepository.ObtenerPorCodigoAsync(codigo, cancellationToken));

    public async Task<GeneroDto> CrearAsync(GuardarGeneroDto dto, CancellationToken cancellationToken) =>
        Mapear(await _generoRepository.CrearAsync(CrearEntidad(0, dto), cancellationToken))!;

    public async Task<GeneroDto?> ActualizarAsync(int codigo, GuardarGeneroDto dto, CancellationToken cancellationToken) =>
        MapearOpcional(await _generoRepository.ActualizarAsync(CrearEntidad(codigo, dto), cancellationToken));

    public Task<bool> EliminarAsync(int codigo, CancellationToken cancellationToken) =>
        _generoRepository.EliminarAsync(codigo, cancellationToken);

    private static Genero CrearEntidad(int codigo, GuardarGeneroDto dto) => new()
    {
        GenCodigo = codigo,
        GenNombre = dto.Nombre.Trim(),
        GenDescripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
        GenActivo = dto.Activo
    };

    private static GeneroDto Mapear(Genero genero) => new()
    {
        GenCodigo = genero.GenCodigo,
        GenNombre = genero.GenNombre,
        GenDescripcion = genero.GenDescripcion,
        GenActivo = genero.GenActivo
    };

    private static GeneroDto? MapearOpcional(Genero? genero) => genero is null ? null : Mapear(genero);
}
