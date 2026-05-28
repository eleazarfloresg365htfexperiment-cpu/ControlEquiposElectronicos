using ControlEquiposElectronicos.DTOs.Catalogos;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class NuevoChecklistViewModel : BaseViewModel
{
    private readonly ICatalogoApiService _catalogoApiService;
    private readonly IChecklistApiService _checklistApiService;
    private readonly SesionService _sesion;

    private CatalogoItemDto? _ubicacionSeleccionada;
    private string? _observacionesGenerales;
    private string _tecnicoId = string.Empty;
    private string _tecnicoNombre = "—";
    private string _resumenPreparacion = string.Empty;
    private string _motivoIniciarDeshabilitado = string.Empty;
    private bool _ubicacionPreparada;
    private bool _puedeIniciar;

    public ObservableCollection<CatalogoItemDto> Ubicaciones { get; } = new();
    public ObservableCollection<EquipoChecklistPreparadoDto> EquiposPreparados { get; } = new();

    public CatalogoItemDto? UbicacionSeleccionada
    {
        get => _ubicacionSeleccionada;
        set
        {
            if (_ubicacionSeleccionada == value)
                return;

            _ubicacionSeleccionada = value;
            ReiniciarPreparacion();
            OnPropertyChanged();
            OnPropertyChanged(nameof(PuedePreparar));
            OnPropertyChanged(nameof(PasoActualTexto));
        }
    }

    public string TecnicoId
    {
        get => _tecnicoId;
        private set
        {
            _tecnicoId = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PuedeIniciar));
        }
    }

    public string TecnicoNombre
    {
        get => _tecnicoNombre;
        private set
        {
            _tecnicoNombre = value;
            OnPropertyChanged();
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

    public string ResumenPreparacion
    {
        get => _resumenPreparacion;
        private set
        {
            _resumenPreparacion = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MostrarResumenPreparacion));
        }
    }

    public bool MostrarResumenPreparacion => _ubicacionPreparada && !string.IsNullOrWhiteSpace(ResumenPreparacion);

    public int TotalEquipos { get; private set; }
    public int EquiposConPlantilla { get; private set; }

    public PrepararChecklistUbicacionDto? DatosPreparados { get; private set; }

    public bool PuedePreparar => UbicacionSeleccionada != null && !IsBusy;

    public bool PuedeIniciar
    {
        get => _puedeIniciar;
        private set
        {
            _puedeIniciar = value;
            OnPropertyChanged();
        }
    }

    public string MotivoIniciarDeshabilitado
    {
        get => _motivoIniciarDeshabilitado;
        private set
        {
            _motivoIniciarDeshabilitado = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MostrarMotivoIniciarDeshabilitado));
        }
    }

    public bool MostrarMotivoIniciarDeshabilitado =>
        _ubicacionPreparada && !PuedeIniciar && !string.IsNullOrWhiteSpace(MotivoIniciarDeshabilitado);

    public bool TieneEquiposPreparados => EquiposPreparados.Count > 0;

    private static bool EquipoListoParaIniciar(EquipoChecklistPreparadoDto equipo) =>
        equipo.ListoParaIniciar;

    public string PasoActualTexto =>
        !_ubicacionPreparada
            ? "Paso 1 de 2: elige la ubicación y pulsa «Ver equipos de la ubicación»."
            : EquiposConPlantilla > 0
                ? "Paso 2 de 2: revisa la lista y pulsa «Iniciar checklist» para comenzar la revisión."
                : "No hay equipos con plantilla en esta ubicación. Crea plantillas antes de iniciar.";

    public NuevoChecklistViewModel(
        ICatalogoApiService catalogoApiService,
        IChecklistApiService checklistApiService,
        SesionService sesion)
    {
        _catalogoApiService = catalogoApiService;
        _checklistApiService = checklistApiService;
        _sesion = sesion;
        Title = "Nuevo checklist";
    }

    public void InicializarDesdeSesion()
    {
        if (_sesion.UsuarioActual == null)
            return;

        TecnicoNombre = _sesion.UsuarioActual.Nombre;

        if (_sesion.UsuarioActual.UsuarioId > 0)
        {
            TecnicoId = _sesion.UsuarioActual.UsuarioId.ToString();
            ActualizarEstadoIniciar();
            return;
        }

        TecnicoId = string.Empty;
        TecnicoNombre = $"{TecnicoNombre} (vuelve a iniciar sesión)";
        ActualizarEstadoIniciar();
    }

    public async Task CargarCatalogosAsync()
    {
        if (Ubicaciones.Count > 0)
            return;

        var ubicaciones = await _catalogoApiService.ObtenerUbicacionesAsync();
        Ubicaciones.Clear();
        foreach (var ubicacion in ubicaciones.Where(u => u.Activo))
        {
            Ubicaciones.Add(ubicacion);
        }
    }

    public async Task<(bool Ok, string Mensaje)> PrepararAsync()
    {
        if (UbicacionSeleccionada == null || IsBusy)
            return (false, "Selecciona una ubicación primero.");

        try
        {
            IsBusy = true;
            DatosPreparados = await _checklistApiService.PrepararPorUbicacionAsync(UbicacionSeleccionada.Id);

            EquiposPreparados.Clear();
            if (DatosPreparados == null)
            {
                ReiniciarPreparacion();
                return (false, "No se pudo conectar con la API o la ubicación no existe.");
            }

            foreach (var equipo in DatosPreparados.Equipos.OrderBy(e => e.NombreEquipo))
            {
                EquiposPreparados.Add(equipo);
            }

            TotalEquipos = DatosPreparados.TotalEquipos;
            EquiposConPlantilla = DatosPreparados.Equipos.Count(EquipoListoParaIniciar);
            var sinPlantilla = TotalEquipos - EquiposConPlantilla;

            _ubicacionPreparada = true;
            ResumenPreparacion =
                $"{TotalEquipos} equipo(s) en {DatosPreparados.Ubicacion}. " +
                $"{EquiposConPlantilla} listo(s) para checklist" +
                (sinPlantilla > 0 ? $" · {sinPlantilla} sin plantilla (no se incluirán al iniciar)." : ".");

            OnPropertyChanged(nameof(TotalEquipos));
            OnPropertyChanged(nameof(EquiposConPlantilla));
            OnPropertyChanged(nameof(TieneEquiposPreparados));
            OnPropertyChanged(nameof(PasoActualTexto));
            ActualizarEstadoIniciar();

            if (TotalEquipos == 0)
                return (false, "Esta ubicación no tiene equipos activos registrados.");

            if (EquiposConPlantilla == 0)
                return (false, "Hay equipos, pero ninguno tiene plantilla con aspectos activos. Revisa «Plantillas».");

            return (true, ResumenPreparacion);
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(PuedePreparar));
            ActualizarEstadoIniciar();
        }
    }

    public async Task<(ChecklistTecnicoDto? Checklist, string? Error)> IniciarConDetalleAsync()
    {
        ActualizarEstadoIniciar();

        if (UbicacionSeleccionada == null)
            return (null, "Selecciona una ubicación.");

        if (!_ubicacionPreparada)
            return (null, "Primero pulsa «Ver equipos de la ubicación».");

        if (!int.TryParse(TecnicoId, out var tecnicoId) || tecnicoId <= 0)
            return (null, "No hay técnico válido. Cierra sesión y vuelve a entrar.");

        if (EquiposConPlantilla == 0)
            return (null, MotivoIniciarDeshabilitado);

        try
        {
            IsBusy = true;
            ActualizarEstadoIniciar();

            var (checklist, error) = await _checklistApiService.IniciarConDetalleAsync(new IniciarChecklistTecnicoDto
            {
                UbicacionId = UbicacionSeleccionada.Id,
                TecnicoId = tecnicoId,
                ObservacionesGenerales = ObservacionesGenerales
            });

            if (checklist != null)
                return (checklist, null);

            return (null, ExtraerMensajeApi(error));
        }
        finally
        {
            IsBusy = false;
            ActualizarEstadoIniciar();
        }
    }

    private void ActualizarEstadoIniciar()
    {
        if (!_ubicacionPreparada)
        {
            PuedeIniciar = false;
            MotivoIniciarDeshabilitado = string.Empty;
            return;
        }

        if (!int.TryParse(TecnicoId, out var tecnicoId) || tecnicoId <= 0)
        {
            PuedeIniciar = false;
            MotivoIniciarDeshabilitado = "Vuelve a iniciar sesión para cargar tu ID de técnico.";
            return;
        }

        if (EquiposConPlantilla == 0)
        {
            PuedeIniciar = false;
            MotivoIniciarDeshabilitado =
                "Ningún equipo tiene plantilla con aspectos. Agrega aspectos en «Plantillas» o elige otra ubicación.";
            return;
        }

        if (IsBusy)
        {
            PuedeIniciar = false;
            MotivoIniciarDeshabilitado = "Espera a que termine la operación…";
            return;
        }

        PuedeIniciar = true;
        MotivoIniciarDeshabilitado = string.Empty;
    }

    private static string ExtraerMensajeApi(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return "La API no respondió al iniciar el checklist.";

        try
        {
            using var doc = JsonDocument.Parse(raw);
            if (doc.RootElement.TryGetProperty("mensaje", out var mensaje))
                return mensaje.GetString() ?? raw;
        }
        catch
        {
            // usar texto crudo
        }

        return raw.Length > 280 ? raw[..280] + "…" : raw;
    }

    private void ReiniciarPreparacion()
    {
        DatosPreparados = null;
        _ubicacionPreparada = false;
        ResumenPreparacion = string.Empty;
        EquiposPreparados.Clear();
        TotalEquipos = 0;
        EquiposConPlantilla = 0;
        OnPropertyChanged(nameof(TieneEquiposPreparados));
        OnPropertyChanged(nameof(PasoActualTexto));
        OnPropertyChanged(nameof(TotalEquipos));
        OnPropertyChanged(nameof(EquiposConPlantilla));
        ActualizarEstadoIniciar();
    }
}
