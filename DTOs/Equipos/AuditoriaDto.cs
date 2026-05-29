namespace ControlEquiposElectronicos.DTOs.Equipos;

public class AuditoriaDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string TablaAfectada { get; set; } = string.Empty;
    public int RegistroId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string DireccionIP { get; set; } = string.Empty;
    public DateTime FechaOperacion { get; set; }
}