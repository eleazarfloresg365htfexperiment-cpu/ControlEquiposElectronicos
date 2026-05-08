namespace ControlEquiposElectronicos.Api.DTOs.Reclasificaciones;

public class ReclasificacionEquipoDetalleDto
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public string CodigoActualEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public int UsuarioId { get; set; }
    public string Usuario { get; set; } = string.Empty;

    public string? CodigoAnterior { get; set; }
    public string? CodigoNuevo { get; set; }

    public int? EstadoAnteriorId { get; set; }
    public string? EstadoAnterior { get; set; }

    public int? EstadoNuevoId { get; set; }
    public string? EstadoNuevo { get; set; }

    public int? UbicacionAnteriorId { get; set; }
    public string? UbicacionAnterior { get; set; }

    public int? UbicacionNuevaId { get; set; }
    public string? UbicacionNueva { get; set; }

    public string Motivo { get; set; } = string.Empty;
    public string TipoReclasificacion { get; set; } = string.Empty;

    public DateTime FechaReclasificacion { get; set; }
}