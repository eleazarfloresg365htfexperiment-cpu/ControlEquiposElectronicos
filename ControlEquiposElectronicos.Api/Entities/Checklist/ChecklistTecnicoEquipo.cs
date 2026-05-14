using ControlEquiposElectronicos.Api.Entities.Inventario;

namespace ControlEquiposElectronicos.Api.Entities.Checklist;

public class ChecklistTecnicoEquipo
{
    public int Id { get; set; }

    public int ChecklistTecnicoId { get; set; }

    public ChecklistTecnico ChecklistTecnico { get; set; } = null!;

    public int EquipoId { get; set; }

    public Equipo Equipo { get; set; } = null!;

    public int PlantillaChecklistId { get; set; }

    public PlantillaChecklist PlantillaChecklist { get; set; } = null!;

    public string ResultadoGeneral { get; set; } = "Pendiente";

    public string? ObservacionesEquipo { get; set; }

    public DateTime? FechaRevision { get; set; }

    public ICollection<ChecklistTecnicoDetalle> Detalles { get; set; } = new List<ChecklistTecnicoDetalle>();
}