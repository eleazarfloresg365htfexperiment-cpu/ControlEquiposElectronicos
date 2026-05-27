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

    public ObservableCollection<ReporteFallaListadoDto> Reportes { get; set; } = new();
    public ObservableCollection<AuditoriaDto> Historial { get; set; } = new();

    private bool _mostrarHistorial = false;
    public bool MostrarHistorial
    {
        get => _mostrarHistorial;
        set { _mostrarHistorial = value; OnPropertyChanged(); }
    }

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

    public ReportesViewModel(IReporteFallaApiService reporteFallaApiService, IAuditoriaApiService auditoriaApiService)
    {
        Title = "Reportes";
        _reporteFallaApiService = reporteFallaApiService;
        _auditoriaApiService = auditoriaApiService;

        EliminarCommand = new Command<ReporteFallaListadoDto>(async (r) => await EliminarAsync(r));
        NuevoReporteCommand = new Command(async () => await Shell.Current.GoToAsync("ReporteFallaFormulario"));
        ExportarExcelCommand = new Command(async () => await Shell.Current.DisplayAlertAsync("Exportar Excel", "Exportación a Excel próximamente disponible.", "OK"));
        ExportarPdfCommand = new Command(async () => await Shell.Current.DisplayAlertAsync("Exportar PDF", "Exportación a PDF próximamente disponible.", "OK"));
        HistorialCommand = new Command(async () => await CargarHistorialAsync());
        CerrarHistorialCommand = new Command(() => MostrarHistorial = false);
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