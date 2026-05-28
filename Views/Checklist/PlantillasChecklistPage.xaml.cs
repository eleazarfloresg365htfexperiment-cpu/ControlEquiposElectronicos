using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public partial class PlantillasChecklistPage : ContentPage
{
    private readonly PlantillasChecklistViewModel _viewModel;

    public PlantillasChecklistPage()
    {
        InitializeComponent();

        var services = IPlatformApplication.Current!.Services;
        var checklistApi = services.GetRequiredService<IChecklistApiService>();
        var catalogoApi = services.GetRequiredService<ICatalogoApiService>();
        _viewModel = new PlantillasChecklistViewModel(checklistApi, catalogoApi);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }

    private async void OnCrearPlantillaClicked(object? sender, EventArgs e)
    {
        var (ok, mensaje) = await _viewModel.CrearPlantillaAsync();
        await DisplayAlert(ok ? "Plantilla" : "No se pudo crear", mensaje, "Entendido");
    }
}
