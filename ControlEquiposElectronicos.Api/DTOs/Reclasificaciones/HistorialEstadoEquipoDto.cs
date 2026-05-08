namespace ControlEquiposElectronicos.Api.DTOs.Reclasificaciones;

public class HistorialEstadoEquipoDto
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public int EstadoAnteriorId { get; set; }
    public string EstadoAnterior { get; set; } = string.Empty;

    public int EstadoNuevoId { get; set; }
    public string EstadoNuevo { get; set; } = string.Empty;

    public int UsuarioId { get; set; }
    public string Usuario { get; set; } = string.Empty;

    public string Motivo { get; set; } = string.Empty;

    public DateTime FechaCambio { get; set; }
}