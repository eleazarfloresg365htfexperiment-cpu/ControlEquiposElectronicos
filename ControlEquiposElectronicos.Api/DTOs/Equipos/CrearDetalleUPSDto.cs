using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class CrearDetalleUPSDto
{
    [MaxLength(100)]
    public string? CapacidadVA { get; set; }

    public DateTime? FechaCambioBateria { get; set; }

    [MaxLength(100)]
    public string? EstadoBateria { get; set; }
}