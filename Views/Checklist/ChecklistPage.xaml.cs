using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public partial class ChecklistPage : ContentPage
{
    private readonly ChecklistViewModel _viewModel;
    private readonly SesionService _sesion;

    public ChecklistPage()
    {
        InitializeComponent();

        var services = IPlatformApplication.Current!.Services;
        var checklistApiService = services.GetRequiredService<IChecklistApiService>();
        _sesion = services.GetRequiredService<SesionService>();

        _viewModel = new ChecklistViewModel(checklistApiService);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        MostrarUsuario();
        await _viewModel.CargarAsync();
    }

    private void MostrarUsuario()
    {
        if (_sesion.UsuarioActual == null)
            return;

        NombreUsuarioLabel.Text = _sesion.UsuarioActual.Nombre;
        RolUsuarioLabel.Text = _sesion.UsuarioActual.Rol;
    }

    private async void OnNuevoChecklistClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("NuevoChecklist");
    }

    private async void OnHistorialClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("HistorialChecklist");
    }

    private async void OnPlantillasClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("PlantillasChecklist");
    }

    private void OnNuevoChecklistTapped(object? sender, TappedEventArgs e) => OnNuevoChecklistClicked(sender, EventArgs.Empty);
    private void OnHistorialTapped(object? sender, TappedEventArgs e) => OnHistorialClicked(sender, EventArgs.Empty);
    private void OnPlantillasTapped(object? sender, TappedEventArgs e) => OnPlantillasClicked(sender, EventArgs.Empty);
}
