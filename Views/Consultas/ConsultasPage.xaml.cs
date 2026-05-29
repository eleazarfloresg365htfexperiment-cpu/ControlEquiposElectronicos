using ControlEquiposElectronicos.ViewModels.Consultas;

namespace ControlEquiposElectronicos.Views.Consultas;

public partial class ConsultasPage : ContentPage
{
    private readonly ConsultasViewModel _viewModel;

    public ConsultasPage(ConsultasViewModel viewModel)
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