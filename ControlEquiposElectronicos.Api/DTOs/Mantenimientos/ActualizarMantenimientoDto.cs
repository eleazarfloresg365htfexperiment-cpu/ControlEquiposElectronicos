using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Mantenimientos;

public class ActualizarMantenimientoDto
{
    [Required]
    public int TipoMantenimientoId { get; set; }

    [Required]
    public int TecnicoId { get; set; }

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    public string? Diagnostico { get; set; }

    public decimal? CostoEstimado { get; set; }

    [Required]
    [MaxLength(50)]
    public string EstadoMantenimiento { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}