using ControlEquiposElectronicos.DTOs.Auditoria;
using ControlEquiposElectronicos.DTOs.Reportes;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels;

public class ReportesViewModel : BaseViewModel
{
    private readonly IReporteFallaApiService _reporteFallaApiService;
    private readonly IAuditoriaApiService _auditoriaApiService;
    private readonly IExportService _exportService;
    private readonly HashSet<(int Anio, int Mes)> _mesesSeleccionados = new();

    public ObservableCollection<ReporteFallaListadoDto> Reportes { get; set; } = new();
    public ObservableCollection<HistorialOperacionDto> Historial { get; set; } = new();
    public ObservableCollection<MesCalendarioExportItem> MesesCalendario { get; } = new();

    private bool _mostrarHistorial;
    public bool MostrarHistorial
    {
        get => _mostrarHistorial;
        set { _mostrarHistorial = value; OnPropertyChanged(); }
    }

    private bool _mostrarOpcionesExport;
    public bool MostrarOpcionesExport
    {
        get => _mostrarOpcionesExport;
        set { _mostrarOpcionesExport = value; OnPropertyChanged(); }
    }

    private int _anioCalendario = DateTime.Today.Year;
    public int AnioCalendario
    {
        get => _anioCalendario;
        set
        {
            if (_anioCalendario == value) return;
            _anioCalendario = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TextoAnioCalendario));
            CargarMesesCalendario();
        }
    }

    public string TextoAnioCalendario => AnioCalendario.ToString();

    private bool _exportarTodos;
    public bool ExportarTodos
    {
        get => _exportarTodos;
        set
        {
            if (_exportarTodos == value) return;
            _exportarTodos = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MostrarCalendarioMeses));
            if (value)
                DeseleccionarTodosLosMeses();
            ActualizarResumenExportacion();
        }
    }

    public bool MostrarCalendarioMeses => !ExportarTodos;

    private string _resumenExportacion = string.Empty;
    public string ResumenExportacion
    {
        get => _resumenExportacion;
        private set { _resumenExportacion = value; OnPropertyChanged(); }
    }

    private string _tipoExport = string.Empty;

    private string _busqueda = string.Empty;
    public string Busqueda
    {
        get => _busqueda;
        set { _busqueda = value; OnPropertyChanged(); FiltrarReportes(); }
    }

    private string _estadoFiltro = string.Empty;
    public string EstadoFiltro
    {
        get => _estadoFiltro;
        set { _estadoFiltro = value; OnPropertyChanged(); FiltrarReportes(); }
    }

    private List<ReporteFallaListadoDto> _todosLosReportes = new();

    public ICommand EliminarCommand { get; }
    public ICommand NuevoReporteCommand { get; }
    public ICommand ExportarExcelCommand { get; }
    public ICommand ExportarPdfCommand { get; }
    public ICommand HistorialCommand { get; }
    public ICommand CerrarHistorialCommand { get; }
    public ICommand ConfirmarExportCommand { get; }
    public ICommand CancelarExportCommand { get; }
    public ICommand AnioAnteriorCommand { get; }
    public ICommand AnioSiguienteCommand { get; }
    public ICommand ToggleMesCommand { get; }
    public ICommand SeleccionarMesesConDatosCommand { get; }
    public ICommand LimpiarMesesCommand { get; }

    public ReportesViewModel(
        IReporteFallaApiService reporteFallaApiService,
        IAuditoriaApiService auditoriaApiService,
        IExportService exportService)
    {
        Title = "Reportes";
        _reporteFallaApiService = reporteFallaApiService;
        _auditoriaApiService = auditoriaApiService;
        _exportService = exportService;

        EliminarCommand = new Command<ReporteFallaListadoDto>(async (r) => await EliminarAsync(r));
        NuevoReporteCommand = new Command(async () => await Shell.Current.GoToAsync("ReporteFallaFormulario"));
        ExportarExcelCommand = new Command(() => AbrirOpcionesExportacion("excel"));
        ExportarPdfCommand = new Command(() => AbrirOpcionesExportacion("pdf"));
        HistorialCommand = new Command(async () => await CargarHistorialAsync());
        CerrarHistorialCommand = new Command(() => MostrarHistorial = false);
        ConfirmarExportCommand = new Command(async () => await ConfirmarExportAsync());
        CancelarExportCommand = new Command(() => MostrarOpcionesExport = false);
        AnioAnteriorCommand = new Command(() => AnioCalendario--);
        AnioSiguienteCommand = new Command(() => AnioCalendario++);
        ToggleMesCommand = new Command<MesCalendarioExportItem>(ToggleMes);
        SeleccionarMesesConDatosCommand = new Command(SeleccionarMesesConDatosEnTodosLosAnios);
        LimpiarMesesCommand = new Command(DeseleccionarTodosLosMeses);
    }

    public async Task CargarReportesAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var lista = await _reporteFallaApiService.ObtenerTodosAsync();
            _todosLosReportes = lista;
            Reportes.Clear();
            foreach (var r in lista)
                Reportes.Add(r);

            if (MostrarOpcionesExport)
                CargarMesesCalendario();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void AbrirOpcionesExportacion(string tipo)
    {
        _tipoExport = tipo;
        _mesesSeleccionados.Clear();
        ExportarTodos = false;
        AnioCalendario = DateTime.Today.Year;
        CargarMesesCalendario();
        ActualizarResumenExportacion();
        MostrarOpcionesExport = true;
    }

    private void CargarMesesCalendario()
    {
        MesesCalendario.Clear();
        for (int mes = 1; mes <= 12; mes++)
        {
            var cantidad = _todosLosReportes.Count(r =>
                r.FechaReporte.Year == AnioCalendario && r.FechaReporte.Month == mes);

            var seleccionado = _mesesSeleccionados.Contains((AnioCalendario, mes));
            MesesCalendario.Add(new MesCalendarioExportItem(AnioCalendario, mes, cantidad, seleccionado));
        }
    }

    private void ToggleMes(MesCalendarioExportItem? item)
    {
        if (item == null || ExportarTodos) return;

        item.Seleccionado = !item.Seleccionado;
        var clave = (item.Anio, item.Mes);
        if (item.Seleccionado)
            _mesesSeleccionados.Add(clave);
        else
            _mesesSeleccionados.Remove(clave);

        ActualizarResumenExportacion();
    }

    private void SeleccionarMesesConDatosEnTodosLosAnios()
    {
        ExportarTodos = false;
        _mesesSeleccionados.Clear();

        foreach (var grupo in _todosLosReportes
                     .GroupBy(r => (r.FechaReporte.Year, r.FechaReporte.Month)))
        {
            _mesesSeleccionados.Add(grupo.Key);
        }

        CargarMesesCalendario();
        ActualizarResumenExportacion();
    }

    private void DeseleccionarTodosLosMeses()
    {
        _mesesSeleccionados.Clear();
        foreach (var mes in MesesCalendario)
            mes.Seleccionado = false;
        ActualizarResumenExportacion();
    }

    private void ActualizarResumenExportacion()
    {
        if (ExportarTodos)
        {
            ResumenExportacion = $"Se exportarán todos los reportes ({_todosLosReportes.Count}).";
            return;
        }

        if (_mesesSeleccionados.Count == 0)
        {
            ResumenExportacion = "Selecciona uno o más meses en el calendario.";
            return;
        }

        var total = _todosLosReportes.Count(r =>
            _mesesSeleccionados.Contains((r.FechaReporte.Year, r.FechaReporte.Month)));

        ResumenExportacion = _mesesSeleccionados.Count == 1
            ? $"1 mes seleccionado · {total} reporte(s)."
            : $"{_mesesSeleccionados.Count} meses seleccionados · {total} reporte(s).";
    }

    private async Task ConfirmarExportAsync()
    {
        List<ReporteFallaListadoDto> filtrados;

        if (ExportarTodos)
        {
            filtrados = _todosLosReportes.ToList();
        }
        else if (_mesesSeleccionados.Count > 0)
        {
            filtrados = _todosLosReportes
                .Where(r => _mesesSeleccionados.Contains((r.FechaReporte.Year, r.FechaReporte.Month)))
                .OrderByDescending(r => r.FechaReporte)
                .ToList();
        }
        else
        {
            await Shell.Current.DisplayAlertAsync("Aviso",
                "Marca «Exportar todos» o selecciona al menos un mes en el calendario.", "OK");
            return;
        }

        if (filtrados.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Aviso", "No hay reportes para la selección indicada.", "OK");
            return;
        }

        MostrarOpcionesExport = false;
        IsBusy = true;
        try
        {
            string path;
            if (_tipoExport == "excel")
                path = await _exportService.ExportarReportesExcelAsync(filtrados);
            else
                path = await _exportService.ExportarReportesPdfAsync(filtrados);

            await Shell.Current.DisplayAlertAsync("Éxito", $"Archivo guardado en:\n{path}", "OK");
            await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(path) });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CargarHistorialAsync()
    {
        IsBusy = true;
        try
        {
            var lista = await _auditoriaApiService.ObtenerPorModuloAsync("ReportesFalla");
            Historial.Clear();
            foreach (var h in lista)
                Historial.Add(h);
            MostrarHistorial = true;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void FiltrarReportes()
    {
        var filtrados = _todosLosReportes.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(_busqueda))
            filtrados = filtrados.Where(r =>
                r.CodigoEquipo.Contains(_busqueda, StringComparison.OrdinalIgnoreCase) ||
                r.Titulo.Contains(_busqueda, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(_estadoFiltro))
            filtrados = filtrados.Where(r =>
                r.EstadoReporte.Equals(_estadoFiltro, StringComparison.OrdinalIgnoreCase));

        Reportes.Clear();
        foreach (var r in filtrados)
            Reportes.Add(r);
    }

    private async Task EliminarAsync(ReporteFallaListadoDto reporte)
    {
        bool confirmar = await Shell.Current.DisplayAlertAsync(
            "Confirmar eliminación",
            $"¿Deseas eliminar el reporte '{reporte.Titulo}'?",
            "Sí, eliminar", "Cancelar");

        if (!confirmar) return;

        var resultado = await _reporteFallaApiService.EliminarAsync(reporte.Id);
        if (resultado)
        {
            _todosLosReportes.RemoveAll(r => r.Id == reporte.Id);
            Reportes.Remove(reporte);
            if (MostrarOpcionesExport)
            {
                CargarMesesCalendario();
                ActualizarResumenExportacion();
            }
            await Shell.Current.DisplayAlertAsync("Éxito", "Reporte eliminado correctamente.", "OK");
        }
        else
            await Shell.Current.DisplayAlertAsync("Error", "No se pudo eliminar el reporte.", "OK");
    }
}
