namespace ControlEquiposElectronicos.DTOs.Checklist;

public class ChecklistTecnicoDetalleDto
{
    public int Id { get; set; }
    public int ChecklistTecnicoEquipoId { get; set; }

    public int PlantillaChecklistItemId { get; set; }
    public string Item { get; set; } = string.Empty;
    public string? DescripcionItem { get; set; }

    public int Orden { get; set; }
    public bool EsObligatorio { get; set; }

    public string EstadoRevision { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}
