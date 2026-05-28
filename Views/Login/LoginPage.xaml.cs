using ControlEquiposElectronicos.DTOs;
using ControlEquiposElectronicos.Services;

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

        // Validación básica: que no estén vacíos
        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
        {
            await DisplayAlert("Datos incompletos",
                "Por favor ingresa tu usuario y contraseña.", "Entendido");
            return;
        }

        // Llamar a la API para validar las credenciales
        var respuesta = await _authService.LoginAsync(usuario, contrasena);

        // Si la API devuelve null, las credenciales son incorrectas
        if (respuesta == null)
        {
            await DisplayAlert("Acceso denegado",
                "Usuario o contraseña incorrectos. Por favor verifica tus datos e intenta de nuevo.",
                "Entendido");
            ContrasenaEntry.Text = string.Empty;
            return;
        }

        // Login exitoso: armar la sesión con los datos reales del usuario
        // Convertir los permisos al formato "Modulo.Accion" que usa la app
        var permisosTexto = new List<string>();
        foreach (var p in respuesta.Permisos)
        {
            permisosTexto.Add($"{p.Modulo}.{p.Accion}");
        }

        var usuarioSesion = new UsuarioSesionDto
        {
            Nombre = respuesta.NombreCompleto,
            Rol = respuesta.Rol,
            Permisos = permisosTexto
        };

        _sesion.IniciarSesion(usuarioSesion);

        Application.Current!.Windows[0].Page = new AppShell(_sesion);
    }

    private async void OnOlvidasteContrasenaTapped(object? sender, TappedEventArgs e)
    {
        await DisplayAlert("Recuperar contraseña",
            "Para restablecer tu contraseña, contacta al administrador al siguiente número: +502 1234-5678",
            "Entendido");
    }

    private async void OnRegistrarseTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new RegistroUsuarioPage());
    }
}
