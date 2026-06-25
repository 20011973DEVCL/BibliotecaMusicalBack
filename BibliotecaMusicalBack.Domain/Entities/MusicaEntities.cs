using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMusicalBack.Domain.Entities;

[Table("artistas")]
public class Artista
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("art_codigo")]
    public int ArtCodigo { get; set; }

    [Column("pais_codigo")]
    public int? PaisCodigo { get; set; }

    [Column("art_nombre")]
    public string ArtNombre { get; set; } = string.Empty;

    [Column("art_nombre_real")]
    public string? ArtNombreReal { get; set; }

    [Column("art_fecha_nacimiento")]
    public DateOnly? ArtFechaNacimiento { get; set; }

    [Column("art_fecha_fallecimiento")]
    public DateOnly? ArtFechaFallecimiento { get; set; }

    [Column("art_biografia")]
    public string? ArtBiografia { get; set; }

    [Column("art_activo")]
    public bool ArtActivo { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("art_fecha_creacion")]
    public DateTime ArtFechaCreacion { get; set; }
}

[Table("artistas_tipos")]
public class ArtistaTipo
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("art_tipo_codigo")]
    public int ArtTipoCodigo { get; set; }

    [Column("art_codigo")]
    public int ArtCodigo { get; set; }

    [Column("tart_codigo")]
    public int TartCodigo { get; set; }
}

[Table("albums")]
public class Album
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("alb_codigo")]
    public int AlbCodigo { get; set; }

    [Column("art_codigo")]
    public int ArtCodigo { get; set; }

    [Column("gen_codigo")]
    public int? GenCodigo { get; set; }

    [Column("sello_codigo")]
    public int? SelloCodigo { get; set; }

    [Column("fmt_codigo")]
    public int? FmtCodigo { get; set; }

    [Column("alb_titulo")]
    public string AlbTitulo { get; set; } = string.Empty;

    [Column("alb_anio_lanzamiento")]
    public int? AlbAnioLanzamiento { get; set; }

    [Column("alb_fecha_lanzamiento")]
    public DateOnly? AlbFechaLanzamiento { get; set; }

    [Column("alb_numero_discos")]
    public int AlbNumeroDiscos { get; set; }

    [Column("alb_portada_url")]
    public string? AlbPortadaUrl { get; set; }

    [Column("alb_observacion")]
    public string? AlbObservacion { get; set; }

    [Column("alb_activo")]
    public bool AlbActivo { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("alb_fecha_creacion")]
    public DateTime AlbFechaCreacion { get; set; }
}

[Table("canciones")]
public class Cancion
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("can_codigo")]
    public int CanCodigo { get; set; }

    [Column("alb_codigo")]
    public int? AlbCodigo { get; set; }

    [Column("gen_codigo")]
    public int? GenCodigo { get; set; }

    [Column("can_titulo")]
    public string CanTitulo { get; set; } = string.Empty;

    [Column("can_duracion_segundos")]
    public int? CanDuracionSegundos { get; set; }

    [Column("can_numero_pista")]
    public int? CanNumeroPista { get; set; }

    [Column("can_letra")]
    public string? CanLetra { get; set; }

    [Column("can_activo")]
    public bool CanActivo { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("can_fecha_creacion")]
    public DateTime CanFechaCreacion { get; set; }
}

[Table("canciones_artistas")]
public class CancionArtista
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("can_art_codigo")]
    public int CanArtCodigo { get; set; }

    [Column("can_codigo")]
    public int CanCodigo { get; set; }

    [Column("art_codigo")]
    public int ArtCodigo { get; set; }

    [Column("can_art_es_principal")]
    public bool CanArtEsPrincipal { get; set; }
}

[Table("compositores")]
public class Compositor
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("comp_codigo")]
    public int CompCodigo { get; set; }

    [Column("comp_nombres")]
    public string CompNombres { get; set; } = string.Empty;

    [Column("comp_apellidos")]
    public string? CompApellidos { get; set; }

    [Column("pais_codigo")]
    public int? PaisCodigo { get; set; }

    [Column("comp_activo")]
    public bool CompActivo { get; set; }
}

[Table("canciones_compositores")]
public class CancionCompositor
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("can_comp_codigo")]
    public int CanCompCodigo { get; set; }

    [Column("can_codigo")]
    public int CanCodigo { get; set; }

    [Column("comp_codigo")]
    public int CompCodigo { get; set; }
}
