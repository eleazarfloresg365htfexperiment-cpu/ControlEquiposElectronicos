namespace ControlEquiposElectronicos.DTOs.Equipos;

public class CrearEquipoDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;

    public int CategoriaEquipoId { get; set; }
    public int TipoEquipoId { get; set; }
    public int EstadoEquipoId { get; set; }
    public int UbicacionId { get; set; }

    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string NumeroSerie { get; set; } = string.Empty;

    public DateTime? FechaCompra { get; set; }
    public DateTime? FechaInstalacion { get; set; }

    public string? Observaciones { get; set; }
    public bool Activo { get; set; } = true;
}
