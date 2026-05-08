using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class ActualizarEquipoDto
{
    [Required]
    [MaxLength(50)]
    public string Codigo { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public int CategoriaEquipoId { get; set; }

    [Required]
    public int TipoEquipoId { get; set; }

    [Required]
    public int EstadoEquipoId { get; set; }

    public int? UbicacionId { get; set; }

    [MaxLength(100)]
    public string? Marca { get; set; }

    [MaxLength(100)]
    public string? Modelo { get; set; }

    [MaxLength(100)]
    public string? NumeroSerie { get; set; }

    public DateTime? FechaCompra { get; set; }

    public DateTime? FechaInstalacion { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; } = true;
}