namespace ControlEquiposElectronicos.DTOs.Auth;

// Datos que enviamos a la API para iniciar sesión
public class LoginRequestDto
{
    public string Nickname { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
}
 