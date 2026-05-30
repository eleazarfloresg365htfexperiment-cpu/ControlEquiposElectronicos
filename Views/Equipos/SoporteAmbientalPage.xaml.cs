using ControlEquiposElectronicos.ViewModels.Equipos;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class SoporteAmbientalPage : ContentPage
{
    public SoporteAmbientalPage(SoporteAmbientalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SoporteAmbientalViewModel vm)
            await vm.CargarAsync();
    }
}
