using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMusicalBack.Domain.Entities;

[Table("colecciones")]
public class Coleccion
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("col_codigo")]
    public int ColCodigo { get; set; }

    [Column("col_nombre")]
    public string ColNombre { get; set; } = string.Empty;

    [Column("col_descripcion")]
    public string? ColDescripcion { get; set; }

    [Column("col_activo")]
    public bool ColActivo { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("col_fecha_creacion")]
    public DateTime ColFechaCreacion { get; set; }
}

[Table("coleccion_albums")]
public class ColeccionAlbum
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("col_alb_codigo")]
    public int ColAlbCodigo { get; set; }

    [Column("col_codigo")]
    public int ColCodigo { get; set; }

    [Column("alb_codigo")]
    public int AlbCodigo { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("col_alb_fecha_agregado")]
    public DateTime ColAlbFechaAgregado { get; set; }
}

[Table("biblioteca_items")]
public class BibliotecaItem
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("item_codigo")]
    public int ItemCodigo { get; set; }

    [Column("alb_codigo")]
    public int AlbCodigo { get; set; }

    [Column("ubi_codigo")]
    public int? UbiCodigo { get; set; }

    [Column("item_codigo_interno")]
    public string? ItemCodigoInterno { get; set; }

    [Column("item_codigo_barra")]
    public string? ItemCodigoBarra { get; set; }

    [Column("item_es_fisico")]
    public bool ItemEsFisico { get; set; }

    [Column("item_estado")]
    public string ItemEstado { get; set; } = string.Empty;

    [Column("item_fecha_compra")]
    public DateOnly? ItemFechaCompra { get; set; }

    [Column("item_precio_compra")]
    public decimal? ItemPrecioCompra { get; set; }

    [Column("item_observacion")]
    public string? ItemObservacion { get; set; }

    [Column("item_activo")]
    public bool ItemActivo { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("item_fecha_creacion")]
    public DateTime ItemFechaCreacion { get; set; }
}

[Table("playlists")]
public class Playlist
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("play_codigo")]
    public int PlayCodigo { get; set; }

    [Column("play_nombre")]
    public string PlayNombre { get; set; } = string.Empty;

    [Column("play_descripcion")]
    public string? PlayDescripcion { get; set; }

    [Column("play_activo")]
    public bool PlayActivo { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("play_fecha_creacion")]
    public DateTime PlayFechaCreacion { get; set; }
}

[Table("playlist_canciones")]
public class PlaylistCancion
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("play_can_codigo")]
    public int PlayCanCodigo { get; set; }

    [Column("play_codigo")]
    public int PlayCodigo { get; set; }

    [Column("can_codigo")]
    public int CanCodigo { get; set; }

    [Column("play_can_orden")]
    public int? PlayCanOrden { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("play_can_fecha_agregado")]
    public DateTime PlayCanFechaAgregado { get; set; }
}
