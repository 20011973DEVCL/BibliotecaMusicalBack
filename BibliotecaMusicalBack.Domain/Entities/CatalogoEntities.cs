namespace BibliotecaMusicalBack.Domain.Entities;

public sealed record ArtistaResumen(int Codigo, string Nombre, string? NombreReal, int? PaisCodigo, string? Pais, bool Activo);
public sealed record AlbumResumen(int Codigo, string Titulo, int ArtistaCodigo, string Artista, int? GeneroCodigo, string? Genero, int? AnioLanzamiento, string? Formato, string? PortadaUrl, bool Activo);
public sealed record CancionResumen(int Codigo, string Titulo, int? AlbumCodigo, string? Album, int? ArtistaCodigo, string? Artista, int? GeneroCodigo, string? Genero, int? DuracionSegundos, int? NumeroPista, bool Activo);
public sealed record PlaylistResumen(int Codigo, string Nombre, string? Descripcion, bool Activo, DateTime FechaCreacion, int TotalCanciones);
public sealed record ColeccionResumen(int Codigo, string Nombre, string? Descripcion, bool Activo, DateTime FechaCreacion, int TotalAlbums);
public sealed record BibliotecaItemResumen(int Codigo, int AlbumCodigo, string Album, string Artista, int? UbicacionCodigo, string? Ubicacion, string? CodigoInterno, string? CodigoBarra, bool EsFisico, string Estado, DateOnly? FechaCompra, decimal? PrecioCompra, string? Observacion, bool Activo);
public sealed record CatalogoItem(int Codigo, string Nombre, bool Activo);

public sealed class Pagina<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required int PaginaActual { get; init; }
    public required int TamanoPagina { get; init; }
    public required int Total { get; init; }
    public int TotalPaginas => (int)Math.Ceiling(Total / (double)TamanoPagina);
}
