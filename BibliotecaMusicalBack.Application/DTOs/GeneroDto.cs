namespace BibliotecaMusicalBack.Application.DTOs;

public class GeneroDto
{
    public int GenCodigo { get; set; }
    public string GenNombre { get; set; } = string.Empty;
    public string? GenDescripcion { get; set; }
    public bool GenActivo { get; set; }
}

public sealed record GuardarGeneroDto(string Nombre, string? Descripcion, bool Activo = true);
