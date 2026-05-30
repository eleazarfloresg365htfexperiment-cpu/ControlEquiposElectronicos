using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public partial class PlantillasChecklistPage : ContentPage
{
    private readonly PlantillasChecklistViewModel _viewModel;

    public PlantillasChecklistPage(PlantillasChecklistViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }
}
