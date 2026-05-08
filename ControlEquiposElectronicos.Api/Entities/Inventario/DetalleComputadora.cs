namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class DetalleComputadora
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public string? Procesador { get; set; }
    public int? RamGB { get; set; }
    public string? SerialRam { get; set; }

    public string? AlmacenamientoTipo { get; set; }
    public int? AlmacenamientoCapacidadGB { get; set; }

    public string? SistemaOperativo { get; set; }
    public string? NumeroEquipo { get; set; }
}