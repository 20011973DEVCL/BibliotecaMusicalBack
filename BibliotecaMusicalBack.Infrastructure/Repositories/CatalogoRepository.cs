using BibliotecaMusicalBack.Domain.Entities;
using BibliotecaMusicalBack.Domain.Interfaces;
using Npgsql;

namespace BibliotecaMusicalBack.Infrastructure.Repositories;

public sealed class CatalogoRepository(NpgsqlDataSource dataSource) : ICatalogoRepository
{
    public Task<Pagina<ArtistaResumen>> ObtenerArtistasAsync(string? buscar, int? paisCodigo, bool soloActivos, int pagina, int tamano, CancellationToken ct) =>
        ConsultarPaginaAsync("""
            SELECT a.art_codigo, a.art_nombre, a.art_nombre_real, a.pais_codigo, p.pais_nombre, a.art_activo,
                   COUNT(*) OVER()::int AS total
            FROM artistas a LEFT JOIN paises p ON p.pais_codigo=a.pais_codigo
            WHERE (@buscar IS NULL OR a.art_nombre ILIKE '%' || @buscar || '%' OR a.art_nombre_real ILIKE '%' || @buscar || '%')
              AND (@pais IS NULL OR a.pais_codigo=@pais) AND (NOT @activos OR a.art_activo)
            ORDER BY a.art_nombre LIMIT @tamano OFFSET @offset
            """, MapearArtista, buscar, paisCodigo, null, null, soloActivos, pagina, tamano, ct);

    public Task<ArtistaResumen?> ObtenerArtistaAsync(int codigo, CancellationToken ct) => ConsultarUnoAsync("""
        SELECT a.art_codigo, a.art_nombre, a.art_nombre_real, a.pais_codigo, p.pais_nombre, a.art_activo
        FROM artistas a LEFT JOIN paises p ON p.pais_codigo=a.pais_codigo WHERE a.art_codigo=@codigo
        """, codigo, MapearArtista, ct);

    public Task<Pagina<AlbumResumen>> ObtenerAlbumsAsync(string? buscar, int? artistaCodigo, int? generoCodigo, int? anio, bool soloActivos, int pagina, int tamano, CancellationToken ct) =>
        ConsultarPaginaAsync("""
            SELECT al.alb_codigo, al.alb_titulo, al.art_codigo, ar.art_nombre, al.gen_codigo, g.gen_nombre,
                   al.alb_anio_lanzamiento, f.fmt_nombre, al.alb_portada_url, al.alb_activo, COUNT(*) OVER()::int AS total
            FROM albums al JOIN artistas ar ON ar.art_codigo=al.art_codigo
            LEFT JOIN generos g ON g.gen_codigo=al.gen_codigo LEFT JOIN formatos_musicales f ON f.fmt_codigo=al.fmt_codigo
            WHERE (@buscar IS NULL OR al.alb_titulo ILIKE '%' || @buscar || '%' OR ar.art_nombre ILIKE '%' || @buscar || '%')
              AND (@artista IS NULL OR al.art_codigo=@artista) AND (@genero IS NULL OR al.gen_codigo=@genero)
              AND (@anio IS NULL OR al.alb_anio_lanzamiento=@anio) AND (NOT @activos OR al.alb_activo)
            ORDER BY ar.art_nombre, al.alb_anio_lanzamiento, al.alb_titulo LIMIT @tamano OFFSET @offset
            """, MapearAlbum, buscar, artistaCodigo, generoCodigo, anio, soloActivos, pagina, tamano, ct);

    public Task<AlbumResumen?> ObtenerAlbumAsync(int codigo, CancellationToken ct) => ConsultarUnoAsync("""
        SELECT al.alb_codigo, al.alb_titulo, al.art_codigo, ar.art_nombre, al.gen_codigo, g.gen_nombre,
               al.alb_anio_lanzamiento, f.fmt_nombre, al.alb_portada_url, al.alb_activo
        FROM albums al JOIN artistas ar ON ar.art_codigo=al.art_codigo LEFT JOIN generos g ON g.gen_codigo=al.gen_codigo
        LEFT JOIN formatos_musicales f ON f.fmt_codigo=al.fmt_codigo WHERE al.alb_codigo=@codigo
        """, codigo, MapearAlbum, ct);

