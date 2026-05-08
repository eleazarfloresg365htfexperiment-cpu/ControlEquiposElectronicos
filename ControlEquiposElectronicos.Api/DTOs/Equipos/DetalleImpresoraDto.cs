namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class DetalleImpresoraDto
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public string? TipoImpresora { get; set; }
    public string? TipoCartucho { get; set; }
    public string? ModeloCartucho { get; set; }

    public DateTime? FechaUltimoCambioCartucho { get; set; }

    public int? ContadorImpresiones { get; set; }
}
