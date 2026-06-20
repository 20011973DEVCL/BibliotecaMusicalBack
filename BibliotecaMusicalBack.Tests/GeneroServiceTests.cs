using BibliotecaMusicalBack.Application.DTOs;
using BibliotecaMusicalBack.Application.Services;
using BibliotecaMusicalBack.Domain.Entities;
using BibliotecaMusicalBack.Domain.Interfaces;
using Shouldly;

namespace BibliotecaMusicalBack.Tests;

public sealed class GeneroServiceTests
{
    [Fact]
    public async Task ObtenerGeneros_mapea_todos_los_campos()
    {
        FakeGeneroRepository repository = new([new() { GenCodigo=1, GenNombre="Rock", GenDescripcion="Guitarras", GenActivo=true }]);
        GeneroService service = new(repository);

        List<GeneroDto> resultado = await service.ObtenerGenerosAsync(true, CancellationToken.None);

        resultado.Count.ShouldBe(1);
        resultado[0].GenNombre.ShouldBe("Rock");
        resultado[0].GenDescripcion.ShouldBe("Guitarras");
        resultado[0].GenActivo.ShouldBeTrue();
        repository.SoloActivos.ShouldBeTrue();
    }

    [Fact]
    public async Task Crear_limpia_nombre_y_descripcion()
    {
        FakeGeneroRepository repository = new([]);
        GeneroService service = new(repository);

        GeneroDto resultado = await service.CrearAsync(new GuardarGeneroDto("  Jazz  ", "  Improvisación  "), CancellationToken.None);

        resultado.GenNombre.ShouldBe("Jazz");
        resultado.GenDescripcion.ShouldBe("Improvisación");
    }

    [Fact]
    public async Task Obtener_inexistente_retorna_null()
    {
        GeneroService service = new(new FakeGeneroRepository([]));
        (await service.ObtenerPorCodigoAsync(99, CancellationToken.None)).ShouldBeNull();
    }

    private sealed class FakeGeneroRepository(List<Genero> items) : IGeneroRepository
    {
        public bool SoloActivos { get; private set; }
        public Task<List<Genero>> ObtenerGenerosAsync(bool soloActivos,CancellationToken ct){SoloActivos=soloActivos;return Task.FromResult(items);}
        public Task<Genero?> ObtenerPorCodigoAsync(int codigo,CancellationToken ct)=>Task.FromResult(items.SingleOrDefault(x=>x.GenCodigo==codigo));
        public Task<Genero> CrearAsync(Genero genero,CancellationToken ct){genero.GenCodigo=items.Count+1;items.Add(genero);return Task.FromResult(genero);}
        public Task<Genero?> ActualizarAsync(Genero genero,CancellationToken ct)=>Task.FromResult<Genero?>(genero);
        public Task<bool> EliminarAsync(int codigo,CancellationToken ct)=>Task.FromResult(true);
    }
}
