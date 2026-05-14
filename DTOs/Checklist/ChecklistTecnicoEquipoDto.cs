namespace ControlEquiposElectronicos.DTOs.Checklist;

public class ChecklistTecnicoEquipoDto
{
    public int Id { get; set; }
    public int ChecklistTecnicoId { get; set; }

    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public int TipoEquipoId { get; set; }
    public string TipoEquipo { get; set; } = string.Empty;

    public int PlantillaChecklistId { get; set; }
    public string PlantillaChecklist { get; set; } = string.Empty;

    public string ResultadoGeneral { get; set; } = string.Empty;
    public string? ObservacionesEquipo { get; set; }
    public DateTime? FechaRevision { get; set; }

    public List<ChecklistTecnicoDetalleDto> Detalles { get; set; } = new();
}
