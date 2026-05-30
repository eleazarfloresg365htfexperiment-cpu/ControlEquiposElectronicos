using ControlEquiposElectronicos.ViewModels.Reportes;

namespace ControlEquiposElectronicos.Views.Reportes;

public partial class ReporteFallaFormularioPage : ContentPage, IQueryAttributable
{
    private readonly ReporteFallaFormularioViewModel _viewModel;

    public ReporteFallaFormularioPage(ReporteFallaFormularioViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _viewModel.ApplyQueryAttributes(query);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InicializarAsync();
    }
}
