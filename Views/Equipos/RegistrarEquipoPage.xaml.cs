using ControlEquiposElectronicos.ViewModels.Equipos;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class RegistrarEquipoPage : ContentPage
{
    private readonly EquipoFormularioViewModel _viewModel;

    public RegistrarEquipoPage(EquipoFormularioViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        BtnGuardar.Clicked += async (s, e) =>
        {
            var resultado = await _viewModel.GuardarEquiposAsync();
            if (resultado)
            {
                await DisplayAlert("Éxito", "Equipo registrado correctamente", "OK");
                await Shell.Current.GoToAsync("..");
            }
        };
        BtnLimpiar.Clicked += async (s, e) => _viewModel.LimpiarFormulario();
        BtnCancelar.Clicked += async (s, e) => await Shell.Current.GoToAsync("..");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarCatalogosAsync();
    }
}
