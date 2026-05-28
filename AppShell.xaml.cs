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

        ConfigurarMenu();

        // Rutas de navegación - Equipos (Suarlin)
        Routing.RegisterRoute("RegistrarEquipoPage", typeof(Views.Equipos.RegistrarEquipoPage));
        Routing.RegisterRoute("DetalleEquipoPage", typeof(Views.Equipos.DetalleEquipoPage));
        Routing.RegisterRoute("ComputoPage", typeof(Views.Equipos.ComputoPage));
        Routing.RegisterRoute("AuditoriaPage", typeof(AuditoriaPage));
    }

    private void ConfigurarMenu()
    {
        foreach (var item in Items)
        {
            item.IsVisible = true;
        }
    }
}