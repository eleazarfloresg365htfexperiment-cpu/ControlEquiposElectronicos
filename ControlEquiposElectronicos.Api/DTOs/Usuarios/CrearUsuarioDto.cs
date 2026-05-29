using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Usuarios;

public class CrearUsuarioDto
{
    [Required]
    [MaxLength(150)]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Nickname { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Contrasena { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Rol { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [MaxLength(150)]
    public string? Correo { get; set; }
}