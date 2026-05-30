using ControlEquiposElectronicos.DTOs.Catalogos;
using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.DTOs.Mantenimientos;
using ControlEquiposElectronicos.DTOs.Reportes;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Consultas;

public class ConsultasViewModel : BaseViewModel
{
    private readonly IEquipoApiService _equipoApiService;
    private readonly IReporteFallaApiService _reporteFallaApiService;
    private readonly IMantenimientoApiService _mantenimientoApiService;
    private readonly ICatalogoApiService _catalogoApiService;

    public ObservableCollection<EquipoListadoDto> Resultados { get; } = new();
    public ObservableCollection<CatalogoItemDto> Categorias { get; } = new();
    public ObservableCollection<CatalogoItemDto> Estados { get; } = new();
    public ObservableCollection<CatalogoItemDto> Ubicaciones { get; } = new();

    private string _busqueda = string.Empty;
    public string Busqueda
    {
        get => _busqueda;
        set { _busqueda = value; OnPropertyChanged(); }
    }

    private CatalogoItemDto? _categoriaSeleccionada;
    public CatalogoItemDto? CategoriaSeleccionada
    {
        get => _categoriaSeleccionada;
        set { _categoriaSeleccionada = value; OnPropertyChanged(); }
    }

    private CatalogoItemDto? _estadoSeleccionado;
    public CatalogoItemDto? EstadoSeleccionado
    {
        get => _estadoSeleccionado;
        set { _estadoSeleccionado = value; OnPropertyChanged(); }
    }

    private CatalogoItemDto? _ubicacionSeleccionada;
    public CatalogoItemDto? UbicacionSeleccionada
    {
        get => _ubicacionSeleccionada;
        set { _ubicacionSeleccionada = value; OnPropertyChanged(); }
    }

    private DateTime _fechaDesde = DateTime.Now.AddMonths(-1);
    public DateTime FechaDesde
    {
        get => _fechaDesde;
        set { _fechaDesde = value; OnPropertyChanged(); }
    }

    private DateTime _fechaHasta = DateTime.Now;
    public DateTime FechaHasta
    {
        get => _fechaHasta;
        set { _fechaHasta = value; OnPropertyChanged(); }
    }

