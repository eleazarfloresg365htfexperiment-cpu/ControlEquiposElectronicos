using ControlEquiposElectronicos.ViewModels.Mantenimientos;

namespace ControlEquiposElectronicos.Views.Mantenimientos;

public partial class MantenimientoFormularioPage : ContentPage
{
    private readonly MantenimientoFormularioViewModel _viewModel;

    public MantenimientoFormularioPage(MantenimientoFormularioViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}