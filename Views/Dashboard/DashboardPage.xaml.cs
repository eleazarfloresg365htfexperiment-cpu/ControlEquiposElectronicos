using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Dashboard;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _viewModel;
    private readonly IPermisoService _permisoService;

    public DashboardPage(DashboardViewModel viewModel, IPermisoService permisoService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _permisoService = permisoService;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.ActualizarVisibilidad();
        AplicarPermisos();
        await _viewModel.CargarAsync();
    }

    private void AplicarPermisos()
    {
        TarjetaEquipos.IsVisible = _permisoService.PuedeVerModulo("Equipos");
        TarjetaMantenimientos.IsVisible = _permisoService.PuedeVerModulo("Mantenimientos");
        TarjetaReportes.IsVisible = _permisoService.PuedeVerModulo("Reportes");
        TarjetaChecklist.IsVisible = _permisoService.PuedeVerModulo("Checklist");

        AccesoEquipos.IsVisible = _permisoService.PuedeVerModulo("Equipos");
        AccesoMantenimientos.IsVisible = _permisoService.PuedeVerModulo("Mantenimientos");
        AccesoReportes.IsVisible = _permisoService.PuedeVerModulo("Reportes");
        AccesoChecklist.IsVisible = _permisoService.PuedeVerModulo("Checklist");
        AccesoConsultas.IsVisible = _viewModel.MostrarConsultas;
        AccesoUsuarios.IsVisible = _permisoService.PuedeVerModulo("Usuarios");
        AccesoConfiguracion.IsVisible = _permisoService.PuedeVerModulo("Configuracion");
    }

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

    private async void OnEquiposTapped(object? sender, TappedEventArgs e)
        => await NavegarSeguro("//Equipos");

    private async void OnMantenimientosTapped(object? sender, TappedEventArgs e)
        => await NavegarSeguro("//Mantenimientos");

    private async void OnReportesTapped(object? sender, TappedEventArgs e)
        => await NavegarSeguro("//Reportes");

    private async void OnChecklistTapped(object? sender, TappedEventArgs e)
        => await NavegarSeguro("//Checklist");

    private async void OnConsultasTapped(object? sender, TappedEventArgs e)
        => await NavegarSeguro("//Consultas");

    private async void OnUsuariosTapped(object? sender, TappedEventArgs e)
        => await NavegarSeguro("//Usuarios");

    private async void OnConfiguracionTapped(object? sender, TappedEventArgs e)
        => await NavegarSeguro("//Configuracion");
}
