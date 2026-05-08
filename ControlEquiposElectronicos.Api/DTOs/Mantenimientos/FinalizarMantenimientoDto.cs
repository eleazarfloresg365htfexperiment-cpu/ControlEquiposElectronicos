using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Mantenimientos;

public class FinalizarMantenimientoDto
{
    public string? Resultado { get; set; }

    [Required]
    [MaxLength(50)]
    public string EstadoMantenimiento { get; set; } = "Finalizado";
}