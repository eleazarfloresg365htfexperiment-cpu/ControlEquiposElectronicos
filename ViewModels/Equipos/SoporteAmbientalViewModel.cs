using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class SoporteAmbientalViewModel : EquipoSubmoduloViewModelBase
{
    public SoporteAmbientalViewModel(
        IEquipoApiService equipoApiService,
        IReporteFallaApiService reporteFallaApiService,
        SesionService sesionService,
        ICatalogoApiService catalogoApiService)
        : base(equipoApiService, reporteFallaApiService, sesionService, catalogoApiService)
    {
        Title = "Soporte ambiental";
    }

    protected override IReadOnlyList<string> TiposEquipo { get; } =
        ["Aire acondicionado", "Ambientador", "Ventilador"];

    private int _totalAires;
    public int TotalAires
    {
        get => _totalAires;
        private set { _totalAires = value; OnPropertyChanged(); }
    }

    private int _totalAmbientadores;
    public int TotalAmbientadores
    {
        get => _totalAmbientadores;
        private set { _totalAmbientadores = value; OnPropertyChanged(); }
    }

    public ICommand RegistrarAireCommand => CrearRegistrarCommand("Aire acondicionado");
    public ICommand RegistrarAmbientadorCommand => CrearRegistrarCommand("Ambientador");

    public ICommand RegistrarMantenimientoCommand => ReportarFallaCommand;

    public ICommand ReportarProblemaCommand => ReportarFallaCommand;

    public ICommand RegistrarRecargaCommand => new Command<EquipoListadoDto>(async equipo =>
    {
        if (equipo != null)
            await Shell.Current.DisplayAlertAsync("Recarga",
                $"Registrar recarga para {equipo.Nombre}", "OK");
    });

    protected override void ActualizarContadores()
    {
        TotalAires = Equipos.Count(e => e.Tipo.Contains("Aire", StringComparison.OrdinalIgnoreCase));
        TotalAmbientadores = Equipos.Count(e => e.Tipo.Equals("Ambientador", StringComparison.OrdinalIgnoreCase));
    }
}
