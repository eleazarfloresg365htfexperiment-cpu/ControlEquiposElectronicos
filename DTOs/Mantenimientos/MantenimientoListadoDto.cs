namespace ControlEquiposElectronicos.DTOs.Mantenimientos;

public class MantenimientoListadoDto
{
    public int Id { get; set; }
    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;
    public string TipoMantenimiento { get; set; } = string.Empty;
    public string EstadoMantenimiento { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}
