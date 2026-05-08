using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Entities.Seguridad;

namespace ControlEquiposElectronicos.Api.Entities.Auditoria;

public class HistorialEstadoEquipo
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public int EstadoAnteriorId { get; set; }
    public EstadoEquipo EstadoAnterior { get; set; } = null!;

    public int EstadoNuevoId { get; set; }
    public EstadoEquipo EstadoNuevo { get; set; } = null!;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public string? Motivo { get; set; }
    public DateTime FechaCambio { get; set; } = DateTime.UtcNow;
}
