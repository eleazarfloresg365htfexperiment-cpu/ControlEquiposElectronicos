using ControlEquiposElectronicos.ViewModels.Reportes;

namespace ControlEquiposElectronicos.Views.Reportes;

public partial class ReporteFallaFormularioPage : ContentPage
{
    private readonly ReporteFallaFormularioViewModel _viewModel;

    public ReporteFallaFormularioPage(ReporteFallaFormularioViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}