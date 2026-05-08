namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class DetalleAmbientalDto
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public string? TipoAmbiental { get; set; }
    public string? BTU { get; set; }
    public DateTime? FechaUltimoServicio { get; set; }
}