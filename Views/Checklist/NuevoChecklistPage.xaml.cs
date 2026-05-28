using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public partial class NuevoChecklistPage : ContentPage
{
    private readonly NuevoChecklistViewModel _viewModel;

    public NuevoChecklistPage()
    {
        InitializeComponent();

        var services = IPlatformApplication.Current!.Services;
        var checklistApi = services.GetRequiredService<IChecklistApiService>();
        var catalogoApi = services.GetRequiredService<ICatalogoApiService>();
        var sesion = services.GetRequiredService<SesionService>();

        _viewModel = new NuevoChecklistViewModel(catalogoApi, checklistApi, sesion);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.InicializarDesdeSesion();
        await _viewModel.CargarCatalogosAsync();
    }

    private async void OnPrepararClicked(object? sender, EventArgs e)
    {
        var (ok, mensaje) = await _viewModel.PrepararAsync();
        if (!ok)
        {
            await DisplayAlertAsync("No se pudo preparar", mensaje, "Entendido");
            return;
        }

        await DisplayAlertAsync("Ubicación lista",
            mensaje,
            "Continuar");
    }

    private async void OnIniciarClicked(object? sender, EventArgs e)
    {
        var (checklist, error) = await _viewModel.IniciarConDetalleAsync();

        if (checklist == null)
        {
            await DisplayAlertAsync("No se pudo iniciar", error ?? "Error desconocido.", "Entendido");
            return;
        }

        await DisplayAlertAsync("Checklist iniciado",
            $"Revisión #{checklist.Id} creada para {checklist.Ubicacion}.",
            "Continuar");

        await Shell.Current.GoToAsync($"RevisionEquipoChecklist?checklistId={checklist.Id}");
    }
}