    public Task<Pagina<CancionResumen>> ObtenerCancionesAsync(string? buscar, int? albumCodigo, int? artistaCodigo, int? generoCodigo, bool soloActivos, int pagina, int tamano, CancellationToken ct) =>
        ConsultarPaginaAsync("""
            SELECT c.can_codigo, c.can_titulo, c.alb_codigo, al.alb_titulo, ar.art_codigo, ar.art_nombre,
                   c.gen_codigo, g.gen_nombre, c.can_duracion_segundos, c.can_numero_pista, c.can_activo, COUNT(*) OVER()::int AS total
            FROM canciones c LEFT JOIN albums al ON al.alb_codigo=c.alb_codigo LEFT JOIN artistas ar ON ar.art_codigo=al.art_codigo
            LEFT JOIN generos g ON g.gen_codigo=c.gen_codigo
            WHERE (@buscar IS NULL OR c.can_titulo ILIKE '%' || @buscar || '%' OR ar.art_nombre ILIKE '%' || @buscar || '%')
              AND (@album IS NULL OR c.alb_codigo=@album) AND (@artista IS NULL OR al.art_codigo=@artista)
              AND (@genero IS NULL OR c.gen_codigo=@genero) AND (NOT @activos OR c.can_activo)
            ORDER BY ar.art_nombre, al.alb_titulo, c.can_numero_pista, c.can_titulo LIMIT @tamano OFFSET @offset
            """, MapearCancion, buscar, artistaCodigo, generoCodigo, null, soloActivos, pagina, tamano, ct, albumCodigo);

    public Task<CancionResumen?> ObtenerCancionAsync(int codigo, CancellationToken ct) => ConsultarUnoAsync("""
        SELECT c.can_codigo, c.can_titulo, c.alb_codigo, al.alb_titulo, ar.art_codigo, ar.art_nombre,
               c.gen_codigo, g.gen_nombre, c.can_duracion_segundos, c.can_numero_pista, c.can_activo
        FROM canciones c LEFT JOIN albums al ON al.alb_codigo=c.alb_codigo LEFT JOIN artistas ar ON ar.art_codigo=al.art_codigo
        LEFT JOIN generos g ON g.gen_codigo=c.gen_codigo WHERE c.can_codigo=@codigo
        """, codigo, MapearCancion, ct);

    public Task<IReadOnlyList<PlaylistResumen>> ObtenerPlaylistsAsync(bool soloActivos, CancellationToken ct) => ConsultarListaAsync("""
        SELECT p.play_codigo,p.play_nombre,p.play_descripcion,p.play_activo,p.play_fecha_creacion,COUNT(pc.can_codigo)::int
        FROM playlists p LEFT JOIN playlist_canciones pc ON pc.play_codigo=p.play_codigo
        WHERE (NOT @activos OR p.play_activo) GROUP BY p.play_codigo ORDER BY p.play_nombre
        """, cmd => cmd.Parameters.AddWithValue("activos", soloActivos), r => new PlaylistResumen(r.GetInt32(0),r.GetString(1),Texto(r,2),r.GetBoolean(3),r.GetDateTime(4),r.GetInt32(5)), ct);

    public async Task<PlaylistResumen?> ObtenerPlaylistAsync(int codigo, CancellationToken ct) =>
        (await ConsultarListaAsync("""
            SELECT p.play_codigo,p.play_nombre,p.play_descripcion,p.play_activo,p.play_fecha_creacion,COUNT(pc.can_codigo)::int
            FROM playlists p LEFT JOIN playlist_canciones pc ON pc.play_codigo=p.play_codigo WHERE p.play_codigo=@codigo GROUP BY p.play_codigo
            """, cmd => cmd.Parameters.AddWithValue("codigo", codigo), r => new PlaylistResumen(r.GetInt32(0),r.GetString(1),Texto(r,2),r.GetBoolean(3),r.GetDateTime(4),r.GetInt32(5)), ct)).SingleOrDefault();

