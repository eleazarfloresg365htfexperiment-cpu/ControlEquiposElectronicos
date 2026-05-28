using ControlEquiposElectronicos.ViewModels;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class RevisionEquipoChecklistViewModel : BaseViewModel
{
    private readonly IChecklistApiService _checklistApiService;
    private ChecklistTecnicoDto? _checklist;
    private EquipoRevisionItemViewModel? _equipoSeleccionado;
    private bool _crearReportesAutomaticos = true;

    public ObservableCollection<EquipoRevisionItemViewModel> Equipos { get; } = new();
    public List<string> EstadosDisponibles { get; } = new() { "Correcto", "Con problema", "No aplica", "No revisado" };

    public int ChecklistId => _checklist?.Id ?? 0;
    public string Ubicacion => _checklist?.Ubicacion ?? string.Empty;
    public string Tecnico => _checklist?.Tecnico ?? string.Empty;
    public string EstadoChecklist => _checklist?.EstadoChecklist ?? "Pendiente";

    public string? ObservacionesGenerales
    {
        get => _checklist?.ObservacionesGenerales;
        set
        {
            if (_checklist == null)
                return;

            _checklist.ObservacionesGenerales = value;
            OnPropertyChanged();
        }
    }

    public EquipoRevisionItemViewModel? EquipoSeleccionado
    {
        get => _equipoSeleccionado;
        set
        {
            _equipoSeleccionado = value;
            OnPropertyChanged();
        }
    }

    public bool CrearReportesAutomaticos
    {
        get => _crearReportesAutomaticos;
        set
        {
            _crearReportesAutomaticos = value;
            OnPropertyChanged();
        }
    }

    public RevisionEquipoChecklistViewModel(IChecklistApiService checklistApiService)
    {
        _checklistApiService = checklistApiService;
        Title = "Revisión de equipo";
    }

    public async Task<bool> CargarChecklistAsync(int checklistId)
    {
        if (IsBusy)
            return false;

        try
        {
            IsBusy = true;
            _checklist = await _checklistApiService.ObtenerPorIdAsync(checklistId);

            Equipos.Clear();
            if (_checklist == null)
                return false;

            foreach (var equipo in _checklist.EquiposRevisados.OrderBy(e => e.NombreEquipo))
            {
                Equipos.Add(new EquipoRevisionItemViewModel(equipo));
            }

            EquipoSeleccionado = Equipos.FirstOrDefault();

            OnPropertyChanged(nameof(ChecklistId));
            OnPropertyChanged(nameof(Ubicacion));
            OnPropertyChanged(nameof(Tecnico));
            OnPropertyChanged(nameof(EstadoChecklist));
            OnPropertyChanged(nameof(ObservacionesGenerales));

            return true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task<bool> GuardarAvanceAsync()
    {
        if (_checklist == null || IsBusy)
            return false;

        try
        {
            IsBusy = true;
            var dto = new GuardarChecklistTecnicoDto
            {
                ObservacionesGenerales = ObservacionesGenerales,
                Equipos = Equipos.Select(e => e.ToDto()).ToList()
            };

            return await _checklistApiService.GuardarAvanceAsync(_checklist.Id, dto);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task<bool> FinalizarAsync()
    {
        if (_checklist == null || IsBusy)
            return false;

        try
        {
            IsBusy = true;
            return await _checklistApiService.FinalizarAsync(_checklist.Id, new FinalizarChecklistTecnicoDto
            {
                ObservacionesGenerales = ObservacionesGenerales,
                CrearReportesFallaAutomaticos = CrearReportesAutomaticos
            });
        }
        finally
        {
            IsBusy = false;
        }
    }
}

public class EquipoRevisionItemViewModel : BaseViewModel
{
    public int ChecklistTecnicoEquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;
    public string TipoEquipo { get; set; } = string.Empty;
    public string PlantillaChecklist { get; set; } = string.Empty;
    public ObservableCollection<DetalleRevisionItemViewModel> Detalles { get; } = new();

    private string _resultadoGeneral = "No revisado";
    public string ResultadoGeneral
    {
        get => _resultadoGeneral;
        set
        {
            _resultadoGeneral = value;
            OnPropertyChanged();
        }
    }

    private string? _observacionesEquipo;
    public string? ObservacionesEquipo
    {
        get => _observacionesEquipo;
        set
        {
            _observacionesEquipo = value;
            OnPropertyChanged();
        }
    }

    public EquipoRevisionItemViewModel(ChecklistTecnicoEquipoDto equipo)
    {
        ChecklistTecnicoEquipoId = equipo.Id;
        CodigoEquipo = equipo.CodigoEquipo;
        NombreEquipo = equipo.NombreEquipo;
        TipoEquipo = equipo.TipoEquipo;
        PlantillaChecklist = equipo.PlantillaChecklist;
        ResultadoGeneral = string.IsNullOrWhiteSpace(equipo.ResultadoGeneral) ? "No revisado" : equipo.ResultadoGeneral;
        ObservacionesEquipo = equipo.ObservacionesEquipo;

        foreach (var detalle in equipo.Detalles.OrderBy(d => d.Orden))
        {
            Detalles.Add(new DetalleRevisionItemViewModel(detalle));
        }
    }

    public GuardarChecklistEquipoDto ToDto()
    {
        return new GuardarChecklistEquipoDto
        {
            ChecklistTecnicoEquipoId = ChecklistTecnicoEquipoId,
            ResultadoGeneral = ResultadoGeneral,
            ObservacionesEquipo = ObservacionesEquipo,
            Detalles = Detalles.Select(d => d.ToDto()).ToList()
        };
    }
}

public class DetalleRevisionItemViewModel : BaseViewModel
{
    public int ChecklistTecnicoDetalleId { get; set; }
    public string Item { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool EsObligatorio { get; set; }

    private string _estadoRevision = "No revisado";
    public string EstadoRevision
    {
        get => _estadoRevision;
        set
        {
            _estadoRevision = value;
            OnPropertyChanged();
        }
    }

    private string? _observacion;
    public string? Observacion
    {
        get => _observacion;
        set
        {
            _observacion = value;
            OnPropertyChanged();
        }
    }

    public DetalleRevisionItemViewModel(ChecklistTecnicoDetalleDto detalle)
    {
        ChecklistTecnicoDetalleId = detalle.Id;
        Item = detalle.Item;
        Descripcion = detalle.DescripcionItem;
        EsObligatorio = detalle.EsObligatorio;
        EstadoRevision = string.IsNullOrWhiteSpace(detalle.EstadoRevision) ? "No revisado" : detalle.EstadoRevision;
        Observacion = detalle.Observacion;
    }

    public GuardarChecklistDetalleDto ToDto()
    {
        return new GuardarChecklistDetalleDto
        {
            ChecklistTecnicoDetalleId = ChecklistTecnicoDetalleId,
            EstadoRevision = EstadoRevision,
            Observacion = Observacion
        };
    }
}
