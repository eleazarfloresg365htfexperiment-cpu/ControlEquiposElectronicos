namespace ControlEquiposElectronicos.Api.Entities.Seguridad;

public class Permiso
{
    public int Id { get; set; }
    public string Modulo { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
}