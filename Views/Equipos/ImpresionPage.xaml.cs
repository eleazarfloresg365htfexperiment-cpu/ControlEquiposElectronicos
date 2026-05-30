using ControlEquiposElectronicos.ViewModels.Equipos;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class ImpresionPage : ContentPage
{
    public ImpresionPage(ImpresionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ImpresionViewModel vm)
            await vm.CargarAsync();
    }
}
