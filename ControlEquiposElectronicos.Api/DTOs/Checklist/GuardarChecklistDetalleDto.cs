using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Checklist;

public class GuardarChecklistDetalleDto
{
    [Required]
    public int ChecklistTecnicoDetalleId { get; set; }

    [Required]
    [MaxLength(50)]
    public string EstadoRevision { get; set; } = "No revisado";

    [MaxLength(1000)]
    public string? Observacion { get; set; }
}
