using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private readonly IEquipoApiService _equipoApiService;
    private readonly IMantenimientoApiService _mantenimientoApiService;
    private readonly IReporteFallaApiService _reporteFallaApiService;
    private readonly IChecklistApiService _checklistApiService;
    private readonly IPermisoService _permisoService;

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

    private int _totalChecklists;
    public int TotalChecklists
    {
        get => _totalChecklists;
        set { _totalChecklists = value; OnPropertyChanged(); }
    }

    public bool MostrarEquipos => _permisoService.PuedeVerModulo("Equipos");
    public bool MostrarMantenimientos => _permisoService.PuedeVerModulo("Mantenimientos");
    public bool MostrarReportes => _permisoService.PuedeVerModulo("Reportes");
    public bool MostrarChecklist => _permisoService.PuedeVerModulo("Checklist");
    public bool MostrarUsuarios => _permisoService.PuedeVerModulo("Usuarios");
    public bool MostrarConfiguracion => _permisoService.PuedeVerModulo("Configuracion");
    public bool MostrarConsultas => _permisoService.PuedeVerModulo("Equipos")
        || _permisoService.PuedeVerModulo("Reportes")
        || _permisoService.PuedeVerModulo("Mantenimientos");

    public DashboardViewModel(
        IEquipoApiService equipoApiService,
        IMantenimientoApiService mantenimientoApiService,
        IReporteFallaApiService reporteFallaApiService,
        IChecklistApiService checklistApiService,
        IPermisoService permisoService)
    {
        _equipoApiService = equipoApiService;
        _mantenimientoApiService = mantenimientoApiService;
        _reporteFallaApiService = reporteFallaApiService;
        _checklistApiService = checklistApiService;
        _permisoService = permisoService;
        Title = "Dashboard";
    }

    public void ActualizarVisibilidad()
    {
        OnPropertyChanged(nameof(MostrarEquipos));
        OnPropertyChanged(nameof(MostrarMantenimientos));
        OnPropertyChanged(nameof(MostrarReportes));
        OnPropertyChanged(nameof(MostrarChecklist));
        OnPropertyChanged(nameof(MostrarUsuarios));
        OnPropertyChanged(nameof(MostrarConfiguracion));
        OnPropertyChanged(nameof(MostrarConsultas));
    }

    public async Task CargarAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var equipos = await _equipoApiService.ObtenerTodosAsync();
            TotalEquipos = equipos?.Count ?? 0;

            var mantenimientos = await _mantenimientoApiService.ObtenerTodosAsync();
            TotalMantenimientos = mantenimientos?.Count ?? 0;

            var reportes = await _reporteFallaApiService.ObtenerTodosAsync();
            TotalReportes = reportes?.Count ?? 0;

            var checklists = await _checklistApiService.ObtenerTodosAsync();
            TotalChecklists = checklists?.Count ?? 0;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
