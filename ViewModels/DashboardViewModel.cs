using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private readonly IEquipoApiService _equipoApiService;
    private readonly IMantenimientoApiService _mantenimientoApiService;
    private readonly IReporteFallaApiService _reporteFallaApiService;

    private int _totalEquipos;
    public int TotalEquipos
    {
        get => _totalEquipos;
        set { _totalEquipos = value; OnPropertyChanged(); }
    }

    private int _totalMantenimientos;
    public int TotalMantenimientos
    {
        get => _totalMantenimientos;
        set { _totalMantenimientos = value; OnPropertyChanged(); }
    }

    private int _totalReportes;
    public int TotalReportes
    {
        get => _totalReportes;
        set { _totalReportes = value; OnPropertyChanged(); }
    }

    public DashboardViewModel(
        IEquipoApiService equipoApiService,
        IMantenimientoApiService mantenimientoApiService,
        IReporteFallaApiService reporteFallaApiService)
    {
        _equipoApiService = equipoApiService;
        _mantenimientoApiService = mantenimientoApiService;
        _reporteFallaApiService = reporteFallaApiService;
        Title = "Dashboard";
    }

    public async Task CargarAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var equipos = await _equipoApiService.ObtenerTodosAsync();
            TotalEquipos = equipos?.Count ?? 0;

            var mantenimientos = await _mantenimientoApiService.ObtenerTodosAsync();
            TotalMantenimientos = mantenimientos?.Count ?? 0;

            var reportes = await _reporteFallaApiService.ObtenerTodosAsync();
            TotalReportes = reportes?.Count ?? 0;
        }
        finally
        {
            IsBusy = false;
        }
    }
}