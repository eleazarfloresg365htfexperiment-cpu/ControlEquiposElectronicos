using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Entities.Reportes;
using ControlEquiposElectronicos.Api.Entities.Seguridad;

namespace ControlEquiposElectronicos.Api.Entities.Mantenimientos;

public class Mantenimiento
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public int? ReporteFallaId { get; set; }
    public ReporteFalla? ReporteFalla { get; set; }

    public int TipoMantenimientoId { get; set; }
    public TipoMantenimiento TipoMantenimiento { get; set; } = null!;

    public int TecnicoId { get; set; }
    public Usuario Tecnico { get; set; } = null!;

    public string Descripcion { get; set; } = string.Empty;
    public string? Diagnostico { get; set; }
    public string? Resultado { get; set; }

    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
    public DateTime? FechaFin { get; set; }

    public decimal? CostoEstimado { get; set; }
    public string EstadoMantenimiento { get; set; } = "En proceso";

    public bool Activo { get; set; } = true;

    public ICollection<MantenimientoRepuesto> Repuestos { get; set; } = new List<MantenimientoRepuesto>();
}
