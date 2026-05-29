using ControlEquiposElectronicos.DTOs;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Views.Login;

public partial class LoginPage : ContentPage
{
    private readonly SesionService _sesion;
    private readonly IAuthService _authService;

    public LoginPage(SesionService sesion)
    {
        InitializeComponent();
        _sesion = sesion;
        _authService = IPlatformApplication.Current!.Services.GetRequiredService<IAuthService>();
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        var usuario = UsuarioEntry.Text?.Trim() ?? string.Empty;
        var contrasena = ContrasenaEntry.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
        {
            await DisplayAlert("Datos incompletos",
                "Por favor ingresa tu usuario y contraseña.", "Entendido");
            return;
        }

        var respuesta = await _authService.LoginAsync(usuario, contrasena);

        if (respuesta == null)
        {
            await DisplayAlert("Acceso denegado",
                "Usuario o contraseña incorrectos.", "Entendido");
            ContrasenaEntry.Text = string.Empty;
            return;
        }

        var permisos = respuesta.Permisos
            .Select(p => $"{p.Modulo}.{p.Accion}")
            .ToList();

        _sesion.IniciarSesion(new UsuarioSesionDto
        {
            Nombre = respuesta.NombreCompleto,
            Rol = respuesta.Rol,
            Permisos = permisos
        });

        // ← Vuelve a AppShell (Shell nativo = API funciona)
        Application.Current!.Windows[0].Page = new AppShell(_sesion);
    }

    private async void OnRegistrarseTapped(object? sender, TappedEventArgs e)
        => await Navigation.PushModalAsync(new RegistroUsuarioPage(false));
}