    public Task<IReadOnlyList<CancionResumen>> ObtenerCancionesPlaylistAsync(int codigo, CancellationToken ct) => ConsultarListaAsync("""
        SELECT c.can_codigo,c.can_titulo,c.alb_codigo,al.alb_titulo,ar.art_codigo,ar.art_nombre,c.gen_codigo,g.gen_nombre,c.can_duracion_segundos,c.can_numero_pista,c.can_activo
        FROM playlist_canciones pc JOIN canciones c ON c.can_codigo=pc.can_codigo LEFT JOIN albums al ON al.alb_codigo=c.alb_codigo
        LEFT JOIN artistas ar ON ar.art_codigo=al.art_codigo LEFT JOIN generos g ON g.gen_codigo=c.gen_codigo
        WHERE pc.play_codigo=@codigo ORDER BY pc.play_can_orden NULLS LAST,pc.play_can_fecha_agregado
        """, cmd => cmd.Parameters.AddWithValue("codigo", codigo), MapearCancion, ct);

    public async Task<PlaylistResumen> CrearPlaylistAsync(string nombre, string? descripcion, CancellationToken ct)
    {
        await using NpgsqlCommand cmd=dataSource.CreateCommand("INSERT INTO playlists(play_nombre,play_descripcion) VALUES(@nombre,@descripcion) RETURNING play_codigo,play_nombre,play_descripcion,play_activo,play_fecha_creacion");
        cmd.Parameters.AddWithValue("nombre",nombre); cmd.Parameters.AddWithValue("descripcion",(object?)descripcion??DBNull.Value);
        await using NpgsqlDataReader r=await cmd.ExecuteReaderAsync(ct); await r.ReadAsync(ct);
        return new(r.GetInt32(0),r.GetString(1),Texto(r,2),r.GetBoolean(3),r.GetDateTime(4),0);
    }

    public async Task<bool> AgregarCancionPlaylistAsync(int playlistCodigo,int cancionCodigo,int? orden,CancellationToken ct)
    {
        await using NpgsqlCommand cmd=dataSource.CreateCommand("INSERT INTO playlist_canciones(play_codigo,can_codigo,play_can_orden) SELECT @playlist,@cancion,@orden WHERE EXISTS(SELECT 1 FROM playlists WHERE play_codigo=@playlist AND play_activo) AND EXISTS(SELECT 1 FROM canciones WHERE can_codigo=@cancion AND can_activo) ON CONFLICT(play_codigo,can_codigo) DO UPDATE SET play_can_orden=EXCLUDED.play_can_orden");
        cmd.Parameters.AddWithValue("playlist",playlistCodigo); cmd.Parameters.AddWithValue("cancion",cancionCodigo); cmd.Parameters.AddWithValue("orden",(object?)orden??DBNull.Value);
        return await cmd.ExecuteNonQueryAsync(ct)>0;
    }

    public Task<bool> QuitarCancionPlaylistAsync(int playlistCodigo,int cancionCodigo,CancellationToken ct) => EjecutarAsync("DELETE FROM playlist_canciones WHERE play_codigo=@codigo AND can_codigo=@relacion",playlistCodigo,cancionCodigo,ct);
    public Task<bool> EliminarPlaylistAsync(int codigo,CancellationToken ct) => EjecutarAsync("UPDATE playlists SET play_activo=FALSE WHERE play_codigo=@codigo AND play_activo",codigo,null,ct);

    public Task<IReadOnlyList<ColeccionResumen>> ObtenerColeccionesAsync(bool soloActivos,CancellationToken ct) => ConsultarListaAsync("""
        SELECT c.col_codigo,c.col_nombre,c.col_descripcion,c.col_activo,c.col_fecha_creacion,COUNT(ca.alb_codigo)::int
        FROM colecciones c LEFT JOIN coleccion_albums ca ON ca.col_codigo=c.col_codigo WHERE (NOT @activos OR c.col_activo)
        GROUP BY c.col_codigo ORDER BY c.col_nombre
        """,cmd=>cmd.Parameters.AddWithValue("activos",soloActivos),r=>new ColeccionResumen(r.GetInt32(0),r.GetString(1),Texto(r,2),r.GetBoolean(3),r.GetDateTime(4),r.GetInt32(5)),ct);

