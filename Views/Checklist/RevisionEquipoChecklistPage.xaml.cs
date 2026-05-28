using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

[QueryProperty(nameof(ChecklistId), "checklistId")]
public partial class RevisionEquipoChecklistPage : ContentPage
{
    private readonly RevisionEquipoChecklistViewModel _viewModel;
    private bool _yaCargado;

    public string ChecklistId { get; set; } = "0";

    public RevisionEquipoChecklistPage()
    {
        InitializeComponent();

        var services = IPlatformApplication.Current!.Services;
        var checklistApi = services.GetRequiredService<IChecklistApiService>();

        _viewModel = new RevisionEquipoChecklistViewModel(checklistApi);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_yaCargado)
            return;

        if (!int.TryParse(ChecklistId, out var checklistId) || checklistId <= 0)
        {
            await DisplayAlert("Checklist inválido",
                "No se recibió un identificador de checklist válido.",
                "Entendido");
            return;
        }

        var cargado = await _viewModel.CargarChecklistAsync(checklistId);
        _yaCargado = true;

        if (!cargado)
        {
            await DisplayAlert("No se pudo cargar",
                "No fue posible obtener los datos del checklist.",
                "Entendido");
        }
    }

    private async void OnGuardarAvanceClicked(object? sender, EventArgs e)
    {
        var guardado = await _viewModel.GuardarAvanceAsync();
        await DisplayAlert(
            guardado ? "Avance guardado" : "No se pudo guardar",
            guardado
                ? "Los cambios de revisión fueron guardados correctamente."
                : "No fue posible guardar el avance del checklist.",
            "Entendido");
    }

    private async void OnFinalizarClicked(object? sender, EventArgs e)
    {
        var confirmar = await DisplayAlert(
            "Finalizar checklist",
            "Al finalizar, se cerrará la revisión actual.",
            "Finalizar",
            "Cancelar");

        if (!confirmar)
            return;

        var finalizado = await _viewModel.FinalizarAsync();
        if (!finalizado)
        {
            await DisplayAlert("No se pudo finalizar",
                "No fue posible completar el checklist.",
                "Entendido");
            return;
        }

        await DisplayAlert("Checklist finalizado",
            "El checklist se finalizó correctamente.",
            "Entendido");

        await Shell.Current.GoToAsync("//Checklist");
    }
}
