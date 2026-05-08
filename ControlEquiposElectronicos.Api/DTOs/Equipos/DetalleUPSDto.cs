namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class DetalleUPSDto
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public string? CapacidadVA { get; set; }
    public DateTime? FechaCambioBateria { get; set; }
    public string? EstadoBateria { get; set; }
}
