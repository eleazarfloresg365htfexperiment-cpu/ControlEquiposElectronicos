using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Dashboard;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _viewModel;
    private readonly SesionService _sesion;

    public DashboardPage()
    {
        InitializeComponent();

        var services = IPlatformApplication.Current!.Services;
        var equipoApi = services.GetRequiredService<IEquipoApiService>();
        var mantenimientoApi = services.GetRequiredService<IMantenimientoApiService>();
        var reporteApi = services.GetRequiredService<IReporteFallaApiService>();
        _sesion = services.GetRequiredService<SesionService>();

        _viewModel = new DashboardViewModel(equipoApi, mantenimientoApi, reporteApi);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        AplicarPermisos();
        await _viewModel.CargarAsync();
    }

    // Oculta las tarjetas y accesos rápidos a los que el usuario no tiene permiso
    private void AplicarPermisos()
    {
        // Tarjetas de resumen
        TarjetaEquipos.IsVisible = _sesion.TienePermiso("Equipos.Ver");
        TarjetaMantenimientos.IsVisible = _sesion.TienePermiso("Mantenimientos.Ver");
        TarjetaReportes.IsVisible = _sesion.TienePermiso("Reportes.Ver");

        // Accesos rápidos
        AccesoEquipos.IsVisible = _sesion.TienePermiso("Equipos.Ver");
        AccesoMantenimientos.IsVisible = _sesion.TienePermiso("Mantenimientos.Ver");
        AccesoReportes.IsVisible = _sesion.TienePermiso("Reportes.Ver");
        AccesoUsuarios.IsVisible = _sesion.TienePermiso("Usuarios.Ver");
        AccesoConfiguracion.IsVisible = _sesion.TienePermiso("Configuracion.Ver");
    }

    private async void OnEquiposTapped(object? sender, TappedEventArgs e)
    {
        await NavegarSeguro("//Equipos");
    }

    private async void OnMantenimientosTapped(object? sender, TappedEventArgs e)
    {
        await NavegarSeguro("//Mantenimientos");
    }

    private async void OnReportesTapped(object? sender, TappedEventArgs e)
    {
        await NavegarSeguro("//Reportes");
    }

    private async void OnUsuariosTapped(object? sender, TappedEventArgs e)
    {
        await NavegarSeguro("//Usuarios");
    }

    private async void OnConfiguracionTapped(object? sender, TappedEventArgs e)
    {
        await NavegarSeguro("//Configuracion");
    }

    // Navega con seguridad: si la ruta no existe (porque el menú la ocultó por permisos), no crashea
    private async Task NavegarSeguro(string ruta)
    {
        try
        {
            await Shell.Current.GoToAsync(ruta);
        }
        catch
        {
            await DisplayAlert("Acceso restringido",
                "No tienes permiso para acceder a esta sección.", "Entendido");
        }
    }
}
