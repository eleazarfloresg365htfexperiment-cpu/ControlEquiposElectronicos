using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class CrearDetalleImpresoraDto
{
    [MaxLength(100)]
    public string? TipoImpresora { get; set; }

    [MaxLength(100)]
    public string? TipoCartucho { get; set; }

    [MaxLength(100)]
    public string? ModeloCartucho { get; set; }

    public DateTime? FechaUltimoCambioCartucho { get; set; }

    public int? ContadorImpresiones { get; set; }
}