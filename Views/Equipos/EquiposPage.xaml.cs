using ControlEquiposElectronicos.DTOs.Equipos;
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

    private async void OnActualizarListaTapped(object sender, EventArgs e)
    {
        await _viewModel.CargarEquiposAsync();
        await DisplayAlert("Actualizado", "Lista de equipos actualizada.", "OK");
    }

    private async void OnVerComputoTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ComputoPage");
    }

    private void OnFiltrarActivosTapped(object sender, EventArgs e)
    {
        _viewModel.TextoBusqueda = string.Empty;
        _viewModel.FiltrarPorEstado("Funcional");
    }

    private void OnFiltrarInactivosTapped(object sender, EventArgs e)
    {
        _viewModel.TextoBusqueda = string.Empty;
        _viewModel.FiltrarPorEstado("mantenimiento");
    }

    private async void OnEditarFilaTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is int id)
            await Shell.Current.GoToAsync($"DetalleEquipoPage?equipoId={id}");
    }

    private async void OnMasOpcionesTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int id) return;

        string accion = await DisplayActionSheet(
            "Opciones del equipo", "Cancelar", null,
            "🔄 Cambiar estado",
            "🏷️ Reclasificar",
            "🗑️ Desactivar");

        if (accion == null || accion == "Cancelar") return;

        if (accion.Contains("Cambiar estado"))
        {
            string estado = await DisplayActionSheet(
                "Cambiar estado", "Cancelar", null,
                "Activo", "En mantenimiento", "Dañado", "Inactivo");
            if (estado != null && estado != "Cancelar")
                await DisplayAlert("Estado", $"Estado cambiado a {estado}", "OK");
        }
        else if (accion.Contains("Reclasificar"))
        {
            await DisplayAlert("Reclasificar", "Función próximamente.", "OK");
        }
        else if (accion.Contains("Desactivar"))
        {
            bool confirmar = await DisplayAlert("Desactivar",
                "¿Estás segura de desactivar este equipo?", "Sí", "No");
            if (confirmar)
                await DisplayAlert("Desactivado", "Equipo desactivado correctamente.", "OK");
        }
    }

    private async void OnEquipoSeleccionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not EquipoListadoDto equipo)
            return;
        ListaEquipos.SelectedItem = null;
        await Shell.Current.GoToAsync($"DetalleEquipoPage?equipoId={equipo.Id}");
    }
}