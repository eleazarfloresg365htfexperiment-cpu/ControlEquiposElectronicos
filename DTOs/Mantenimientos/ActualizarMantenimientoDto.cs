namespace ControlEquiposElectronicos.DTOs.Mantenimientos;

public class ActualizarMantenimientoDto
{
    public int TipoMantenimientoId { get; set; }
    public int TecnicoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? Diagnostico { get; set; }
    public decimal? CostoEstimado { get; set; }
    public string EstadoMantenimiento { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}