namespace ControlEquiposElectronicos.Api.Entities.Seguridad;

public class RolPermiso
{
    public int Id { get; set; }

    public int RolId { get; set; }
    public Rol Rol { get; set; } = null!;

    public int PermisoId { get; set; }
    public Permiso Permiso { get; set; } = null!;

    public bool Permitido { get; set; } = true;
}