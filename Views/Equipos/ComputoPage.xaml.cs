using ControlEquiposElectronicos.DTOs.Equipos;
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

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarEquiposAsync();
    }

    private async void OnRegistrarPCTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("RegistrarEquipoPage");
    }

    private async void OnRegistrarPeifericoTapped(object sender, TappedEventArgs e)
    {
        string tipo = await DisplayActionSheet(
            "Tipo de periférico", "Cancelar", null,
            "Mouse", "Teclado", "Monitor", "Bocina", "Cañonera");

        if (tipo != null && tipo != "Cancelar")
            await Shell.Current.GoToAsync("RegistrarEquipoPage");
    }

    private async void OnAsignarPeifericoTapped(object sender, TappedEventArgs e)
    {
        var pcs = _viewModel.Equipos
            .Where(eq => eq.Tipo.Equals("PC", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!pcs.Any())
        {
            await DisplayAlert("Sin PCs", "No hay PCs registradas en el sistema.", "OK");
            return;
        }

        string seleccionPc = await DisplayActionSheet(
            "Selecciona la PC", "Cancelar", null,
            pcs.Select(p => $"{p.Codigo} - {p.Nombre}").ToArray());

        if (seleccionPc == null || seleccionPc == "Cancelar") return;

        var pcSeleccionada = pcs.FirstOrDefault(p => $"{p.Codigo} - {p.Nombre}" == seleccionPc);
        if (pcSeleccionada == null) return;

        var perifericos = _viewModel.Equipos
            .Where(eq => !eq.Tipo.Equals("PC", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!perifericos.Any())
        {
            await DisplayAlert("Sin periféricos",
                "No hay periféricos registrados en el inventario.", "OK");
            return;
        }

        string seleccionPer = await DisplayActionSheet(
            "Selecciona el periférico", "Cancelar", null,
            perifericos.Select(p => $"{p.Codigo} - {p.Nombre} ({p.Tipo})").ToArray());

        if (seleccionPer == null || seleccionPer == "Cancelar") return;

        var perifericoSeleccionado = perifericos
            .FirstOrDefault(p => $"{p.Codigo} - {p.Nombre} ({p.Tipo})" == seleccionPer);
        if (perifericoSeleccionado == null) return;

        string obs = await DisplayPromptAsync(
            "Observaciones",
            "Escribe una nota (opcional):",
            placeholder: "Ej. Inicio de semestre 2026",
            maxLength: 200,
            accept: "Asignar",
            cancel: "Cancelar") ?? string.Empty;

        try
        {
            var dto = new AsignarPerifericoDto
            {
                EquipoPrincipalId = pcSeleccionada.Id,
                PerifericoId = perifericoSeleccionado.Id,
                Observaciones = obs
            };

            var resultado = await _viewModel.AsignarPerifericoAsync(dto);

            if (resultado != null)
                await DisplayAlert("✅ Asignado",
                    $"{perifericoSeleccionado.Nombre} asignado a {pcSeleccionada.Nombre} correctamente.", "OK");
            else
                await DisplayAlert("Error",
                    "No se pudo registrar la asignación. Verifica que no esté ya asignado.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }

    private async void OnEditarPCTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is int id)
            await Shell.Current.GoToAsync($"DetalleEquipoPage?equipoId={id}");
    }

    private async void OnCambiarEstadoPCTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int id) return;

        string estado = await DisplayActionSheet(
            "Cambiar estado", "Cancelar", null,
            "Funcional",
            "No funcional",
            "En mantenimiento",
            "Dado de baja");

        if (estado == null || estado == "Cancelar") return;

        var guardado = await _viewModel.CambiarEstadoEquipoAsync(id, estado);

        if (guardado)
            await DisplayAlert("✅ Éxito", $"Estado cambiado a {estado}", "OK");
        else
            await DisplayAlert("Error", "No se pudo guardar el estado en el servidor.", "OK");
    }

    private async void OnReclasificarPCTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Reclasificar", "Función de reclasificación próximamente.", "OK");
    }
}