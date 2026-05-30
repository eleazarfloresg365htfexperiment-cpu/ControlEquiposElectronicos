using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class ElectricidadViewModel : EquipoSubmoduloViewModelBase
{
    public ElectricidadViewModel(
        IEquipoApiService equipoApiService,
        IReporteFallaApiService reporteFallaApiService,
        SesionService sesionService,
        ICatalogoApiService catalogoApiService)
        : base(equipoApiService, reporteFallaApiService, sesionService, catalogoApiService)
    {
        Title = "Electricidad y UPS";
    }

    protected override IReadOnlyList<string> TiposEquipo { get; } = ["UPS", "Regulador"];

    private int _totalUps;
    public int TotalUps
    {
        get => _totalUps;
        private set { _totalUps = value; OnPropertyChanged(); }
    }

    public ICommand RegistrarUpsCommand => CrearRegistrarCommand("UPS");

    public ICommand RegistrarMantenimientoCommand => new Command(async () =>
        await Shell.Current.GoToAsync("//Mantenimientos"));

    public ICommand VerHistorialCommand => new Command<EquipoListadoDto>(async equipo =>
    {
        if (equipo != null)
            await VerDetalleAsync(equipo);
    });

    protected override void ActualizarContadores()
    {
        TotalUps = Equipos.Count(e => e.Tipo.Equals("UPS", StringComparison.OrdinalIgnoreCase));
    }
}
