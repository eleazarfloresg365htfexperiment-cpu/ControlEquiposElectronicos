using System.Collections.ObjectModel;
using System.Windows.Input;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class PlantillaChecklistItem
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public string TipoEquipo { get; init; } = string.Empty;
    public string? CategoriaEquipo { get; init; }
    public bool Activo { get; init; }
    public int TotalAspectos { get; init; }
    public ObservableCollection<PlantillaAspectoItem> Aspectos { get; init; } = new();
}

public class PlantillaAspectoItem
{
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public int Orden { get; init; }
    public bool EsObligatorio { get; init; }
    public string ObligatorioTexto => EsObligatorio ? "Obligatorio" : "Opcional";
}

public class PlantillasChecklistViewModel : BaseViewModel
{
    private readonly IChecklistApiService _checklistApiService;
    private string? _mensajeError;

    public PlantillasChecklistViewModel(IChecklistApiService checklistApiService)
    {
        _checklistApiService = checklistApiService;
        Title = "Plantillas de checklist";
        Plantillas = new ObservableCollection<PlantillaChecklistItem>();
        CargarCommand = new AsyncRelayCommand(CargarAsync);
        VolverCommand = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(".."));
    }

    public ObservableCollection<PlantillaChecklistItem> Plantillas { get; }

    public string? MensajeError
    {
        get => _mensajeError;
        set
        {
            _mensajeError = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TieneError));
        }
    }

    public bool TieneError => !string.IsNullOrWhiteSpace(MensajeError);

    public ICommand CargarCommand { get; }
    public ICommand VolverCommand { get; }

    public async Task CargarAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            MensajeError = null;
            Plantillas.Clear();

            var plantillas = await _checklistApiService.ObtenerPlantillasAsync();
            foreach (var plantilla in plantillas.OrderBy(p => p.TipoEquipo).ThenBy(p => p.Nombre))
            {
                var item = new PlantillaChecklistItem
                {
                    Id = plantilla.Id,
                    Nombre = plantilla.Nombre,
                    Descripcion = plantilla.Descripcion,
                    TipoEquipo = plantilla.TipoEquipo,
                    CategoriaEquipo = plantilla.CategoriaEquipo,
                    Activo = plantilla.Activo,
                    TotalAspectos = plantilla.Items.Count(i => i.Activo)
                };

                foreach (var aspecto in plantilla.Items
                             .Where(i => i.Activo)
                             .OrderBy(i => i.Orden))
                {
                    item.Aspectos.Add(new PlantillaAspectoItem
                    {
                        Nombre = aspecto.Nombre,
                        Descripcion = aspecto.Descripcion,
                        Orden = aspecto.Orden,
                        EsObligatorio = aspecto.EsObligatorio
                    });
                }

                Plantillas.Add(item);
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"No se pudieron cargar las plantillas: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

}
