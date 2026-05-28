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

        MostrarUsuario();
        ConfigurarMenu();
        AgregarBotonCerrarSesion();
    }

    // Botón de "Cerrar sesión" en la barra superior, visible para TODOS los roles
    private void AgregarBotonCerrarSesion()
    {
        var cerrarSesion = new ToolbarItem
        {
            Text = "Cerrar sesión",
            IconImageSource = new FontImageSource
            {
                FontFamily = "FontAwesome",
                Glyph = "\uf2f5",
                Color = Colors.White
            },
            Order = ToolbarItemOrder.Primary,
            Priority = 0
        };

        cerrarSesion.Clicked += OnCerrarSesionGlobal;
        ToolbarItems.Add(cerrarSesion);
    }

    private async void OnCerrarSesionGlobal(object? sender, EventArgs e)
    {
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
                "Reportes" => _sesion.TienePermiso("Reportes.Ver"),
                "Usuarios" => _sesion.TienePermiso("Usuarios.Ver"),
                "Configuración" => _sesion.TienePermiso("Configuracion.Ver"),
                _ => true
            };
        }
    }
}
