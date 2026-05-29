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

    public ObservableCollection<ReporteFallaListadoDto> Reportes { get; set; } = new();
    public ObservableCollection<HistorialOperacionDto> Historial { get; set; } = new();

    private bool _mostrarHistorial = false;
    public bool MostrarHistorial
    {
        get => _mostrarHistorial;
        set { _mostrarHistorial = value; OnPropertyChanged(); }
    }

    private bool _mostrarFiltroFechas = false;
    public bool MostrarFiltroFechas
    {
        get => _mostrarFiltroFechas;
        set { _mostrarFiltroFechas = value; OnPropertyChanged(); }
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

    public ReportesViewModel(IReporteFallaApiService reporteFallaApiService, IAuditoriaApiService auditoriaApiService, IExportService exportService)
    {
        Title = "Reportes";
        _reporteFallaApiService = reporteFallaApiService;
        _auditoriaApiService = auditoriaApiService;
        _exportService = exportService;

        EliminarCommand = new Command<ReporteFallaListadoDto>(async (r) => await EliminarAsync(r));
        NuevoReporteCommand = new Command(async () => await Shell.Current.GoToAsync("ReporteFallaFormulario"));
        ExportarExcelCommand = new Command(() => { _tipoExport = "excel"; MostrarFiltroFechas = true; });
        ExportarPdfCommand = new Command(() => { _tipoExport = "pdf"; MostrarFiltroFechas = true; });
        HistorialCommand = new Command(async () => await CargarHistorialAsync());
        CerrarHistorialCommand = new Command(() => MostrarHistorial = false);
        ConfirmarExportCommand = new Command(async () => await ConfirmarExportAsync());
        CancelarExportCommand = new Command(() => MostrarFiltroFechas = false);
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
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally { IsBusy = false; }
    }

    private async Task ConfirmarExportAsync()
    {
        MostrarFiltroFechas = false;

        var filtrados = _todosLosReportes
            .Where(r => r.FechaReporte.Date >= FechaDesde.Date && r.FechaReporte.Date <= FechaHasta.Date)
            .ToList();

        if (filtrados.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Aviso", "No hay reportes en el rango de fechas seleccionado.", "OK");
            return;
        }

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
        finally { IsBusy = false; }
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
        finally { IsBusy = false; }
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
            Reportes.Remove(reporte);
            await Shell.Current.DisplayAlertAsync("Éxito", "Reporte eliminado correctamente.", "OK");
        }
        else
            await Shell.Current.DisplayAlertAsync("Error", "No se pudo eliminar el reporte.", "OK");
    }
}
