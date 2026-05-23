using ControlEquiposElectronicos.Services;

namespace ControlEquiposElectronicos;

public partial class AppShell : Shell
{
    private readonly SesionService _sesion;

    public AppShell(SesionService sesion)
    {
        InitializeComponent();
        _sesion = sesion;

        ConfigurarMenu();
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