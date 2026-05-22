using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class EquiposPage : ContentPage
{
    private readonly EquiposViewModel _viewModel;

    public EquiposPage(EquiposViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            await _viewModel.CargarEquiposAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnRegistrarEquipoTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("RegistrarEquipoPage");
    }

    private async void OnEditarEquipoTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Editar", "Selecciona un equipo de la lista para editar.", "OK");
    }

    private async void OnDesactivarTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Desactivar", "Selecciona un equipo de la lista para desactivar.", "OK");
    }

    private async void OnCambiarEstadoTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Cambiar estado", "Selecciona un equipo de la lista para cambiar su estado.", "OK");
    }

    private async void OnReclasificarTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Reclasificar", "Selecciona un equipo de la lista para reclasificar.", "OK");
    }

    private async void OnEquipoSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not ControlEquiposElectronicos.DTOs.Equipos.EquipoListadoDto equipo)
            return;

        ListaEquipos.SelectedItem = null;
        await Shell.Current.GoToAsync($"DetalleEquipoPage?equipoId={equipo.Id}");
    }
}