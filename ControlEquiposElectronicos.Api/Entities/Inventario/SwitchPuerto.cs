namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class SwitchPuerto
{
    public int Id { get; set; }

    public int SwitchEquipoId { get; set; }
    public Equipo SwitchEquipo { get; set; } = null!;

    public int NumeroPuerto { get; set; }

    public bool EstaActivo { get; set; } = true;
    public bool EstaDanado { get; set; } = false;
    public bool EstaOcupado { get; set; } = false;

    public int? EquipoConectadoId { get; set; }
    public Equipo? EquipoConectado { get; set; }

    public int? UbicacionDestinoId { get; set; }
    public Ubicacion? UbicacionDestino { get; set; }

    public string? Observaciones { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaActualizacion { get; set; }
}