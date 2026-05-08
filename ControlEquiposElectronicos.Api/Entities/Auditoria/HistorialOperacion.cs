using ControlEquiposElectronicos.Api.Entities.Seguridad;

namespace ControlEquiposElectronicos.Api.Entities.Auditoria;

public class HistorialOperacion
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public string Accion { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string? TablaAfectada { get; set; }
    public int? RegistroId { get; set; }

    public string? Descripcion { get; set; }
    public string? DireccionIP { get; set; }

    public DateTime FechaOperacion { get; set; } = DateTime.UtcNow;
}