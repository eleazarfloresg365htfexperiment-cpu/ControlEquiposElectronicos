using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Dashboard;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel _viewModel;

    public DashboardPage()
    {
        InitializeComponent();

        var services = IPlatformApplication.Current!.Services;
        var equipoApi = services.GetRequiredService<IEquipoApiService>();
        var mantenimientoApi = services.GetRequiredService<IMantenimientoApiService>();
        var reporteApi = services.GetRequiredService<IReporteFallaApiService>();

        _viewModel = new DashboardViewModel(equipoApi, mantenimientoApi, reporteApi);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }

    private async void OnEquiposTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//Equipos");
    }

    private async void OnMantenimientosTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//Mantenimientos");
    }

    private async void OnReportesTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//Reportes");
    }

    private async void OnUsuariosTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//Usuarios");
    }

    private async void OnConfiguracionTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//Configuracion");
    }
}