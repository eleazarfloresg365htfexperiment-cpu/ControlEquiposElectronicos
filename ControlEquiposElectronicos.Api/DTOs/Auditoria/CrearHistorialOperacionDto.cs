using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Auditoria;

public class CrearHistorialOperacionDto
{
    public int? UsuarioId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Accion { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Modulo { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? TablaAfectada { get; set; }

    public int? RegistroId { get; set; }

    public string? Descripcion { get; set; }

    [MaxLength(100)]
    public string? DireccionIP { get; set; }
}