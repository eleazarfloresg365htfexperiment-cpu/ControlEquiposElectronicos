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
        Routing.RegisterRoute("NuevoChecklist", typeof(Views.Checklist.NuevoChecklistPage));
        Routing.RegisterRoute("RevisionEquipoChecklist", typeof(Views.Checklist.RevisionEquipoChecklistPage));
        Routing.RegisterRoute("HistorialChecklist", typeof(Views.Checklist.HistorialChecklistPage));
        Routing.RegisterRoute("PlantillasChecklist", typeof(Views.Checklist.PlantillasChecklistPage));

        MostrarUsuario();
        ConfigurarMenu();
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
        // Si la sesión no trae permisos (ambiente de pruebas), se muestra el menú completo.
        if (_sesion.UsuarioActual == null || _sesion.UsuarioActual.Permisos.Count == 0)
        {
            foreach (var item in Items)
                item.IsVisible = true;
            return;
        }

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
