using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Checklist;

public class ActualizarPlantillaChecklistDto
{
    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    public int? CategoriaEquipoId { get; set; }

    [Required]
    public int TipoEquipoId { get; set; }

    public bool Activo { get; set; } = true;
}