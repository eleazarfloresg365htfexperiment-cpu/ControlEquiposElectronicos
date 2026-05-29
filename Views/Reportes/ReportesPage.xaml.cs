using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Reportes;

public partial class ReportesPage : ContentPage
{
    private readonly ReportesViewModel _viewModel;

    public ReportesPage(ReportesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarReportesAsync();
    }
}