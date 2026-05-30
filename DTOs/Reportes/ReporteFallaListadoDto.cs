using System.Text.Json.Serialization;

namespace ControlEquiposElectronicos.DTOs.Reportes;

public class ReporteFallaListadoDto
{
    public int Id { get; set; }
    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;

    [JsonPropertyName("estado")]
    public string EstadoReporte { get; set; } = string.Empty;
    public string Prioridad { get; set; } = string.Empty;
    public DateTime FechaReporte { get; set; }
}
