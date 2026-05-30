using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels;

public class ConfiguracionViewModel : BaseViewModel
{
    private readonly SesionService _sesionService;
    private readonly IPermisoService _permisoService;

    public string NombreUsuario => _sesionService.UsuarioActual?.Nombre ?? "Usuario";
    public string RolUsuario => _sesionService.UsuarioActual?.Rol ?? "Sin rol";
    public bool TieneAcceso => _permisoService.EsAdministradorOP();
    public bool MostrarAuditoria => _sesionService.UsuarioActual?.Rol == "OP";

    public ICommand IrPermisosCommand { get; }
    public ICommand IrRegistroUsuarioCommand { get; }
    public ICommand IrRolesUsuarioCommand { get; }
    public ICommand IrAuditoriaCommand { get; }

    public ConfiguracionViewModel(SesionService sesionService, IPermisoService permisoService)
    {
        Title = "Configuración";
        _sesionService = sesionService;
        _permisoService = permisoService;

        IrPermisosCommand = new Command(async () => await Shell.Current.GoToAsync("PermisosRol"));
        IrRegistroUsuarioCommand = new Command(async () => await Shell.Current.GoToAsync("RegistroUsuario"));
        IrRolesUsuarioCommand = new Command(async () => await Shell.Current.GoToAsync("RolesUsuario"));
        IrAuditoriaCommand = new Command(async () => await Shell.Current.GoToAsync("Auditoria"));
    }

    public void Refrescar()
    {
        OnPropertyChanged(nameof(NombreUsuario));
        OnPropertyChanged(nameof(RolUsuario));
        OnPropertyChanged(nameof(TieneAcceso));
        OnPropertyChanged(nameof(MostrarAuditoria));
    }
}