    public async Task<ColeccionResumen?> ObtenerColeccionAsync(int codigo,CancellationToken ct) => (await ConsultarListaAsync("""
        SELECT c.col_codigo,c.col_nombre,c.col_descripcion,c.col_activo,c.col_fecha_creacion,COUNT(ca.alb_codigo)::int
        FROM colecciones c LEFT JOIN coleccion_albums ca ON ca.col_codigo=c.col_codigo WHERE c.col_codigo=@codigo GROUP BY c.col_codigo
        """,cmd=>cmd.Parameters.AddWithValue("codigo",codigo),r=>new ColeccionResumen(r.GetInt32(0),r.GetString(1),Texto(r,2),r.GetBoolean(3),r.GetDateTime(4),r.GetInt32(5)),ct)).SingleOrDefault();

    public Task<IReadOnlyList<AlbumResumen>> ObtenerAlbumsColeccionAsync(int codigo,CancellationToken ct) => ConsultarListaAsync("""
        SELECT al.alb_codigo,al.alb_titulo,al.art_codigo,ar.art_nombre,al.gen_codigo,g.gen_nombre,al.alb_anio_lanzamiento,f.fmt_nombre,al.alb_portada_url,al.alb_activo
        FROM coleccion_albums ca JOIN albums al ON al.alb_codigo=ca.alb_codigo JOIN artistas ar ON ar.art_codigo=al.art_codigo
        LEFT JOIN generos g ON g.gen_codigo=al.gen_codigo LEFT JOIN formatos_musicales f ON f.fmt_codigo=al.fmt_codigo WHERE ca.col_codigo=@codigo ORDER BY ar.art_nombre,al.alb_titulo
        """,cmd=>cmd.Parameters.AddWithValue("codigo",codigo),MapearAlbum,ct);

    public Task<IReadOnlyList<BibliotecaItemResumen>> ObtenerItemsAsync(string? estado,bool? esFisico,bool soloActivos,CancellationToken ct) => ConsultarListaAsync("""
        SELECT i.item_codigo,i.alb_codigo,al.alb_titulo,ar.art_nombre,i.ubi_codigo,u.ubi_nombre,i.item_codigo_interno,i.item_codigo_barra,
               i.item_es_fisico,i.item_estado,i.item_fecha_compra,i.item_precio_compra,i.item_observacion,i.item_activo
        FROM biblioteca_items i JOIN albums al ON al.alb_codigo=i.alb_codigo JOIN artistas ar ON ar.art_codigo=al.art_codigo LEFT JOIN ubicaciones_fisicas u ON u.ubi_codigo=i.ubi_codigo
        WHERE (@estado IS NULL OR i.item_estado ILIKE @estado) AND (@fisico IS NULL OR i.item_es_fisico=@fisico) AND (NOT @activos OR i.item_activo) ORDER BY ar.art_nombre,al.alb_titulo
        """,cmd=>{cmd.Parameters.AddWithValue("estado",(object?)estado??DBNull.Value);cmd.Parameters.AddWithValue("fisico",(object?)esFisico??DBNull.Value);cmd.Parameters.AddWithValue("activos",soloActivos);},
        r=>new BibliotecaItemResumen(r.GetInt32(0),r.GetInt32(1),r.GetString(2),r.GetString(3),Entero(r,4),Texto(r,5),Texto(r,6),Texto(r,7),r.GetBoolean(8),r.GetString(9),r.IsDBNull(10)?null:DateOnly.FromDateTime(r.GetDateTime(10)),r.IsDBNull(11)?null:r.GetDecimal(11),Texto(r,12),r.GetBoolean(13)),ct);

    public Task<IReadOnlyList<CatalogoItem>> ObtenerCatalogoAsync(string catalogo,bool soloActivos,CancellationToken ct)
    {
        (string tabla,string codigo,string nombre,string activo)=catalogo switch
        {
            "paises"=>("paises","pais_codigo","pais_nombre","pais_activo"), "formatos"=>("formatos_musicales","fmt_codigo","fmt_nombre","fmt_activo"),
            "sellos"=>("sellos_discograficos","sello_codigo","sello_nombre","sello_activo"), "tipos-artista"=>("tipos_artista","tart_codigo","tart_nombre","TRUE"),
            "ubicaciones"=>("ubicaciones_fisicas","ubi_codigo","ubi_nombre","ubi_activo"), _=>throw new ArgumentOutOfRangeException(nameof(catalogo))
        };
        return ConsultarListaAsync($"SELECT {codigo},{nombre},{activo} FROM {tabla} WHERE (NOT @activos OR {activo}) ORDER BY {nombre}",cmd=>cmd.Parameters.AddWithValue("activos",soloActivos),r=>new CatalogoItem(r.GetInt32(0),r.GetString(1),r.GetBoolean(2)),ct);
    }

