using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class ActualizarDetalleRedDto
{
    [MaxLength(50)]
    public string? DireccionIP { get; set; }

    [MaxLength(50)]
    public string? MacAddress { get; set; }

    public int? CantidadPuertos { get; set; }

    public int? PuertosDanados { get; set; }

    public int? EquipoProveedorId { get; set; }

    public string? ObservacionesRed { get; set; }
}