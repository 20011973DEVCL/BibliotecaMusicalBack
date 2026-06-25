using BibliotecaMusicalBack.Domain.Entities;

namespace BibliotecaMusicalBack.Application.DTOs;

public class UsuarioRolDto : UsuarioRol { }
public class AuditoriaDto : Auditoria { }

public class UsuarioDto
{
    public int UsuCodigo { get; set; }
    public string UsuNombres { get; set; } = string.Empty;
    public string UsuApellidos { get; set; } = string.Empty;
    public string UsuEmail { get; set; } = string.Empty;
    public bool UsuActivo { get; set; }
    public DateTime UsuFechaCreacion { get; set; }
}

public class UsuarioRequest
{
    public string UsuNombres { get; set; } = string.Empty;
    public string UsuApellidos { get; set; } = string.Empty;
    public string UsuEmail { get; set; } = string.Empty;
    public string? Password { get; set; }
    public bool UsuActivo { get; set; } = true;
}