    private async Task<Pagina<T>> ConsultarPaginaAsync<T>(string sql,Func<NpgsqlDataReader,T> map,string? buscar,int? artista,int? genero,int? anio,bool activos,int pagina,int tamano,CancellationToken ct,int? album=null)
    {
        await using NpgsqlCommand cmd=dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("buscar",(object?)buscar??DBNull.Value);cmd.Parameters.AddWithValue("pais",(object?)artista??DBNull.Value);cmd.Parameters.AddWithValue("artista",(object?)artista??DBNull.Value);
        cmd.Parameters.AddWithValue("genero",(object?)genero??DBNull.Value);cmd.Parameters.AddWithValue("anio",(object?)anio??DBNull.Value);cmd.Parameters.AddWithValue("album",(object?)album??DBNull.Value);
        cmd.Parameters.AddWithValue("activos",activos);cmd.Parameters.AddWithValue("tamano",tamano);cmd.Parameters.AddWithValue("offset",(pagina-1)*tamano);
        await using NpgsqlDataReader r=await cmd.ExecuteReaderAsync(ct);List<T> items=[];int total=0;while(await r.ReadAsync(ct)){items.Add(map(r));total=r.GetInt32(r.FieldCount-1);}return new(){Items=items,PaginaActual=pagina,TamanoPagina=tamano,Total=total};
    }
    private async Task<T?> ConsultarUnoAsync<T>(string sql,int codigo,Func<NpgsqlDataReader,T> map,CancellationToken ct){await using NpgsqlCommand cmd=dataSource.CreateCommand(sql);cmd.Parameters.AddWithValue("codigo",codigo);await using NpgsqlDataReader r=await cmd.ExecuteReaderAsync(ct);return await r.ReadAsync(ct)?map(r):default;}
    private async Task<IReadOnlyList<T>> ConsultarListaAsync<T>(string sql,Action<NpgsqlCommand> parametros,Func<NpgsqlDataReader,T> map,CancellationToken ct){await using NpgsqlCommand cmd=dataSource.CreateCommand(sql);parametros(cmd);await using NpgsqlDataReader r=await cmd.ExecuteReaderAsync(ct);List<T> items=[];while(await r.ReadAsync(ct))items.Add(map(r));return items;}
    private async Task<bool> EjecutarAsync(string sql,int codigo,int? relacion,CancellationToken ct){await using NpgsqlCommand cmd=dataSource.CreateCommand(sql);cmd.Parameters.AddWithValue("codigo",codigo);if(relacion.HasValue)cmd.Parameters.AddWithValue("relacion",relacion.Value);return await cmd.ExecuteNonQueryAsync(ct)>0;}
    private static ArtistaResumen MapearArtista(NpgsqlDataReader r)=>new(r.GetInt32(0),r.GetString(1),Texto(r,2),Entero(r,3),Texto(r,4),r.GetBoolean(5));
    private static AlbumResumen MapearAlbum(NpgsqlDataReader r)=>new(r.GetInt32(0),r.GetString(1),r.GetInt32(2),r.GetString(3),Entero(r,4),Texto(r,5),Entero(r,6),Texto(r,7),Texto(r,8),r.GetBoolean(9));
    private static CancionResumen MapearCancion(NpgsqlDataReader r)=>new(r.GetInt32(0),r.GetString(1),Entero(r,2),Texto(r,3),Entero(r,4),Texto(r,5),Entero(r,6),Texto(r,7),Entero(r,8),Entero(r,9),r.GetBoolean(10));
    private static string? Texto(NpgsqlDataReader r,int i)=>r.IsDBNull(i)?null:r.GetString(i); private static int? Entero(NpgsqlDataReader r,int i)=>r.IsDBNull(i)?null:r.GetInt32(i);
}
