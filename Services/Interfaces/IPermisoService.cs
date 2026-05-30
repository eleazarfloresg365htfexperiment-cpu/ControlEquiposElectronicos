namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IPermisoService
{
    bool TienePermiso(string permiso);
    bool PuedeVerModulo(string modulo);
    bool EsAdministradorOP();
}
