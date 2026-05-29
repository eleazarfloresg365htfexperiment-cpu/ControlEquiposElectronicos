using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Views.Equipos;

namespace ControlEquiposElectronicos;

public partial class AppShell : Shell
{
    private readonly SesionService _sesion;

    public AppShell(SesionService sesion)
    {
        InitializeComponent();
        _sesion = sesion;

        // ── Rutas registradas ──────────────────────────────────────────────────
        Routing.RegisterRoute("PermisosRol", typeof(Views.Configuracion.PermisosRolPage));
        Routing.RegisterRoute("RegistroUsuario", typeof(Views.Login.RegistroUsuarioPage));
        Routing.RegisterRoute("RolesUsuario", typeof(Views.Configuracion.RolesUsuarioPage));
        Routing.RegisterRoute("PerfilUsuario", typeof(Views.Configuracion.PerfilUsuarioPage)); // ← NUEVO

        MostrarUsuario();
        ConfigurarMenu();

        // Rutas de navegación - Equipos (Suarlin)
        Routing.RegisterRoute("RegistrarEquipoPage", typeof(Views.Equipos.RegistrarEquipoPage));
        Routing.RegisterRoute("DetalleEquipoPage", typeof(Views.Equipos.DetalleEquipoPage));
        Routing.RegisterRoute("ComputoPage", typeof(Views.Equipos.ComputoPage));
        Routing.RegisterRoute("AuditoriaPage", typeof(AuditoriaPage));
    }

    // Al tocar el área del usuario en la barra lateral, muestra/oculta el mini-menú
    private void OnMenuUsuarioTapped(object? sender, TappedEventArgs e)
    {
        MenuUsuarioDesplegable.IsVisible = !MenuUsuarioDesplegable.IsVisible;
        FlechaMenuLabel.Text = MenuUsuarioDesplegable.IsVisible ? "\u25B4" : "\u25BE";
    }

    // Navega a la nueva PerfilUsuarioPage en lugar del DisplayAlert anterior
    private async void OnMiPerfilTapped(object? sender, TappedEventArgs e)
    {
        MenuUsuarioDesplegable.IsVisible = false;
        FlechaMenuLabel.Text = "\u25BE";

        await Shell.Current.GoToAsync("PerfilUsuario");
    }

    private async void OnCerrarSesionTapped(object? sender, TappedEventArgs e)
    {
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
                "Mantenimientos" => _sesion.TienePermiso("Mantenimientos.Ver"),
                "Checklist" => _sesion.TienePermiso("Checklist.Ver"),
                "Reportes" => _sesion.TienePermiso("Reportes.Ver"),
                "Usuarios" => _sesion.TienePermiso("Usuarios.Ver"),
                "Configuración" => _sesion.TienePermiso("Configuracion.Ver"),
                _ => true
            };
        }
    }
}
