namespace ControlEquiposElectronicos.DTOs.Usuarios;

// Datos que se envían a la API para crear un usuario (POST /api/Usuarios)
public class CrearUsuarioDto
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
}
 