namespace ControlEquiposElectronicos.Api.DTOs.Auditoria;

public class HistorialOperacionDto
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }
    public string? Usuario { get; set; }

    public string Accion { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;

    public string? TablaAfectada { get; set; }
    public int? RegistroId { get; set; }

    public string? Descripcion { get; set; }
    public string? DireccionIP { get; set; }

    public DateTime FechaOperacion { get; set; }
}