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

    private void OnLoginClicked(object? sender, EventArgs e)
    {
        var usuario = new UsuarioSesionDto
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

        _sesion.IniciarSesion(usuario);

        Application.Current!.Windows[0].Page = new AppShell(_sesion);
    }
}