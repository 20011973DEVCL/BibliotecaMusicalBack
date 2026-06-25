using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMusicalBack.Domain.Entities;

[Table("usuarios")]
public class Usuario
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("usu_codigo")]
    public int UsuCodigo { get; set; }

    [Column("usu_nombres")]
    public string UsuNombres { get; set; } = string.Empty;

    [Column("usu_apellidos")]
    public string UsuApellidos { get; set; } = string.Empty;

    [Column("usu_email")]
    public string UsuEmail { get; set; } = string.Empty;

    [Column("usu_password_hash")]
    public string UsuPasswordHash { get; set; } = string.Empty;

    [Column("usu_activo")]
    public bool UsuActivo { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("usu_fecha_creacion")]
    public DateTime UsuFechaCreacion { get; set; }
}

[Table("usuarios_roles")]
public class UsuarioRol
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("usu_rol_codigo")]
    public int UsuRolCodigo { get; set; }

    [Column("usu_codigo")]
    public int UsuCodigo { get; set; }

    [Column("rol_codigo")]
    public int RolCodigo { get; set; }
}

[Table("auditoria")]
public class Auditoria
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("aud_codigo")]
    public int AudCodigo { get; set; }

    [Column("usu_codigo")]
    public int? UsuCodigo { get; set; }

    [Column("aud_tabla")]
    public string AudTabla { get; set; } = string.Empty;

    [Column("aud_accion")]
    public string AudAccion { get; set; } = string.Empty;

    [Column("aud_registro_id")]
    public int? AudRegistroId { get; set; }

    [Column("aud_detalle")]
    public string? AudDetalle { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("aud_fecha_evento")]
    public DateTime AudFechaEvento { get; set; }
}
