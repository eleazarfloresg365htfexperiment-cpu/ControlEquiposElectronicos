using ControlEquiposElectronicos.Api.Entities.Reportes;
using ControlEquiposElectronicos.Api.Entities.Mantenimientos;
using ControlEquiposElectronicos.Api.Entities.Auditoria;

namespace ControlEquiposElectronicos.Api.Entities.Seguridad;

public class Usuario
{
    public int UsuarioId { get; set; }

    // INFORMACIÓN PERSONAL
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;

    public string? Telefono { get; set; }
    public string? Correo { get; set; }

    // ACCESO
    public string NombreUsuario { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // ROL ÚNICO
    public int RolId { get; set; }
    public Rol Rol { get; set; } = null!;

    // CONTROL
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; set; }

    // RELACIONES
    public ICollection<ReporteFalla> ReportesRealizados { get; set; } = new List<ReporteFalla>();

    public ICollection<Mantenimiento> MantenimientosRealizados { get; set; } = new List<Mantenimiento>();

    public ICollection<HistorialOperacion> HistorialOperaciones { get; set; } = new List<HistorialOperacion>();

    public ICollection<HistorialEstadoEquipo> HistorialEstadosEquipo { get; set; } = new List<HistorialEstadoEquipo>();

    public ICollection<ReclasificacionEquipo> ReclasificacionesRealizadas { get; set; } = new List<ReclasificacionEquipo>();
}