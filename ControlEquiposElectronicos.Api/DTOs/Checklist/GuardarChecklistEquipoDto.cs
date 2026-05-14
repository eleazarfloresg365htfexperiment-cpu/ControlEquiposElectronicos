using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Checklist;

public class GuardarChecklistEquipoDto
{
    [Required]
    public int ChecklistTecnicoEquipoId { get; set; }

    [Required]
    [MaxLength(80)]
    public string ResultadoGeneral { get; set; } = "Pendiente";

    [MaxLength(1000)]
    public string? ObservacionesEquipo { get; set; }

    public List<GuardarChecklistDetalleDto> Detalles { get; set; } = new();
}