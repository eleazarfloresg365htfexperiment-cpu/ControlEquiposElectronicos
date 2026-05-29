using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Mantenimientos;

public partial class MantenimientosPage : ContentPage
{
    private readonly MantenimientosViewModel _viewModel;

    public MantenimientosPage(MantenimientosViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarMantenimientosAsync();
    }
}