    private bool _filtrarPorActividad;
    public bool FiltrarPorActividad
    {
        get => _filtrarPorActividad;
        set
        {
            _filtrarPorActividad = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MostrarFiltroFechas));
        }
    }

    public bool MostrarFiltroFechas => FiltrarPorActividad;

    private int _totalResultados;
    public int TotalResultados
    {
        get => _totalResultados;
        private set { _totalResultados = value; OnPropertyChanged(); }
    }

    private bool _sinResultados;
    public bool SinResultados
    {
        get => _sinResultados;
        private set { _sinResultados = value; OnPropertyChanged(); }
    }

    private bool _errorConexion;
    public bool ErrorConexion
    {
        get => _errorConexion;
        private set { _errorConexion = value; OnPropertyChanged(); }
    }

    private string _mensajeEstado = string.Empty;
    public string MensajeEstado
    {
        get => _mensajeEstado;
        private set { _mensajeEstado = value; OnPropertyChanged(); }
    }

    private List<EquipoListadoDto> _equipos = new();
    private List<ReporteFallaListadoDto> _reportes = new();
    private List<MantenimientoListadoDto> _mantenimientos = new();

    public ICommand BuscarCommand { get; }
    public ICommand LimpiarFiltrosCommand { get; }

    public ConsultasViewModel(
        IEquipoApiService equipoApiService,
        IReporteFallaApiService reporteFallaApiService,
        IMantenimientoApiService mantenimientoApiService,
        ICatalogoApiService catalogoApiService)
    {
        Title = "Consultas";
        _equipoApiService = equipoApiService;
        _reporteFallaApiService = reporteFallaApiService;
        _mantenimientoApiService = mantenimientoApiService;
        _catalogoApiService = catalogoApiService;
        BuscarCommand = new Command(() => AplicarFiltros());
        LimpiarFiltrosCommand = new Command(LimpiarFiltros);
    }

    public async Task CargarAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorConexion = false;
        try
        {
            await CargarCatalogosAsync();

            var equiposTask = _equipoApiService.ObtenerTodosAsync();
            var reportesTask = _reporteFallaApiService.ObtenerTodosAsync();
            var mantenimientosTask = _mantenimientoApiService.ObtenerTodosAsync();

            await Task.WhenAll(equiposTask, reportesTask, mantenimientosTask);

            _equipos = equiposTask.Result ?? new();
            _reportes = reportesTask.Result ?? new();
            _mantenimientos = mantenimientosTask.Result ?? new();

            if (_equipos.Count == 0 && _reportes.Count == 0 && _mantenimientos.Count == 0)
            {
                ErrorConexion = true;
                MensajeEstado = "No se obtuvieron datos. Verifica que la API esté en ejecución y la URL en ApiConstants.";
            }
            else
            {
                MensajeEstado = $"Datos cargados: {_equipos.Count} equipos, {_reportes.Count} reportes, {_mantenimientos.Count} mantenimientos.";
            }

            AplicarFiltros();
        }
        catch (Exception ex)
        {
            ErrorConexion = true;
            MensajeEstado = ex.Message;
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CargarCatalogosAsync()
    {
        var categorias = await _catalogoApiService.ObtenerCategoriasAsync();
        var estados = await _catalogoApiService.ObtenerEstadosEquipoAsync();
        var ubicaciones = await _catalogoApiService.ObtenerUbicacionesAsync();

        Categorias.Clear();
        foreach (var c in categorias.Where(x => x.Activo))
            Categorias.Add(c);

        Estados.Clear();
        foreach (var e in estados.Where(x => x.Activo))
            Estados.Add(e);

        Ubicaciones.Clear();
        foreach (var u in ubicaciones.Where(x => x.Activo))
            Ubicaciones.Add(u);
    }

    private void LimpiarFiltros()
    {
        Busqueda = string.Empty;
        CategoriaSeleccionada = null;
        EstadoSeleccionado = null;
        UbicacionSeleccionada = null;
        FiltrarPorActividad = false;
        FechaDesde = DateTime.Now.AddMonths(-1);
        FechaHasta = DateTime.Now;
        AplicarFiltros();
    }

    private void AplicarFiltros()
    {
        if (FechaDesde.Date > FechaHasta.Date)
        {
            MensajeEstado = "La fecha «desde» no puede ser posterior a la fecha «hasta».";
            SinResultados = true;
            TotalResultados = 0;
            Resultados.Clear();
            return;
        }

        var filtrados = _equipos.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(Busqueda))
        {
            filtrados = filtrados.Where(e =>
                e.Codigo.Contains(Busqueda, StringComparison.OrdinalIgnoreCase) ||
                e.Nombre.Contains(Busqueda, StringComparison.OrdinalIgnoreCase) ||
                e.Marca.Contains(Busqueda, StringComparison.OrdinalIgnoreCase) ||
                e.Modelo.Contains(Busqueda, StringComparison.OrdinalIgnoreCase));
        }

        if (CategoriaSeleccionada != null)
            filtrados = filtrados.Where(e =>
                e.Categoria.Equals(CategoriaSeleccionada.Nombre, StringComparison.OrdinalIgnoreCase));

        if (EstadoSeleccionado != null)
            filtrados = filtrados.Where(e =>
                e.Estado.Equals(EstadoSeleccionado.Nombre, StringComparison.OrdinalIgnoreCase));

        if (UbicacionSeleccionada != null)
            filtrados = filtrados.Where(e =>
                e.Ubicacion.Equals(UbicacionSeleccionada.Nombre, StringComparison.OrdinalIgnoreCase));

        if (FiltrarPorActividad)
        {
            var desde = FechaDesde.Date;
            var hasta = FechaHasta.Date;

            var equipoIdsConActividad = _reportes
                .Where(r => r.FechaReporte.Date >= desde && r.FechaReporte.Date <= hasta)
                .Select(r => r.EquipoId)
                .Concat(_mantenimientos
                    .Where(m => m.FechaInicio.Date >= desde && m.FechaInicio.Date <= hasta)
                    .Select(m => m.EquipoId))
                .ToHashSet();

            filtrados = filtrados.Where(e => equipoIdsConActividad.Contains(e.Id));
        }

        var lista = filtrados
            .OrderBy(e => e.Codigo)
            .ToList();

        Resultados.Clear();
        foreach (var e in lista)
            Resultados.Add(e);

        TotalResultados = lista.Count;
        SinResultados = lista.Count == 0;

        if (!ErrorConexion)
        {
            MensajeEstado = SinResultados
                ? "No hay equipos que coincidan con los filtros aplicados."
                : $"{TotalResultados} equipo(s) encontrado(s).";
        }
    }
}
