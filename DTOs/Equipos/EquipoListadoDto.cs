namespace ControlEquiposElectronicos.DTOs.Equipos;

public class EquipoListadoDto
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

    public int UbicacionId { get; set; }
    public string Ubicacion { get; set; } = string.Empty;

    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string NumeroSerie { get; set; } = string.Empty;

    public DateTime? FechaCompra { get; set; }
    public DateTime? FechaInstalacion { get; set; }

    public string? Observaciones { get; set; }
    public bool Activo { get; set; }
}
