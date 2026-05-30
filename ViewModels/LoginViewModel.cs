using ControlEquiposElectronicos.DTOs;
using ControlEquiposElectronicos.DTOs.Auth;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
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
        set
        {
            _usuario = value;
            OnPropertyChanged();
        }
    }

    private string _contrasena = string.Empty;
    public string Contrasena
    {
        get => _contrasena;
        set
        {
            _contrasena = value;
            OnPropertyChanged();
        }
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
        if (IsBusy)
            return;

        var usuario = Usuario?.Trim();
        var contrasena = Contrasena?.Trim();

        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
        {
            await MostrarAlertaAsync(
                "Datos incompletos",
                "Por favor ingresa tu usuario y contraseña.");

            return;
        }

        try
        {
            IsBusy = true;

            var respuesta = await _authService.LoginAsync(usuario, contrasena);

            if (respuesta == null)
            {
                await MostrarAlertaAsync(
                    "Acceso denegado",
                    "Usuario o contraseña incorrectos.");

                Contrasena = string.Empty;
                return;
            }

            var permisos = respuesta.Permisos?
                .Select(p => $"{p.Modulo}.{p.Accion}")
                .ToList() ?? new List<string>();

            _sesionService.IniciarSesion(new UsuarioSesionDto
            {
                UsuarioId = respuesta.UsuarioId,
                Nombre = respuesta.NombreCompleto,
                Rol = respuesta.Rol,
                Permisos = permisos
            });

            var shell = IPlatformApplication.Current?
                .Services
                .GetRequiredService<AppShell>();

            if (shell == null)
            {
                await MostrarAlertaAsync(
                    "Error",
                    "No se pudo cargar el menú principal del sistema.");

                return;
            }

            if (Application.Current?.Windows.Count > 0)
            {
                Application.Current.Windows[0].Page = shell;
            }
            else
            {
                await MostrarAlertaAsync(
                    "Error",
                    "No se encontró una ventana activa para abrir el sistema.");
            }
        }
        catch (HttpRequestException)
        {
            await MostrarAlertaAsync(
                "Error de conexión",
                "No se pudo conectar con la API. Verifica que el servidor esté encendido.");
        }
        catch (TaskCanceledException)
        {
            await MostrarAlertaAsync(
                "Tiempo agotado",
                "La API tardó demasiado en responder. Intenta nuevamente.");
        }
        catch (Exception ex)
        {
#if DEBUG
            await MostrarAlertaAsync(
                "Error al iniciar sesión",
                ex.Message);
#else
    await MostrarAlertaAsync(
        "Error al iniciar sesión",
        "Ocurrió un problema inesperado. Intenta nuevamente.");
#endif
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static async Task AbrirRegistroAsync()
    {
        var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;

        if (mainPage == null)
            return;

        await mainPage.Navigation.PushModalAsync(new Views.Login.RegistroUsuarioPage(false));
    }

    private static async Task MostrarAlertaAsync(string titulo, string mensaje)
    {
        var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;

        if (mainPage != null)
        {
            await mainPage.DisplayAlertAsync(titulo, mensaje, "Entendido");
        }
    }
}
