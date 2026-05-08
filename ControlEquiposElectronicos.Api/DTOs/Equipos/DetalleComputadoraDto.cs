namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class DetalleComputadoraDto
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public string? Procesador { get; set; }
    public int? RamGB { get; set; }
    public string? SerialRam { get; set; }

    public string? AlmacenamientoTipo { get; set; }
    public int? AlmacenamientoCapacidadGB { get; set; }

    public string? SistemaOperativo { get; set; }
    public string? NumeroEquipo { get; set; }
}