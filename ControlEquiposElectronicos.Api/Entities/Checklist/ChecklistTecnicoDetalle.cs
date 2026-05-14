namespace ControlEquiposElectronicos.Api.Entities.Checklist;

public class ChecklistTecnicoDetalle
{
    public int Id { get; set; }

    public int ChecklistTecnicoEquipoId { get; set; }

    public ChecklistTecnicoEquipo ChecklistTecnicoEquipo { get; set; } = null!;

    public int PlantillaChecklistItemId { get; set; }

    public PlantillaChecklistItem PlantillaChecklistItem { get; set; } = null!;

    public string EstadoRevision { get; set; } = "No revisado";

    public string? Observacion { get; set; }
}