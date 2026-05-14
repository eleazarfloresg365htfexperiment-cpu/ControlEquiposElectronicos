namespace ControlEquiposElectronicos.DTOs.Checklist;

public class GuardarChecklistEquipoDto
{
    public int ChecklistTecnicoEquipoId { get; set; }
    public string ResultadoGeneral { get; set; } = "Pendiente";
    public string? ObservacionesEquipo { get; set; }

    public List<GuardarChecklistDetalleDto> Detalles { get; set; } = new();
}
