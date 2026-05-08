namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class DetalleAmbiental
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public string? TipoAmbiental { get; set; }
    public string? BTU { get; set; }
    public DateTime? FechaUltimoServicio { get; set; }
}