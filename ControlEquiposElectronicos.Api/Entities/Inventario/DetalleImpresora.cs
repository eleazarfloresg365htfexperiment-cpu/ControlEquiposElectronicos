namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class DetalleImpresora
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public string? TipoImpresora { get; set; }
    public string? TipoCartucho { get; set; }
    public string? ModeloCartucho { get; set; }

    public DateTime? FechaUltimoCambioCartucho { get; set; }
    public int? ContadorImpresiones { get; set; }
}