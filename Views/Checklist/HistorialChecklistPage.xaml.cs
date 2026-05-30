using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public partial class HistorialChecklistPage : ContentPage
{
    private readonly HistorialChecklistViewModel _viewModel;

    public HistorialChecklistPage() : this(ServiceHelper.GetRequiredService<HistorialChecklistViewModel>()) { }

    public HistorialChecklistPage(HistorialChecklistViewModel viewModel)
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
