namespace ControlEquiposElectronicos.Api.DTOs.Mantenimientos;

public class MantenimientoDetalleDto
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public int? ReporteFallaId { get; set; }

    public int TipoMantenimientoId { get; set; }
    public string TipoMantenimiento { get; set; } = string.Empty;

    public int TecnicoId { get; set; }
    public string Tecnico { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;
    public string? Diagnostico { get; set; }
    public string? Resultado { get; set; }

    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public decimal? CostoEstimado { get; set; }

    public string EstadoMantenimiento { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public List<MantenimientoRepuestoDto> Repuestos { get; set; } = new();
}