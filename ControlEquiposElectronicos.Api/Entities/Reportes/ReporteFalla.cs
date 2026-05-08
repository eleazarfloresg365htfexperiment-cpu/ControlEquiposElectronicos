using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Entities.Seguridad;

namespace ControlEquiposElectronicos.Api.Entities.Reportes;

public class ReporteFalla
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public int UsuarioReportaId { get; set; }
    public Usuario UsuarioReporta { get; set; } = null!;

    public int EstadoReporteId { get; set; }
    public EstadoReporte EstadoReporte { get; set; } = null!;

    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public string Prioridad { get; set; } = "Media";

    public DateTime FechaReporte { get; set; } = DateTime.UtcNow;
    public DateTime? FechaCierre { get; set; }

    public string? ObservacionesCierre { get; set; }
}