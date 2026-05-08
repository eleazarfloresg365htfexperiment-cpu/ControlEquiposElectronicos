using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class CrearDetalleAmbientalDto
{
    [MaxLength(100)]
    public string? TipoAmbiental { get; set; }

    [MaxLength(100)]
    public string? BTU { get; set; }

    public DateTime? FechaUltimoServicio { get; set; }
}