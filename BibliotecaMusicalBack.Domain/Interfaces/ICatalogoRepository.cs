using BibliotecaMusicalBack.Domain.Entities;

namespace BibliotecaMusicalBack.Domain.Interfaces;

public interface ICatalogoRepository
{
    Task<Pagina<ArtistaResumen>> ObtenerArtistasAsync(string? buscar, int? paisCodigo, bool soloActivos, int pagina, int tamano, CancellationToken cancellationToken);
    Task<ArtistaResumen?> ObtenerArtistaAsync(int codigo, CancellationToken cancellationToken);
    Task<Pagina<AlbumResumen>> ObtenerAlbumsAsync(string? buscar, int? artistaCodigo, int? generoCodigo, int? anio, bool soloActivos, int pagina, int tamano, CancellationToken cancellationToken);
    Task<AlbumResumen?> ObtenerAlbumAsync(int codigo, CancellationToken cancellationToken);
    Task<Pagina<CancionResumen>> ObtenerCancionesAsync(string? buscar, int? albumCodigo, int? artistaCodigo, int? generoCodigo, bool soloActivos, int pagina, int tamano, CancellationToken cancellationToken);
    Task<CancionResumen?> ObtenerCancionAsync(int codigo, CancellationToken cancellationToken);
    Task<IReadOnlyList<PlaylistResumen>> ObtenerPlaylistsAsync(bool soloActivos, CancellationToken cancellationToken);
    Task<PlaylistResumen?> ObtenerPlaylistAsync(int codigo, CancellationToken cancellationToken);
    Task<IReadOnlyList<CancionResumen>> ObtenerCancionesPlaylistAsync(int codigo, CancellationToken cancellationToken);
    Task<PlaylistResumen> CrearPlaylistAsync(string nombre, string? descripcion, CancellationToken cancellationToken);
    Task<bool> AgregarCancionPlaylistAsync(int playlistCodigo, int cancionCodigo, int? orden, CancellationToken cancellationToken);
    Task<bool> QuitarCancionPlaylistAsync(int playlistCodigo, int cancionCodigo, CancellationToken cancellationToken);
    Task<bool> EliminarPlaylistAsync(int codigo, CancellationToken cancellationToken);
    Task<IReadOnlyList<ColeccionResumen>> ObtenerColeccionesAsync(bool soloActivos, CancellationToken cancellationToken);
    Task<ColeccionResumen?> ObtenerColeccionAsync(int codigo, CancellationToken cancellationToken);
    Task<IReadOnlyList<AlbumResumen>> ObtenerAlbumsColeccionAsync(int codigo, CancellationToken cancellationToken);
    Task<IReadOnlyList<BibliotecaItemResumen>> ObtenerItemsAsync(string? estado, bool? esFisico, bool soloActivos, CancellationToken cancellationToken);
    Task<IReadOnlyList<CatalogoItem>> ObtenerCatalogoAsync(string catalogo, bool soloActivos, CancellationToken cancellationToken);
}
