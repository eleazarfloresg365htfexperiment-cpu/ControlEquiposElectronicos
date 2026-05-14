namespace ControlEquiposElectronicos.DTOs.Checklist;

public class ChecklistTecnicoDto
{
    public int Id { get; set; }

    public int UbicacionId { get; set; }
    public string Ubicacion { get; set; } = string.Empty;

    public int TecnicoId { get; set; }
    public string Tecnico { get; set; } = string.Empty;

    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFinalizacion { get; set; }

    public string EstadoChecklist { get; set; } = string.Empty;
    public string? ObservacionesGenerales { get; set; }

    public bool Activo { get; set; }

    public List<ChecklistTecnicoEquipoDto> EquiposRevisados { get; set; } = new();
}
