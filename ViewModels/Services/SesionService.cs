using ControlEquiposElectronicos.DTOs;
using ControlEquiposElectronicos.ViewModels.DTOs;

namespace ControlEquiposElectronicos.Services;

public class SesionService
{
    public UsuarioSesionDto? UsuarioActual { get; private set; }

    public void IniciarSesion(UsuarioSesionDto usuario)
    {
        UsuarioActual = usuario;
    }

    public void CerrarSesion()
    {
        UsuarioActual = null;
    }

    public bool TienePermiso(string permiso)
    {
        if (UsuarioActual == null) return false;

        return UsuarioActual.Permisos.Contains(permiso);
    }
}
