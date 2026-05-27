using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Auth;

public class LoginRequestDto
{
    [Required]
    public string Nickname { get; set; } = string.Empty;

    [Required]
    public string Contrasena { get; set; } = string.Empty;
}
