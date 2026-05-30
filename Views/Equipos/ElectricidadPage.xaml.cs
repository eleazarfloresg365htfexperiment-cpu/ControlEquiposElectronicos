using ControlEquiposElectronicos.ViewModels.Equipos;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class ElectricidadPage : ContentPage
{
    public ElectricidadPage(ElectricidadViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ElectricidadViewModel vm)
            await vm.CargarAsync();
    }
}
