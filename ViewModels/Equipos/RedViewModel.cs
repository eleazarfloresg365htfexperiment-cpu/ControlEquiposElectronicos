using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class RedViewModel : EquipoSubmoduloViewModelBase
{
    public RedViewModel(
        IEquipoApiService equipoApiService,
        IReporteFallaApiService reporteFallaApiService,
        SesionService sesionService,
        ICatalogoApiService catalogoApiService)
        : base(equipoApiService, reporteFallaApiService, sesionService, catalogoApiService)
    {
        Title = "Infraestructura de Red";
    }

    protected override IReadOnlyList<string> TiposEquipo { get; } =
        ["Router", "Switch", "Repetidor"];

    private int _totalRouters;
    public int TotalRouters
    {
        get => _totalRouters;
        private set { _totalRouters = value; OnPropertyChanged(); }
    }

    private int _totalSwitches;
    public int TotalSwitches
    {
        get => _totalSwitches;
        private set { _totalSwitches = value; OnPropertyChanged(); }
    }

    private int _totalRepetidores;
    public int TotalRepetidores
    {
        get => _totalRepetidores;
        private set { _totalRepetidores = value; OnPropertyChanged(); }
    }

    public ICommand RegistrarRouterCommand => CrearRegistrarCommand("Router");
    public ICommand RegistrarSwitchCommand => CrearRegistrarCommand("Switch");
    public ICommand RegistrarRepetidorCommand => CrearRegistrarCommand("Repetidor");

    public ICommand VerPuertosCommand => new Command(async () =>
        await Shell.Current.DisplayAlertAsync("Puertos", "Gestión de puertos del switch — próximamente.", "OK"));

    protected override void ActualizarContadores()
    {
        TotalRouters = Equipos.Count(e => e.Tipo.Equals("Router", StringComparison.OrdinalIgnoreCase));
        TotalSwitches = Equipos.Count(e => e.Tipo.Equals("Switch", StringComparison.OrdinalIgnoreCase));
        TotalRepetidores = Equipos.Count(e => e.Tipo.Equals("Repetidor", StringComparison.OrdinalIgnoreCase));
    }
}
