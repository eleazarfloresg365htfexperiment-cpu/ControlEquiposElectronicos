namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class EquipoDetalleDto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;

    public int CategoriaEquipoId { get; set; }
    public string Categoria { get; set; } = string.Empty;

    public int TipoEquipoId { get; set; }
    public string Tipo { get; set; } = string.Empty;

    public int EstadoEquipoId { get; set; }
    public string Estado { get; set; } = string.Empty;

    public int? UbicacionId { get; set; }
    public string? Ubicacion { get; set; }

    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? NumeroSerie { get; set; }

    public DateTime? FechaCompra { get; set; }
    public DateTime? FechaInstalacion { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}
