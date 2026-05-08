namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class DetalleUPS
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public string? CapacidadVA { get; set; }
    public DateTime? FechaCambioBateria { get; set; }
    public string? EstadoBateria { get; set; }
}