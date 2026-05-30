using ControlEquiposElectronicos.ViewModels.Equipos;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class RedPage : ContentPage
{
    public RedPage(RedViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is RedViewModel vm)
            await vm.CargarAsync();
    }
}
