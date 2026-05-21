using ControlEquiposElectronicos.ViewModels.Equipos;
using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class DetalleEquipoPage : ContentPage
{
    private readonly DetalleEquiposViewModel _viewModel;

    public DetalleEquipoPage(DetalleEquiposViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        BtnVolver.Clicked += async (s, e) => await Shell.Current.GoToAsync("..");

        BtnEditar.Clicked += async (s, e) =>
        {
            await DisplayAlert("Editar", "Todavia no la hago esperate", "OK");
        };

        BtnCambiarEstado.Clicked += async (s, e) =>
        {
            await DisplayAlert("Cambiar estado", "Todavia no la hago esperate", "OK");
        };

        BtnReclasificar.Clicked += async (s, e) =>
        {
            await DisplayAlert("Reclasificar", "Todavia no la hago esperate", "OK");
        };

        BtnDesactivar.Clicked += async (s, e) =>
        {
            bool confirmar = await DisplayAlert("Desactivar", "¿Estás seguro de desactivar este equipo?", "Sí", "No");
            if (confirmar)
            {
                await DisplayAlert("Desactivado", "Equipo desactivado correctamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.Equipo == null)
            await _viewModel.CargarEquipoAsync(0);
    }
}