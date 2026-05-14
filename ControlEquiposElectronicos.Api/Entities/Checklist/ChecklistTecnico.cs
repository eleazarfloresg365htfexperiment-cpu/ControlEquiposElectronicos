using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Entities.Seguridad;

namespace ControlEquiposElectronicos.Api.Entities.Checklist;

public class ChecklistTecnico
{
    public int Id { get; set; }

    public int UbicacionId { get; set; }

    public Ubicacion Ubicacion { get; set; } = null!;

    public int TecnicoId { get; set; }

    public Usuario Tecnico { get; set; } = null!;

    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;

    public DateTime? FechaFinalizacion { get; set; }

    public string EstadoChecklist { get; set; } = "En proceso";

    public string? ObservacionesGenerales { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaActualizacion { get; set; }

    public ICollection<ChecklistTecnicoEquipo> EquiposRevisados { get; set; } = new List<ChecklistTecnicoEquipo>();
}