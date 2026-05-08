using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Entities.Seguridad;

namespace ControlEquiposElectronicos.Api.Entities.Reportes;

public class ReclasificacionEquipo
{
    public int ReclasificacionEquipoId { get; set; }

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public DateTime FechaReclasificacion { get; set; } = DateTime.UtcNow;

    public string TipoReclasificacion { get; set; } = string.Empty;

    public string? CodigoAnterior { get; set; }
    public string? CodigoNuevo { get; set; }

    public int? EstadoAnteriorId { get; set; }
    public EstadoEquipo? EstadoAnterior { get; set; }

    public int? EstadoNuevoId { get; set; }
    public EstadoEquipo? EstadoNuevo { get; set; }

    public int? UbicacionAnteriorId { get; set; }
    public Ubicacion? UbicacionAnterior { get; set; }

    public int? UbicacionNuevaId { get; set; }
    public Ubicacion? UbicacionNueva { get; set; }

    public string Motivo { get; set; } = string.Empty;
}