using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Reclasificaciones;

public class CrearReclasificacionEquipoDto
{
    [Required]
    public int EquipoId { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [MaxLength(100)]
    public string? CodigoNuevo { get; set; }

    public int? EstadoNuevoId { get; set; }

    public int? UbicacionNuevaId { get; set; }

    [Required]
    public string Motivo { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string TipoReclasificacion { get; set; } = string.Empty;
}