using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class HistorialChecklistViewModel : BaseViewModel
{
    private readonly IChecklistApiService _checklistApiService;

    public ObservableCollection<ChecklistTecnicoDto> Historial { get; } = new();

    public HistorialChecklistViewModel(IChecklistApiService checklistApiService)
    {
        _checklistApiService = checklistApiService;
        Title = "Historial de checklist";
    }

    public async Task CargarAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            Historial.Clear();

            var datos = await _checklistApiService.ObtenerTodosAsync();
            foreach (var checklist in datos.OrderByDescending(c => c.FechaInicio))
            {
                Historial.Add(checklist);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
