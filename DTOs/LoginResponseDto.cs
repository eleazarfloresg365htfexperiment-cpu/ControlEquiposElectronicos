namespace ControlEquiposElectronicos.DTOs.Auth;

// Lo que la API devuelve al iniciar sesión correctamente
public class LoginResponseDto
{
    public int UsuarioId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int RolId { get; set; }
    public string Rol { get; set; } = string.Empty;
    public List<PermisoSesionDto> Permisos { get; set; } = new();
}

public class PermisoSesionDto
{
    public int PermisoId { get; set; }
    public string Modulo { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}
