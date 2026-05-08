using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class CrearDetalleComputadoraDto
{
    [MaxLength(150)]
    public string? Procesador { get; set; }

    public int? RamGB { get; set; }

    [MaxLength(100)]
    public string? SerialRam { get; set; }

    [MaxLength(100)]
    public string? AlmacenamientoTipo { get; set; }

    public int? AlmacenamientoCapacidadGB { get; set; }

    [MaxLength(150)]
    public string? SistemaOperativo { get; set; }

    [MaxLength(50)]
    public string? NumeroEquipo { get; set; }
}