namespace ControlEquiposElectronicos.Api.Entities.Seguridad;

public class Rol
{
    public int RolId { get; set; }

    public string NombreRol { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public bool EsSistema { get; set; } = false;
    public bool Activo { get; set; } = true;

    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
}