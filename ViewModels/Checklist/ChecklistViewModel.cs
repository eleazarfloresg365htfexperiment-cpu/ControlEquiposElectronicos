using System.Collections.ObjectModel;
using System.Windows.Input;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class ChecklistResumenItem
{
    public int Id { get; init; }
    public string Ubicacion { get; init; } = string.Empty;
    public string Tecnico { get; init; } = string.Empty;
    public string Responsable => Tecnico;
    public string EstadoChecklist { get; init; } = string.Empty;
    public string EstadoEtiqueta { get; init; } = string.Empty;
    public Color EstadoBadgeColor { get; init; } = Colors.Gray;
    public DateTime FechaInicio { get; init; }
    public int EquiposTotal { get; init; }
    public int EquiposRevisados { get; init; }
    public string EquiposTexto => EquiposTotal == 1 ? "1 equipo" : $"{EquiposTotal} equipos";
    public string ProgresoTexto => $"{EquiposRevisados}/{EquiposTotal} equipos";
    public bool EsEnProceso => EstadoChecklist == "En proceso";
}

public class ChecklistViewModel : BaseViewModel
{
    private readonly IChecklistApiService _checklistApiService;
    private readonly SesionService _sesionService;
    private string? _mensajeError;
    private int _equiposRevisadosHoy;
    private int _checklistCompletados;
    private int _problemasDetectados;
    private int _equiposPendientes;

    public ChecklistViewModel(IChecklistApiService checklistApiService, SesionService sesionService)
    {
        _checklistApiService = checklistApiService;
        _sesionService = sesionService;
        Title = "Checklist técnico";

        Revisiones = new ObservableCollection<ChecklistResumenItem>();

        CargarCommand = new AsyncRelayCommand(CargarAsync);
        NuevoCommand = new AsyncRelayCommand(IrANuevoAsync);
        HistorialCommand = new AsyncRelayCommand(IrAHistorialAsync);
        PlantillasCommand = new AsyncRelayCommand(IrAPlantillasAsync);
        AbrirRevisionCommand = new AsyncRelayCommand<ChecklistResumenItem>(AbrirRevisionItemAsync);
    }

    public ObservableCollection<ChecklistResumenItem> Revisiones { get; }

    public string NombreUsuario => _sesionService.UsuarioActual?.Nombre ?? "Técnico";
    public string RolUsuario => _sesionService.UsuarioActual?.Rol ?? "Técnico de revisión";

    public int EquiposRevisadosHoy
    {
        get => _equiposRevisadosHoy;
        private set { _equiposRevisadosHoy = value; OnPropertyChanged(); }
    }

    public int ChecklistCompletados
    {
        get => _checklistCompletados;
        private set { _checklistCompletados = value; OnPropertyChanged(); }
    }

    public int ProblemasDetectados
    {
        get => _problemasDetectados;
        private set { _problemasDetectados = value; OnPropertyChanged(); }
    }

    public int EquiposPendientes
    {
        get => _equiposPendientes;
        private set { _equiposPendientes = value; OnPropertyChanged(); }
    }

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
    public ICommand AbrirRevisionCommand { get; }

    public async Task CargarAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            MensajeError = null;
            Revisiones.Clear();

            var todos = await _checklistApiService.ObtenerTodosAsync();

            ActualizarEstadisticas(todos);

            foreach (var checklist in todos
                         .Where(c => c.EstadoChecklist != "Cancelado")
                         .OrderByDescending(c => c.FechaInicio)
                         .Take(20))
            {
                Revisiones.Add(MapResumen(checklist));
            }

            if (todos.Count == 0)
            {
                MensajeError =
                    "No hay datos de checklist. Asegúrate de que la API esté ejecutándose y la base de datos inicializada.";
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"No se pudieron cargar los checklist: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(NombreUsuario));
            OnPropertyChanged(nameof(RolUsuario));
        }
    }

    private void ActualizarEstadisticas(List<ChecklistTecnicoDto> todos)
    {
        var hoy = DateTime.UtcNow.Date;

        EquiposRevisadosHoy = todos
            .SelectMany(c => c.EquiposRevisados)
            .Count(e => e.FechaRevision.HasValue && e.FechaRevision.Value.Date == hoy);

        ChecklistCompletados = todos.Count(c => c.EstadoChecklist == "Finalizado");

        ProblemasDetectados = todos
            .SelectMany(c => c.EquiposRevisados)
            .Count(TieneProblemasEquipo);

        EquiposPendientes = todos
            .Where(c => c.EstadoChecklist == "En proceso")
            .SelectMany(c => c.EquiposRevisados)
            .Count(e => e.ResultadoGeneral is "Pendiente" or "No revisado");
    }

    private static bool TieneProblemasEquipo(ChecklistTecnicoEquipoDto equipo) =>
        equipo.ResultadoGeneral == "Revisado con problemas" ||
        equipo.Detalles.Any(d => d.EstadoRevision == "Con problema");

    private static bool TieneProblemas(ChecklistTecnicoDto checklist) =>
        checklist.EquiposRevisados.Any(TieneProblemasEquipo);

    private static ChecklistResumenItem MapResumen(ChecklistTecnicoDto checklist)
    {
        var total = checklist.EquiposRevisados.Count;
        var revisados = checklist.EquiposRevisados.Count(e =>
            e.ResultadoGeneral is not ("Pendiente" or "No revisado"));

        var (etiqueta, color) = ObtenerEstadoVisual(checklist);

        return new ChecklistResumenItem
        {
            Id = checklist.Id,
            Ubicacion = checklist.Ubicacion,
            Tecnico = checklist.Tecnico,
            EstadoChecklist = checklist.EstadoChecklist,
            EstadoEtiqueta = etiqueta,
            EstadoBadgeColor = color,
            FechaInicio = checklist.FechaInicio,
            EquiposTotal = total,
            EquiposRevisados = revisados
        };
    }

    private static (string Etiqueta, Color Color) ObtenerEstadoVisual(ChecklistTecnicoDto checklist)
    {
        if (TieneProblemas(checklist))
            return ("Problemas detectados", Color.FromArgb("#EB5757"));

        return checklist.EstadoChecklist switch
        {
            "Finalizado" => ("Completado", Color.FromArgb("#27AE60")),
            "En proceso" => ("En progreso", Color.FromArgb("#F2994A")),
            "Cancelado" => ("Cancelado", Color.FromArgb("#919191")),
            _ => (checklist.EstadoChecklist, Color.FromArgb("#4A61DD"))
        };
    }

    private static async Task IrANuevoAsync() =>
        await Shell.Current.GoToAsync(nameof(Views.Checklist.NuevoChecklistPage));

    private static async Task IrAHistorialAsync() =>
        await Shell.Current.GoToAsync(nameof(Views.Checklist.HistorialChecklistPage));

    private static async Task IrAPlantillasAsync() =>
        await Shell.Current.GoToAsync(nameof(Views.Checklist.PlantillasChecklistPage));

    public async Task AbrirRevisionItemAsync(ChecklistResumenItem? item)
    {
        if (item == null)
            return;

        if (item.EsEnProceso)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(Views.Checklist.NuevoChecklistPage)}?ChecklistId={item.Id}");
            return;
        }

        await Shell.Current.GoToAsync(nameof(Views.Checklist.HistorialChecklistPage));
    }
}
