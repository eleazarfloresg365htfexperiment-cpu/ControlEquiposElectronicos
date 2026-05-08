namespace ControlEquiposElectronicos.Api.Entities.Reportes;

public class EstadoReporte
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;

    public ICollection<ReporteFalla> ReportesFalla { get; set; } = new List<ReporteFalla>();
}
