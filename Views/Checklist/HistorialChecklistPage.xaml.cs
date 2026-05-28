using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public partial class HistorialChecklistPage : ContentPage
{
    private readonly HistorialChecklistViewModel _viewModel;

    public HistorialChecklistPage()
    {
        InitializeComponent();

        var services = IPlatformApplication.Current!.Services;
        var checklistApi = services.GetRequiredService<IChecklistApiService>();
        _viewModel = new HistorialChecklistViewModel(checklistApi);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }
}
