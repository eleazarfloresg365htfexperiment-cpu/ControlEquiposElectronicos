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

        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
        {
            await DisplayAlert("Datos incompletos",
                "Por favor ingresa tu usuario y contraseña.", "Entendido");
            return;
        }

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
}