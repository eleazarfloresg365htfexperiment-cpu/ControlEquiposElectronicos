namespace ControlEquiposElectronicos.DTOs;

public class UsuarioSesionDto
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public List<string> Permisos { get; set; } = new();
}