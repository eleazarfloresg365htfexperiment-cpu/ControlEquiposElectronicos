using ControlEquiposElectronicos.Services;

namespace ControlEquiposElectronicos;

public partial class AppShell : Shell
{
    private readonly SesionService _sesion;

    public AppShell(SesionService sesion)
    {
        InitializeComponent();
        _sesion = sesion;

        Routing.RegisterRoute("PermisosRol", typeof(Views.Configuracion.PermisosRolPage));
        Routing.RegisterRoute("RegistroUsuario", typeof(Views.Login.RegistroUsuarioPage));
        Routing.RegisterRoute("RolesUsuario", typeof(Views.Configuracion.RolesUsuarioPage));

        MostrarUsuario();
        ConfigurarMenu();
    }

    // Al tocar el área del usuario en la barra lateral, muestra/oculta el mini-menú
    private void OnMenuUsuarioTapped(object? sender, TappedEventArgs e)
    {
        MenuUsuarioDesplegable.IsVisible = !MenuUsuarioDesplegable.IsVisible;
        // La flecha cambia: ▾ cuando está cerrado, ▴ cuando está abierto
        FlechaMenuLabel.Text = MenuUsuarioDesplegable.IsVisible ? "\u25B4" : "\u25BE";
    }

    private async void OnMiPerfilTapped(object? sender, TappedEventArgs e)
    {
        // Cerrar el menú primero
        MenuUsuarioDesplegable.IsVisible = false;
        FlechaMenuLabel.Text = "\u25BE";

        var usuario = _sesion.UsuarioActual;
        if (usuario == null) return;

        await DisplayAlert("Mi perfil",
            $"Nombre: {usuario.Nombre}\nRol: {usuario.Rol}",
            "Cerrar");
    }

    private async void OnCerrarSesionTapped(object? sender, TappedEventArgs e)
    {
        // Cerrar el menú primero
        MenuUsuarioDesplegable.IsVisible = false;
        FlechaMenuLabel.Text = "\u25BE";

        bool confirmar = await DisplayAlert("Cerrar sesión",
            "¿Seguro que deseas cerrar sesión?", "Sí", "Cancelar");

        if (!confirmar)
            return;

        _sesion.CerrarSesion();
        Application.Current!.Windows[0].Page = new Views.Login.LoginPage(_sesion);
    }

    private void MostrarUsuario()
    {
        if (_sesion.UsuarioActual != null)
        {
            NombreUsuarioLabel.Text = _sesion.UsuarioActual.Nombre;
            RolUsuarioLabel.Text = _sesion.UsuarioActual.Rol;
        }
    }

    private void ConfigurarMenu()
    {
        foreach (var item in Items)
        {
            item.IsVisible = item.Title switch
            {
                "Dashboard" => _sesion.TienePermiso("Dashboard.Ver"),
                "Equipos" => _sesion.TienePermiso("Equipos.Ver"),
                "Checklist" => _sesion.TienePermiso("Checklist.Ver"),
                "Mantenimientos" => _sesion.TienePermiso("Mantenimientos.Ver"),
                "Reportes" => _sesion.TienePermiso("Reportes.Ver"),
                "Usuarios" => _sesion.TienePermiso("Usuarios.Ver"),
                "Configuración" => _sesion.TienePermiso("Configuracion.Ver"),
                _ => true
            };
        }
    }
}
