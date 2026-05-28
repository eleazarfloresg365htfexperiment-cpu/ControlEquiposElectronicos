using ControlEquiposElectronicos.ViewModels;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class ChecklistViewModel : BaseViewModel
{
    private readonly IChecklistApiService _checklistApiService;

    public ObservableCollection<ChecklistTecnicoDto> Checklists { get; } = new();
    public ObservableCollection<EstadoRevisionChecklistItem> EstadoRevisiones { get; } = new();

    private int _equiposRevisadosHoy;
    private int _checklistsCompletados;
    private int _problemasDetectados;
    private int _equiposPendientes;

    public int EquiposRevisadosHoy
    {
        get => _equiposRevisadosHoy;
        set { _equiposRevisadosHoy = value; OnPropertyChanged(); }
    }

    public int ChecklistsCompletados
    {
        get => _checklistsCompletados;
        set { _checklistsCompletados = value; OnPropertyChanged(); }
    }

    public int ProblemasDetectados
    {
        get => _problemasDetectados;
        set { _problemasDetectados = value; OnPropertyChanged(); }
    }

    public int EquiposPendientes
    {
        get => _equiposPendientes;
        set { _equiposPendientes = value; OnPropertyChanged(); }
    }

    public ChecklistViewModel(IChecklistApiService checklistApiService)
    {
        _checklistApiService = checklistApiService;
        Title = "Checklist técnico";
    }

    public async Task CargarAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            Checklists.Clear();

            var datos = await _checklistApiService.ObtenerTodosAsync();
            foreach (var checklist in datos.OrderByDescending(c => c.FechaInicio))
            {
                Checklists.Add(checklist);
            }

            CalcularResumen(datos);
            ConstruirEstadoRevisiones(datos);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void CalcularResumen(List<ChecklistTecnicoDto> datos)
    {
        var hoy = DateTime.Today;
        EquiposRevisadosHoy = datos
            .Where(c => c.FechaInicio.Date == hoy)
            .Sum(c => c.EquiposRevisados?.Count ?? 0);

        ChecklistsCompletados = datos.Count(c =>
            c.EstadoChecklist.Equals("Completado", StringComparison.OrdinalIgnoreCase) ||
            c.EstadoChecklist.Equals("Finalizado", StringComparison.OrdinalIgnoreCase));

        ProblemasDetectados = datos.Sum(c =>
            c.EquiposRevisados?.Count(e =>
                e.ResultadoGeneral.Equals("Con problema", StringComparison.OrdinalIgnoreCase) ||
                e.ResultadoGeneral.Equals("Problemas detectados", StringComparison.OrdinalIgnoreCase)) ?? 0);

        EquiposPendientes = datos.Sum(c =>
            c.EquiposRevisados?.Count(e =>
                e.ResultadoGeneral.Equals("No revisado", StringComparison.OrdinalIgnoreCase) ||
                e.ResultadoGeneral.Equals("Pendiente", StringComparison.OrdinalIgnoreCase)) ?? 0);
    }

    private void ConstruirEstadoRevisiones(List<ChecklistTecnicoDto> datos)
    {
        EstadoRevisiones.Clear();
        foreach (var checklist in datos.OrderByDescending(x => x.FechaInicio).Take(10))
        {
            EstadoRevisiones.Add(new EstadoRevisionChecklistItem
            {
                Ubicacion = checklist.Ubicacion,
                Equipos = $"{checklist.EquiposRevisados.Count} equipos",
                Responsable = checklist.Tecnico,
                Estado = checklist.EstadoChecklist
            });
        }
    }
}

public class EstadoRevisionChecklistItem
{
    public string Ubicacion { get; set; } = string.Empty;
    public string Equipos { get; set; } = string.Empty;
    public string Responsable { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
