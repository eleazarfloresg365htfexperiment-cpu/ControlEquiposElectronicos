using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class ActualizarSwitchPuertoDto
{
    [Required]
    public int NumeroPuerto { get; set; }

    public bool EstaActivo { get; set; } = true;
    public bool EstaDanado { get; set; } = false;
    public bool EstaOcupado { get; set; } = false;

    public int? EquipoConectadoId { get; set; }
    public int? UbicacionDestinoId { get; set; }

    public string? Observaciones { get; set; }
}