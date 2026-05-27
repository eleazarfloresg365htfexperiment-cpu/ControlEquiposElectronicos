namespace ControlEquiposElectronicos.DTOs.Mantenimientos;

public class CrearMantenimientoDto
{
    public int EquipoId { get; set; }
    public int TipoMantenimientoId { get; set; }
    public int TecnicoId { get; set; }
    public int? ReporteFallaId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? Diagnostico { get; set; }
    public decimal? CostoEstimado { get; set; }
    public string EstadoMantenimiento { get; set; } = "Pendiente";
}