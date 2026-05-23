using System.Collections.ObjectModel;
using System.Windows.Input;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class ChecklistResumenItem
{
    public int Id { get; init; }
    public string Ubicacion { get; init; } = string.Empty;
    public string Tecnico { get; init; } = string.Empty;
    public string EstadoChecklist { get; init; } = string.Empty;
    public DateTime FechaInicio { get; init; }
    public int EquiposTotal { get; init; }
    public int EquiposRevisados { get; init; }
    public string ProgresoTexto => $"{EquiposRevisados}/{EquiposTotal} equipos";
}

public class ChecklistViewModel : BaseViewModel
{
    private readonly IChecklistApiService _checklistApiService;
    private string? _mensajeError;

    public ChecklistViewModel(IChecklistApiService checklistApiService)
    {
        _checklistApiService = checklistApiService;
        Title = "Checklist técnico";

        ChecklistsEnProceso = new ObservableCollection<ChecklistResumenItem>();

        CargarCommand = new AsyncRelayCommand(CargarAsync);
        NuevoCommand = new AsyncRelayCommand(IrANuevoAsync);
        HistorialCommand = new AsyncRelayCommand(IrAHistorialAsync);
        PlantillasCommand = new AsyncRelayCommand(IrAPlantillasAsync);
        AbrirChecklistCommand = new AsyncRelayCommand<ChecklistResumenItem>(async item =>
        {
            if (item != null)
                await AbrirChecklistAsync(item.Id);
        });
    }

    public ObservableCollection<ChecklistResumenItem> ChecklistsEnProceso { get; }

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
    public ICommand NuevoCommand { get; }
    public ICommand HistorialCommand { get; }
    public ICommand PlantillasCommand { get; }
    public ICommand AbrirChecklistCommand { get; }

    public async Task CargarAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            MensajeError = null;
            ChecklistsEnProceso.Clear();

            var todos = await _checklistApiService.ObtenerTodosAsync();
            foreach (var checklist in todos.Where(c => c.EstadoChecklist == "En proceso"))
            {
                ChecklistsEnProceso.Add(MapResumen(checklist));
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"No se pudieron cargar los checklist: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static ChecklistResumenItem MapResumen(ChecklistTecnicoDto checklist)
    {
        var total = checklist.EquiposRevisados.Count;
        var revisados = checklist.EquiposRevisados.Count(e =>
            e.ResultadoGeneral is not ("Pendiente" or "No revisado"));

        return new ChecklistResumenItem
        {
            Id = checklist.Id,
            Ubicacion = checklist.Ubicacion,
            Tecnico = checklist.Tecnico,
            EstadoChecklist = checklist.EstadoChecklist,
            FechaInicio = checklist.FechaInicio,
            EquiposTotal = total,
            EquiposRevisados = revisados
        };
    }

    private static async Task IrANuevoAsync() =>
        await Shell.Current.GoToAsync(nameof(Views.Checklist.NuevoChecklistPage));

    private static async Task IrAHistorialAsync() =>
        await Shell.Current.GoToAsync(nameof(Views.Checklist.HistorialChecklistPage));

    private static async Task IrAPlantillasAsync() =>
        await Shell.Current.GoToAsync(nameof(Views.Checklist.PlantillasChecklistPage));

    private static async Task AbrirChecklistAsync(int checklistId) =>
        await Shell.Current.GoToAsync(
            $"{nameof(Views.Checklist.NuevoChecklistPage)}?ChecklistId={checklistId}");
}
