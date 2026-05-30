using ControlEquiposElectronicos.DTOs;
using ControlEquiposElectronicos.DTOs.Auth;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly SesionService _sesionService;

    private string _usuario = string.Empty;
    public string Usuario
    {
        get => _usuario;
        set { _usuario = value; OnPropertyChanged(); }
    }

    private string _contrasena = string.Empty;
    public string Contrasena
    {
        get => _contrasena;
        set { _contrasena = value; OnPropertyChanged(); }
    }

    public ICommand IniciarSesionCommand { get; }
    public ICommand RegistrarseCommand { get; }

    public LoginViewModel(IAuthService authService, SesionService sesionService)
    {
        Title = "Inicio de sesión";
        _authService = authService;
        _sesionService = sesionService;
        IniciarSesionCommand = new Command(async () => await IniciarSesionAsync());
        RegistrarseCommand = new Command(async () => await AbrirRegistroAsync());
    }

    private async Task IniciarSesionAsync()
    {
        if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Contrasena))
        {
            await Shell.Current.DisplayAlertAsync("Datos incompletos",
                "Por favor ingresa tu usuario y contraseña.", "Entendido");
            return;
        }

        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var respuesta = await _authService.LoginAsync(Usuario.Trim(), Contrasena);

            if (respuesta == null)
            {
                await Shell.Current.DisplayAlertAsync("Acceso denegado",
                    "Usuario o contraseña incorrectos.", "Entendido");
                Contrasena = string.Empty;
                return;
            }

            var permisos = respuesta.Permisos
                .Select(p => $"{p.Modulo}.{p.Accion}")
                .ToList();

            _sesionService.IniciarSesion(new UsuarioSesionDto
            {
                UsuarioId = respuesta.UsuarioId,
                Nombre = respuesta.NombreCompleto,
                Rol = respuesta.Rol,
                Permisos = permisos
            });

            var shell = IPlatformApplication.Current!.Services.GetRequiredService<AppShell>();
            Application.Current!.Windows[0].Page = shell;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static async Task AbrirRegistroAsync()
    {
        if (Application.Current?.Windows[0].Page is ContentPage page)
            await page.Navigation.PushModalAsync(new Views.Login.RegistroUsuarioPage(false));
    }
}
