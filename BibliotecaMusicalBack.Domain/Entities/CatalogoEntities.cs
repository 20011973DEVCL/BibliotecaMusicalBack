using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMusicalBack.Domain.Entities;

[Table("paises")]
public class Pais
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("pais_codigo")]
    public int PaisCodigo { get; set; }

    [Column("pais_nombre")]
    public string PaisNombre { get; set; } = string.Empty;

    [Column("pais_codigo_iso")]
    public string? PaisCodigoIso { get; set; }

    [Column("pais_activo")]
    public bool PaisActivo { get; set; }
}

[Table("tipos_artista")]
public class TipoArtista
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("tart_codigo")]
    public int TartCodigo { get; set; }

    [Column("tart_nombre")]
    public string TartNombre { get; set; } = string.Empty;
}

[Table("sellos_discograficos")]
public class SelloDiscografico
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("sello_codigo")]
    public int SelloCodigo { get; set; }

    [Column("sello_nombre")]
    public string SelloNombre { get; set; } = string.Empty;

    [Column("pais_codigo")]
    public int? PaisCodigo { get; set; }

    [Column("sello_activo")]
    public bool SelloActivo { get; set; }
}

[Table("formatos_musicales")]
public class FormatoMusical
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("fmt_codigo")]
    public int FmtCodigo { get; set; }

    [Column("fmt_nombre")]
    public string FmtNombre { get; set; } = string.Empty;

    [Column("fmt_descripcion")]
    public string? FmtDescripcion { get; set; }

    [Column("fmt_activo")]
    public bool FmtActivo { get; set; }
}

[Table("ubicaciones_fisicas")]
public class UbicacionFisica
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ubi_codigo")]
    public int UbiCodigo { get; set; }

    [Column("ubi_nombre")]
    public string UbiNombre { get; set; } = string.Empty;

    [Column("ubi_descripcion")]
    public string? UbiDescripcion { get; set; }

    [Column("ubi_activo")]
    public bool UbiActivo { get; set; }
}

[Table("roles")]
public class Rol
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("rol_codigo")]
    public int RolCodigo { get; set; }

    [Column("rol_nombre")]
    public string RolNombre { get; set; } = string.Empty;

    [Column("rol_activo")]
    public bool RolActivo { get; set; }
}
