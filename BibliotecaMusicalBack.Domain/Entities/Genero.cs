using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMusicalBack.Domain.Entities;

[Table("generos")]
public class Genero
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("gen_codigo")]
    public int GenCodigo { get; set; }

    [Column("gen_nombre")]
    public string GenNombre { get; set; } = string.Empty;

    [Column("gen_descripcion")]
    public string? GenDescripcion { get; set; }

    [Column("gen_activo")]
    public bool GenActivo { get; set; }
}
