using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.ViewModels;
using ControlEquiposElectronicos.ViewModels.Equipos;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class EquiposPage : ContentPage
{
    private readonly EquiposViewModel _viewModel;
    private readonly RedViewModel _redViewModel;
    private readonly ImpresionViewModel _impresionViewModel;
    private readonly ElectricidadViewModel _electricidadViewModel;
    private readonly SoporteAmbientalViewModel _ambientalViewModel;
    private Border? _tabActivo;

    private View[] _contenidos = null!;
    private Border[] _tabs = null!;

    public EquiposPage(
        EquiposViewModel viewModel,
        RedViewModel redViewModel,
        ImpresionViewModel impresionViewModel,
        ElectricidadViewModel electricidadViewModel,
        SoporteAmbientalViewModel ambientalViewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _redViewModel = redViewModel;
        _impresionViewModel = impresionViewModel;
        _electricidadViewModel = electricidadViewModel;
        _ambientalViewModel = ambientalViewModel;

        BindingContext = _viewModel;
        ContenidoRed.BindingContext = _redViewModel;
        ContenidoImpresion.BindingContext = _impresionViewModel;
        ContenidoElectricidad.BindingContext = _electricidadViewModel;
        ContenidoSoporteAmbiental.BindingContext = _ambientalViewModel;

        _contenidos = [ContenidoGeneral, ContenidoRed, ContenidoImpresion, ContenidoElectricidad, ContenidoSoporteAmbiental];
        _tabs = [TabGeneral, TabRed, TabImpresion, TabElectricidad, TabSoporteAmbiental];
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            await _viewModel.CargarEquiposAsync();
            await _redViewModel.CargarAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void MostrarTab(int index)
    {
        for (int i = 0; i < _contenidos.Length; i++)
            _contenidos[i].IsVisible = i == index;

        for (int i = 0; i < _tabs.Length; i++)
        {
            bool activo = i == index;
            _tabs[i].BackgroundColor = activo ? Color.FromArgb("#512BD4") : Colors.White;
            _tabs[i].Stroke = activo ? Colors.Transparent : Color.FromArgb("#E2E6F0");
            _tabs[i].StrokeThickness = activo ? 0 : 1;
            if (_tabs[i].Content is HorizontalStackLayout hsl)
                foreach (var child in hsl.Children)
                    if (child is Label lbl)
                        lbl.TextColor = activo ? Colors.White
                            : (lbl.FontFamily == "FontAwesome" ? Color.FromArgb("#512BD4") : Color.FromArgb("#4A5270"));
        }

        _ = CargarSubmoduloAsync(index);
    }

    private async Task CargarSubmoduloAsync(int index)
    {
        try
        {
            switch (index)
            {
                case 1: await _redViewModel.CargarAsync(); break;
                case 2: await _impresionViewModel.CargarAsync(); break;
                case 3: await _electricidadViewModel.CargarAsync(); break;
                case 4: await _ambientalViewModel.CargarAsync(); break;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnTabGeneralTapped(object sender, TappedEventArgs e) => MostrarTab(0);
    private void OnTabRedTapped(object sender, TappedEventArgs e) => MostrarTab(1);
    private void OnTabImpresionTapped(object sender, TappedEventArgs e) => MostrarTab(2);
    private void OnTabElectricidadTapped(object sender, TappedEventArgs e) => MostrarTab(3);
    private void OnTabSoporteAmbientalTapped(object sender, TappedEventArgs e) => MostrarTab(4);

    private async void OnRegistrarEquipoTapped(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("RegistrarEquipoPage");

    private async void OnActualizarListaTapped(object sender, EventArgs e)
    {
        await _viewModel.CargarEquiposAsync();
        await DisplayAlert("Actualizado", "Lista de equipos actualizada.", "OK");
    }

    private async void OnVerComputoTapped(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("ComputoPage");

    private void OnFiltrarActivosTapped(object sender, EventArgs e)
    {
        MostrarTab(0);
        _viewModel.TextoBusqueda = string.Empty;
        _viewModel.FiltrarPorEstado("Funcional");
    }

    private void OnFiltrarInactivosTapped(object sender, EventArgs e)
    {
        MostrarTab(0);
        _viewModel.TextoBusqueda = string.Empty;
        var filtrados = _viewModel.ObtenerPorEstados("En mantenimiento", "No funcional", "Dado de baja");
        _viewModel.Equipos.Clear();
        foreach (var eq in filtrados) _viewModel.Equipos.Add(eq);
    }

    private async void OnAuditoriaTapped(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("AuditoriaPage");

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
            "🔄 Cambiar estado", "🗑️ Desactivar");

        if (accion == null || accion == "Cancelar") return;

        if (accion.Contains("Cambiar estado"))
        {
            string estado = await DisplayActionSheet("Cambiar estado", "Cancelar", null,
                "Funcional", "No funcional", "En mantenimiento", "Dado de baja");
            if (estado != null && estado != "Cancelar")
            {
                _viewModel.ActualizarEstadoVisual(equipo.Id, estado);
                await DisplayAlert("Estado", $"Estado cambiado a {estado}", "OK");
                await _viewModel.CargarEquiposAsync();
            }
        }
        else if (accion.Contains("Desactivar"))
        {
            bool confirmar = await DisplayAlert("Desactivar",
                $"¿Desactivar {equipo.Codigo}?", "Sí", "No");
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
        if (e.CurrentSelection.FirstOrDefault() is not EquipoListadoDto equipo) return;
        ListaEquipos.SelectedItem = null;
        await Shell.Current.GoToAsync($"DetalleEquipoPage?equipoId={equipo.Id}");
    }
}
