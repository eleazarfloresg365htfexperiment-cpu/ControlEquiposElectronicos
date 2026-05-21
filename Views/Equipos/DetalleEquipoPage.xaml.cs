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
            if (_viewModel.Equipo == null) return;
            await Shell.Current.GoToAsync($"RegistrarEquipoPage?equipoId={_viewModel.Equipo.Id}");
        };

        BtnCambiarEstado.Clicked += async (s, e) =>
        {
            string accion = await DisplayActionSheet(
                "Cambiar estado", "Cancelar", null,
                "Activo", "En mantenimiento", "Dañado", "Inactivo");

            if (accion != null && accion != "Cancelar")
            {
                await _viewModel.CambiarEstadoAsync(accion);
                await DisplayAlert("Éxito", $"Estado cambiado a {accion}", "OK");
            }
        };

        BtnReclasificar.Clicked += async (s, e) =>
        {
            if (_viewModel.Equipo == null) return;
            bool confirmar = await DisplayAlert("Desactivar",
                $"¿Estás seguro de desactivar {_viewModel.Equipo.Nombre}?", "Si", "No");
            if (confirmar)
            {
                var resultado = await _viewModel.DesactivarEquipoAsync();
                if (resultado)
                {
                    await DisplayAlert("Éxito", "Equipo desactivado correctamente", "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
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