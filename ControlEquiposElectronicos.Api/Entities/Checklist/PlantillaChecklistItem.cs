namespace ControlEquiposElectronicos.Api.Entities.Checklist;

public class PlantillaChecklistItem
{
    public int Id { get; set; }

    public int PlantillaChecklistId { get; set; }

    public PlantillaChecklist PlantillaChecklist { get; set; } = null!;

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public int Orden { get; set; }

    public bool EsObligatorio { get; set; } = true;

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaActualizacion { get; set; }

    public ICollection<ChecklistTecnicoDetalle> DetallesChecklist { get; set; } = new List<ChecklistTecnicoDetalle>();
}