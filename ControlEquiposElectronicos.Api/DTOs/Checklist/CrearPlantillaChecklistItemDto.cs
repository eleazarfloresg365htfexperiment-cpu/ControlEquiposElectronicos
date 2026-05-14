using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Checklist;

public class CrearPlantillaChecklistItemDto
{
    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    public int Orden { get; set; }

    public bool EsObligatorio { get; set; } = true;
}