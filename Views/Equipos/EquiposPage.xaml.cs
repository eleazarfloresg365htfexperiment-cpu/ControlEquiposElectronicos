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
        var filtrados = _viewModel.ObtenerPorEstados("En mantenimiento", "No funcional", "Dado de baja");
        _viewModel.Equipos.Clear();
        foreach (var eq in filtrados)
            _viewModel.Equipos.Add(eq);
    }

    private async void OnEditarFilaTapped(object sender, EventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is EquipoListadoDto equipo)
            await Shell.Current.GoToAsync($"DetalleEquipoPage?equipoId={equipo.Id}");
    }

    private async void OnMasOpcionesTapped(object sender, EventArgs e)
    {
        if (sender is not BindableObject bindable) return;
        if (bindable.BindingContext is not EquipoListadoDto equipo) return;

        string accion = await DisplayActionSheet(
            $"Equipo: {equipo.Codigo}", "Cancelar", null,
            "🔄 Cambiar estado",
            "🏷️ Reclasificar",
            "🗑️ Desactivar");

        if (accion == null || accion == "Cancelar") return;

        if (accion.Contains("Cambiar estado"))
        {
            string estado = await DisplayActionSheet(
            "Cambiar estado", "Cancelar", null,
            "Funcional",
            "No funcional",
            "En mantenimiento",
            "Dado de baja");
            
            if (estado != null && estado != "Cancelar")
            {
                _viewModel.ActualizarEstadoVisual(equipo.Id, estado);
                await DisplayAlert("Estado", $"Estado cambiado a {estado}", "OK");
                await _viewModel.CargarEquiposAsync();
            }
        }
        else if (accion.Contains("Reclasificar"))
        {
            await DisplayAlert("Reclasificar", "Función próximamente.", "OK");
        }
        else if (accion.Contains("Desactivar"))
        {
            bool confirmar = await DisplayAlert("Desactivar",
                $"¿Estás segura de desactivar {equipo.Codigo}?", "Sí", "No");
            if (confirmar)
            {
                _viewModel.ActualizarEstadoVisual(equipo.Id, "Inactivo");
                await DisplayAlert("Desactivado", "Equipo desactivado correctamente.", "OK");
                await _viewModel.CargarEquiposAsync();
            }
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