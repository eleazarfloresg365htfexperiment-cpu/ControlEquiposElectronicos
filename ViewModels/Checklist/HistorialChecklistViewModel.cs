using System.Collections.ObjectModel;
using System.Windows.Input;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class HistorialChecklistViewModel : BaseViewModel
{
    private readonly IChecklistApiService _checklistApiService;
    private string? _mensajeError;

    public HistorialChecklistViewModel(IChecklistApiService checklistApiService)
    {
        _checklistApiService = checklistApiService;
        Title = "Historial de checklist";
        Historial = new ObservableCollection<ChecklistResumenItem>();
        CargarCommand = new AsyncRelayCommand(CargarAsync);
    }

    public ObservableCollection<ChecklistResumenItem> Historial { get; }

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

    public async Task CargarAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            MensajeError = null;
            Historial.Clear();

            var todos = await _checklistApiService.ObtenerTodosAsync();
            foreach (var checklist in todos
                         .Where(c => c.EstadoChecklist is "Finalizado" or "Cancelado")
                         .OrderByDescending(c => c.FechaFinalizacion ?? c.FechaInicio))
            {
                Historial.Add(new ChecklistResumenItem
                {
                    Id = checklist.Id,
                    Ubicacion = checklist.Ubicacion,
                    Tecnico = checklist.Tecnico,
                    EstadoChecklist = checklist.EstadoChecklist,
                    FechaInicio = checklist.FechaInicio,
                    EquiposTotal = checklist.EquiposRevisados.Count,
                    EquiposRevisados = checklist.EquiposRevisados.Count(e =>
                        e.ResultadoGeneral is not ("Pendiente" or "No revisado"))
                });
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"No se pudo cargar el historial: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
