namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class Equipo
{
    public int Id { get; set; }

    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;

    public int CategoriaEquipoId { get; set; }
    public CategoriaEquipo CategoriaEquipo { get; set; } = null!;

    public int TipoEquipoId { get; set; }
    public TipoEquipo TipoEquipo { get; set; } = null!;

    public int EstadoEquipoId { get; set; }
    public EstadoEquipo EstadoEquipo { get; set; } = null!;

    public int? UbicacionId { get; set; }
    public Ubicacion? Ubicacion { get; set; }

    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? NumeroSerie { get; set; }

    public DateTime? FechaCompra { get; set; }
    public DateTime? FechaInstalacion { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaActualizacion { get; set; }

    public DetalleComputadora? DetalleComputadora { get; set; }
    public DetalleImpresora? DetalleImpresora { get; set; }
    public DetalleRed? DetalleRed { get; set; }
    public DetalleUPS? DetalleUPS { get; set; }
    public DetalleAmbiental? DetalleAmbiental { get; set; }
}