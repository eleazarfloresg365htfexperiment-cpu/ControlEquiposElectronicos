using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

public class PermisoService : IPermisoService
{
    private readonly SesionService _sesionService;

    public PermisoService(SesionService sesionService)
    {
        _sesionService = sesionService;
    }

    public bool TienePermiso(string permiso) => _sesionService.TienePermiso(permiso);

    public bool PuedeVerModulo(string modulo) => _sesionService.TienePermiso($"{modulo}.Ver");

    public bool EsAdministradorOP()
    {
        var rol = _sesionService.UsuarioActual?.Rol;
        return rol is "OP" or "Administrador";
    }
}
