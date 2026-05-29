using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Usuarios;

public class ActualizarRolUsuarioDto
{
    [Required]
    [MaxLength(50)]
    public string Rol { get; set; } = string.Empty;
}