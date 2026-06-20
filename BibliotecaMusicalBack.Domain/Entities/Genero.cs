namespace BibliotecaMusicalBack.Domain.Entities;

public class Genero
{
    public int GenCodigo { get; set; }
    public string GenNombre { get; set; } = string.Empty;
    public string? GenDescripcion { get; set; }
    public bool GenActivo { get; set; }
}
