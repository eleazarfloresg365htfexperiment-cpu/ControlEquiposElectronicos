using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class ComputoPage : ContentPage
{
    private readonly EquiposViewModel _viewModel;

    public ComputoPage(EquiposViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        BtnRegistrarPC.Clicked += async (s, e) =>
        {
            await Shell.Current.GoToAsync("RegistrarEquipoPage");
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarEquiposAsync();
    }
}