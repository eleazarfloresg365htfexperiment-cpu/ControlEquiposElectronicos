using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Views.Mantenimientos;
using ControlEquiposElectronicos.Views.Reportes;

namespace ControlEquiposElectronicos;

public partial class AppShell : Shell
{
    private readonly SesionService _sesion;

    public AppShell(SesionService sesion)
    {
        InitializeComponent();
        _sesion = sesion;

        // Registrar rutas de navegación
        Routing.RegisterRoute("MantenimientoFormulario", typeof(MantenimientoFormularioPage));
        Routing.RegisterRoute("ReporteFallaFormulario", typeof(ReporteFallaFormularioPage));

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
                "Mantenimientos" => _sesion.TienePermiso("Mantenimientos.Ver"),
                "Reportes" => _sesion.TienePermiso("Reportes.Ver"),
                "Usuarios" => _sesion.TienePermiso("Usuarios.Ver"),
                "Configuración" => _sesion.TienePermiso("Configuracion.Ver"),
                _ => true
            };
        }
    }
}