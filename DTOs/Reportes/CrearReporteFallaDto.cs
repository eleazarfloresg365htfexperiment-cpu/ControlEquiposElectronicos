namespace ControlEquiposElectronicos.DTOs.Reportes;

public class CrearReporteFallaDto
{
    public int EquipoId { get; set; }
    public int UsuarioReportaId { get; set; }
    public int EstadoReporteId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Prioridad { get; set; } = "Media";
}