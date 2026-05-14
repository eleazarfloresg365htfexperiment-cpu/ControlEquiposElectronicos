namespace ControlEquiposElectronicos.DTOs.Usuarios;

public class UsuarioListadoDto
{
    public int UsuarioId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
