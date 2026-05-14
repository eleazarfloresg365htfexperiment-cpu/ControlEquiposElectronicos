namespace ControlEquiposElectronicos.DTOs;

public class UsuarioSesionDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public List<string> Permisos { get; set; } = new();
}