using ControlEquiposElectronicos.DTOs;
using ControlEquiposElectronicos.Services;

namespace ControlEquiposElectronicos.Views.Login;

public partial class LoginPage : ContentPage
{
    private readonly SesionService _sesion;

    public LoginPage(SesionService sesion)
    {
        InitializeComponent();
        _sesion = sesion;
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

        // Login simulado (mientras se conecta el AuthService real con la API)
        var usuarioSesion = new UsuarioSesionDto
        {
            Nombre = "Admin CPC",
            Rol = "Administrador",
            Permisos = new List<string>
            {
                "Dashboard.Ver",
                "Equipos.Ver",
                "Mantenimientos.Ver",
                "Reportes.Ver",
                "Usuarios.Ver",
                "Configuracion.Ver"
            }
        };

        _sesion.IniciarSesion(usuarioSesion);

        Application.Current!.Windows[0].Page = new AppShell(_sesion);
    }

    private async void OnRegistrarseTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new RegistroUsuarioPage());
    }
}