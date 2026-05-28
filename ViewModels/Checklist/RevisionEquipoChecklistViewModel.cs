using System.Collections.ObjectModel;
using System.Windows.Input;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class AspectoRevisionItem : BaseViewModel
{
    private string _estadoRevision = "No revisado";
    private string? _observacion;

    public int ChecklistTecnicoDetalleId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public bool EsObligatorio { get; init; }
    public int Orden { get; init; }

    public IReadOnlyList<string> EstadosDisponibles { get; } = ChecklistEstados.Revision;

    public string EstadoRevision
    {
        get => _estadoRevision;
        set
        {
            _estadoRevision = value;
            OnPropertyChanged();
        }
    }

    public string? Observacion
    {
        get => _observacion;
        set
        {
            _observacion = value;
            OnPropertyChanged();
        }
    }
}

public class RevisionEquipoChecklistViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IChecklistApiService _checklistApiService;

    private int _checklistId;
    private int _checklistTecnicoEquipoId;
    private string _codigoEquipo = string.Empty;
    private string _nombreEquipo = string.Empty;
    private string? _observacionesEquipo;
    private string? _mensajeError;
    private string? _mensajeInfo;

    public RevisionEquipoChecklistViewModel(IChecklistApiService checklistApiService)
    {
        _checklistApiService = checklistApiService;
        Title = "Revisión de equipo";
        Aspectos = new ObservableCollection<AspectoRevisionItem>();
        GuardarAvanceCommand = new AsyncRelayCommand(GuardarAvanceAsync);
        VolverCommand = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(".."));
    }

    public ObservableCollection<AspectoRevisionItem> Aspectos { get; }

    public string CodigoEquipo
    {
        get => _codigoEquipo;
        set
        {
            _codigoEquipo = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(EncabezadoEquipo));
        }
    }

    public string NombreEquipo
    {
        get => _nombreEquipo;
        set
        {
            _nombreEquipo = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(EncabezadoEquipo));
        }
    }

    public string EncabezadoEquipo => $"{CodigoEquipo} — {NombreEquipo}";

    public string? ObservacionesEquipo
    {
        get => _observacionesEquipo;
        set
        {
            _observacionesEquipo = value;
            OnPropertyChanged();
        }
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

    public string? MensajeInfo
    {
        get => _mensajeInfo;
        set
        {
            _mensajeInfo = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TieneInfo));
        }
    }

    public bool TieneError => !string.IsNullOrWhiteSpace(MensajeError);
    public bool TieneInfo => !string.IsNullOrWhiteSpace(MensajeInfo);

    public ICommand GuardarAvanceCommand { get; }
    public ICommand VolverCommand { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ChecklistId", out var checklistId) &&
            int.TryParse(checklistId?.ToString(), out var idChecklist))
        {
            _checklistId = idChecklist;
        }

        if (query.TryGetValue("ChecklistTecnicoEquipoId", out var equipoId) &&
            int.TryParse(equipoId?.ToString(), out var idEquipo))
        {
            _checklistTecnicoEquipoId = idEquipo;
        }
    }

    public async Task CargarAsync()
    {
        if (_checklistId <= 0 || _checklistTecnicoEquipoId <= 0)
        {
            MensajeError = "Parámetros de navegación inválidos.";
            return;
        }

        try
        {
            IsBusy = true;
            MensajeError = null;
            Aspectos.Clear();

            var checklist = await _checklistApiService.ObtenerPorIdAsync(_checklistId);
            if (checklist == null)
            {
                MensajeError = "No se encontró el checklist.";
                return;
            }

            var equipo = checklist.EquiposRevisados
                .FirstOrDefault(e => e.Id == _checklistTecnicoEquipoId);

            if (equipo == null)
            {
                MensajeError = "El equipo no pertenece a este checklist.";
                return;
            }

            CodigoEquipo = equipo.CodigoEquipo;
            NombreEquipo = equipo.NombreEquipo;
            ObservacionesEquipo = equipo.ObservacionesEquipo;
            Title = EncabezadoEquipo;

            foreach (var detalle in equipo.Detalles.OrderBy(d => d.Orden))
            {
                Aspectos.Add(new AspectoRevisionItem
                {
                    ChecklistTecnicoDetalleId = detalle.Id,
                    Nombre = detalle.Item,
                    Descripcion = detalle.DescripcionItem,
                    EsObligatorio = detalle.EsObligatorio,
                    Orden = detalle.Orden,
                    EstadoRevision = detalle.EstadoRevision,
                    Observacion = detalle.Observacion
                });
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al cargar revisión: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task GuardarAvanceAsync()
    {
        if (_checklistId <= 0 || _checklistTecnicoEquipoId <= 0)
            return;

        var obligatoriosPendientes = Aspectos
            .Where(a => a.EsObligatorio && a.EstadoRevision == "No revisado")
            .Select(a => a.Nombre)
            .ToList();

        if (obligatoriosPendientes.Any())
        {
            MensajeError = "Revisa los aspectos obligatorios pendientes: " +
                           string.Join(", ", obligatoriosPendientes);
            return;
        }

        try
        {
            IsBusy = true;
            MensajeError = null;
            MensajeInfo = null;

            var estados = Aspectos.Select(a => a.EstadoRevision);
            var resultado = ChecklistEstados.CalcularResultadoGeneral(estados);

            var dto = new GuardarChecklistTecnicoDto
            {
                Equipos =
                {
                    new GuardarChecklistEquipoDto
                    {
                        ChecklistTecnicoEquipoId = _checklistTecnicoEquipoId,
                        ResultadoGeneral = resultado,
                        ObservacionesEquipo = ObservacionesEquipo,
                        Detalles = Aspectos.Select(a => new GuardarChecklistDetalleDto
                        {
                            ChecklistTecnicoDetalleId = a.ChecklistTecnicoDetalleId,
                            EstadoRevision = a.EstadoRevision,
                            Observacion = a.Observacion
                        }).ToList()
                    }
                }
            };

            var guardado = await _checklistApiService.GuardarAvanceAsync(_checklistId, dto);
            if (!guardado)
            {
                MensajeError = "No se pudo guardar el avance.";
                return;
            }

            MensajeInfo = "Avance guardado correctamente.";
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al guardar: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
