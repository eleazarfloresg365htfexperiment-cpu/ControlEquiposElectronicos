namespace ControlEquiposElectronicos.Api.DTOs.ReportesFallas;

public class ReporteFallaDetalleDto
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public int UsuarioReportaId { get; set; }
    public string UsuarioReporta { get; set; } = string.Empty;

    public int EstadoReporteId { get; set; }
    public string Estado { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Prioridad { get; set; } = string.Empty;

    public DateTime FechaReporte { get; set; }
    public DateTime? FechaCierre { get; set; }

    public string? ObservacionesCierre { get; set; }
}