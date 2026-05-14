namespace ControlEquiposElectronicos.DTOs.Checklist;

public class GuardarChecklistDetalleDto
{
    public int ChecklistTecnicoDetalleId { get; set; }
    public string EstadoRevision { get; set; } = "No revisado";
    public string? Observacion { get; set; }
}
