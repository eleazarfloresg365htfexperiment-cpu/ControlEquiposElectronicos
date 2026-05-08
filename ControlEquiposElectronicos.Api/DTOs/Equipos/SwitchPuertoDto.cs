namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class SwitchPuertoDto
{
    public int Id { get; set; }

    public int SwitchEquipoId { get; set; }
    public string CodigoSwitch { get; set; } = string.Empty;
    public string NombreSwitch { get; set; } = string.Empty;

    public int NumeroPuerto { get; set; }

    public bool EstaActivo { get; set; }
    public bool EstaDanado { get; set; }
    public bool EstaOcupado { get; set; }

    public int? EquipoConectadoId { get; set; }
    public string? CodigoEquipoConectado { get; set; }
    public string? NombreEquipoConectado { get; set; }

    public int? UbicacionDestinoId { get; set; }
    public string? UbicacionDestino { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}