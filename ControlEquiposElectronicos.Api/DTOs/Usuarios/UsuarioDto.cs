namespace ControlEquiposElectronicos.Api.DTOs.Usuarios;

public class UsuarioDto
{
    public int UsuarioId { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Nickname { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public int RolId { get; set; }

    public string Rol { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}


