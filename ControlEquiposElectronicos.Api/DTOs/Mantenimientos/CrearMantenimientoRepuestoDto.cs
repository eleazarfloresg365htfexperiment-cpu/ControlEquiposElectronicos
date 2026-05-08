using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Mantenimientos;

public class CrearMantenimientoRepuestoDto
{
    [Required]
    [MaxLength(150)]
    public string NombreRepuesto { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [Required]
    public int Cantidad { get; set; }

    public decimal? CostoUnitario { get; set; }

    [MaxLength(100)]
    public string? NumeroSerieAnterior { get; set; }

    [MaxLength(100)]
    public string? NumeroSerieNuevo { get; set; }
}