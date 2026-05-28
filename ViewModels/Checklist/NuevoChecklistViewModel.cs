using System.Collections.ObjectModel;
using System.Windows.Input;
using ControlEquiposElectronicos.DTOs.Catalogos;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class EquipoChecklistCardItem
{
    public int ChecklistTecnicoEquipoId { get; init; }
    public int EquipoId { get; init; }
    public string CodigoEquipo { get; init; } = string.Empty;
    public string NombreEquipo { get; init; } = string.Empty;
    public string TipoEquipo { get; init; } = string.Empty;
    public string ResultadoGeneral { get; init; } = string.Empty;
    public string ProgresoAspectos { get; init; } = string.Empty;
    public Color EstadoColor { get; init; } = Colors.Gray;
}

public class NuevoChecklistViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IChecklistApiService _checklistApiService;
    private readonly ICatalogoApiService _catalogoApiService;
    private readonly SesionService _sesionService;

    private CatalogoItemDto? _ubicacionSeleccionada;
    private PrepararChecklistUbicacionDto? _preparacion;
    private ChecklistTecnicoDto? _checklistActivo;
    private string? _observacionesGenerales;
    private string? _mensajeError;
    private string? _mensajeInfo;
    private int? _checklistIdPendiente;

    public NuevoChecklistViewModel(
        IChecklistApiService checklistApiService,
        ICatalogoApiService catalogoApiService,
        SesionService sesionService)
    {
        _checklistApiService = checklistApiService;
        _catalogoApiService = catalogoApiService;
        _sesionService = sesionService;
        Title = "Nuevo checklist";

        Ubicaciones = new ObservableCollection<CatalogoItemDto>();
        EquiposPreview = new ObservableCollection<EquipoChecklistPreparadoDto>();
        EquiposCards = new ObservableCollection<EquipoChecklistCardItem>();

        CargarUbicacionesCommand = new AsyncRelayCommand(CargarUbicacionesAsync);
        PrepararCommand = new AsyncRelayCommand(PrepararAsync, () => UbicacionSeleccionada != null && !ChecklistIniciado);
        IniciarCommand = new AsyncRelayCommand(IniciarAsync, () => Preparacion != null && !ChecklistIniciado);
        AbrirEquipoCommand = new AsyncRelayCommand<EquipoChecklistCardItem>(AbrirEquipoAsync);
        FinalizarCommand = new AsyncRelayCommand(FinalizarAsync, () => ChecklistIniciado);
        VolverCommand = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(".."));
    }

    public ObservableCollection<CatalogoItemDto> Ubicaciones { get; }
    public ObservableCollection<EquipoChecklistPreparadoDto> EquiposPreview { get; }
    public ObservableCollection<EquipoChecklistCardItem> EquiposCards { get; }

    public CatalogoItemDto? UbicacionSeleccionada
    {
        get => _ubicacionSeleccionada;
        set
        {
            _ubicacionSeleccionada = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PuedePreparar));
            (PrepararCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            (IniciarCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public PrepararChecklistUbicacionDto? Preparacion
    {
        get => _preparacion;
        set
        {
            _preparacion = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ResumenPreparacion));
            OnPropertyChanged(nameof(MostrarPreparacion));
            (IniciarCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public ChecklistTecnicoDto? ChecklistActivo
    {
        get => _checklistActivo;
        set
        {
            _checklistActivo = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ChecklistIniciado));
            OnPropertyChanged(nameof(MostrarSeleccionUbicacion));
            OnPropertyChanged(nameof(TituloChecklistActivo));
            (PrepararCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            (IniciarCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            (FinalizarCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public string? ObservacionesGenerales
    {
        get => _observacionesGenerales;
        set
        {
            _observacionesGenerales = value;
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
    public bool ChecklistIniciado => ChecklistActivo != null;
    public bool MostrarSeleccionUbicacion => !ChecklistIniciado;
    public bool MostrarPreparacion => Preparacion != null && !ChecklistIniciado;
    public bool PuedePreparar => UbicacionSeleccionada != null && !ChecklistIniciado;

    public string ResumenPreparacion =>
        Preparacion == null
            ? string.Empty
            : $"{Preparacion.TotalEquipos} equipos en {Preparacion.Ubicacion}";

    public string TituloChecklistActivo =>
        ChecklistActivo == null
            ? string.Empty
            : $"Checklist #{ChecklistActivo.Id} — {ChecklistActivo.Ubicacion}";

    public ICommand CargarUbicacionesCommand { get; }
    public ICommand PrepararCommand { get; }
    public ICommand IniciarCommand { get; }
    public ICommand AbrirEquipoCommand { get; }
    public ICommand FinalizarCommand { get; }
    public ICommand VolverCommand { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ChecklistId", out var value) &&
            int.TryParse(value?.ToString(), out var checklistId))
        {
            _checklistIdPendiente = checklistId;
        }
    }

    public async Task InicializarAsync()
    {
        await CargarUbicacionesAsync();

        if (_checklistIdPendiente.HasValue)
            await CargarChecklistActivoAsync(_checklistIdPendiente.Value);
    }

    private async Task CargarUbicacionesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            MensajeError = null;
            Ubicaciones.Clear();

            var ubicaciones = await _catalogoApiService.ObtenerUbicacionesAsync();
            foreach (var ubicacion in ubicaciones.Where(u => u.Activo).OrderBy(u => u.Nombre))
                Ubicaciones.Add(ubicacion);
        }
        catch (Exception ex)
        {
            MensajeError = $"No se pudieron cargar ubicaciones: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task PrepararAsync()
    {
        if (UbicacionSeleccionada == null)
            return;

        try
        {
            IsBusy = true;
            MensajeError = null;
            MensajeInfo = null;
            EquiposPreview.Clear();
            Preparacion = null;

            var preparado = await _checklistApiService.PrepararPorUbicacionAsync(UbicacionSeleccionada.Id);
            if (preparado == null)
            {
                MensajeError = "No se pudo preparar el checklist para la ubicación seleccionada.";
                return;
            }

            Preparacion = preparado;
            foreach (var equipo in preparado.Equipos)
                EquiposPreview.Add(equipo);

            var sinPlantilla = preparado.Equipos.Count(e => !e.TienePlantilla);
            MensajeInfo = sinPlantilla > 0
                ? $"Se encontraron {preparado.TotalEquipos} equipos. {sinPlantilla} no tienen plantilla y no se incluirán al iniciar."
                : $"Listo para iniciar con {preparado.Equipos.Count(e => e.TienePlantilla)} equipos con plantilla.";
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al preparar: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task IniciarAsync()
    {
        if (UbicacionSeleccionada == null)
            return;

        var tecnicoId = _sesionService.UsuarioActual?.UsuarioId ?? 0;
        if (tecnicoId <= 0)
        {
            MensajeError = "No hay un técnico en sesión. Vuelve a iniciar sesión.";
            return;
        }

        try
        {
            IsBusy = true;
            MensajeError = null;

            var dto = new IniciarChecklistTecnicoDto
            {
                UbicacionId = UbicacionSeleccionada.Id,
                TecnicoId = tecnicoId,
                ObservacionesGenerales = ObservacionesGenerales
            };

            var checklist = await _checklistApiService.IniciarAsync(dto);
            if (checklist == null)
            {
                MensajeError = "No se pudo iniciar el checklist. Verifica plantillas activas en la ubicación.";
                return;
            }

            ChecklistActivo = checklist;
            ActualizarTarjetasEquipos();
            MensajeInfo = $"Checklist iniciado con {checklist.EquiposRevisados.Count} equipos.";
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al iniciar: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CargarChecklistActivoAsync(int checklistId)
    {
        try
        {
            IsBusy = true;
            MensajeError = null;

            var checklist = await _checklistApiService.ObtenerPorIdAsync(checklistId);
            if (checklist == null)
            {
                MensajeError = "El checklist no existe.";
                return;
            }

            if (checklist.EstadoChecklist != "En proceso")
            {
                MensajeError = "Este checklist ya no está en proceso.";
                return;
            }

            ChecklistActivo = checklist;
            ObservacionesGenerales = checklist.ObservacionesGenerales;
            ActualizarTarjetasEquipos();
            Title = TituloChecklistActivo;
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al cargar checklist: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ActualizarTarjetasEquipos()
    {
        EquiposCards.Clear();
        if (ChecklistActivo == null)
            return;

        foreach (var equipo in ChecklistActivo.EquiposRevisados)
        {
            EquiposCards.Add(new EquipoChecklistCardItem
            {
                ChecklistTecnicoEquipoId = equipo.Id,
                EquipoId = equipo.EquipoId,
                CodigoEquipo = equipo.CodigoEquipo,
                NombreEquipo = equipo.NombreEquipo,
                TipoEquipo = equipo.TipoEquipo,
                ResultadoGeneral = equipo.ResultadoGeneral,
                ProgresoAspectos = ChecklistEstados.ResumenProgresoEquipo(
                    equipo.Detalles.Select(d => d.EstadoRevision)),
                EstadoColor = ColorForResultado(equipo.ResultadoGeneral)
            });
        }
    }

    private static Color ColorForResultado(string resultado) => resultado switch
    {
        "Revisado correctamente" => Colors.Green,
        "Revisado con problemas" => Colors.Red,
        "Revisado con observaciones" => Colors.Orange,
        "No revisado" => Colors.Gray,
        _ => Colors.SteelBlue
    };

    private async Task AbrirEquipoAsync(EquipoChecklistCardItem? card)
    {
        if (card == null || ChecklistActivo == null)
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(Views.Checklist.RevisionEquipoChecklistPage)}" +
            $"?ChecklistId={ChecklistActivo.Id}" +
            $"&ChecklistTecnicoEquipoId={card.ChecklistTecnicoEquipoId}");
    }

    public async Task RecargarChecklistActivoAsync()
    {
        if (ChecklistActivo == null)
            return;

        await CargarChecklistActivoAsync(ChecklistActivo.Id);
    }

    private async Task FinalizarAsync()
    {
        if (ChecklistActivo == null)
            return;

        var confirmar = await Shell.Current.DisplayAlertAsync(
            "Finalizar checklist",
            "¿Deseas finalizar el checklist? Se crearán reportes de falla automáticos por aspectos con problema.",
            "Sí, finalizar",
            "Cancelar");

        if (!confirmar)
            return;

        try
        {
            IsBusy = true;
            MensajeError = null;

            var dto = new FinalizarChecklistTecnicoDto
            {
                ObservacionesGenerales = ObservacionesGenerales,
                CrearReportesFallaAutomaticos = true
            };

            var ok = await _checklistApiService.FinalizarAsync(ChecklistActivo.Id, dto);
            if (!ok)
            {
                MensajeError = "No se pudo finalizar el checklist.";
                return;
            }

            await Shell.Current.DisplayAlertAsync(
                "Checklist finalizado",
                "El checklist se cerró correctamente.",
                "Aceptar");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al finalizar: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
