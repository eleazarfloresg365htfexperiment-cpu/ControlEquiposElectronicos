using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.ReportesFallas;

public class CrearReporteFallaDto
{
    [Required]
    public int EquipoId { get; set; }

    [Required]
    public int UsuarioReportaId { get; set; }

    [Required]
    public int EstadoReporteId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Prioridad { get; set; } = string.Empty;
}