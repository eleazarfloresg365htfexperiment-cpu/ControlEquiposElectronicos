using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class ImpresionViewModel : EquipoSubmoduloViewModelBase
{
    public ImpresionViewModel(
        IEquipoApiService equipoApiService,
        IReporteFallaApiService reporteFallaApiService,
        SesionService sesionService,
        ICatalogoApiService catalogoApiService)
        : base(equipoApiService, reporteFallaApiService, sesionService, catalogoApiService)
    {
        Title = "Impresión";
    }

    protected override IReadOnlyList<string> TiposEquipo { get; } = ["Impresora", "Multifuncional"];

    private int _totalImpresoras;
    public int TotalImpresoras
    {
        get => _totalImpresoras;
        private set { _totalImpresoras = value; OnPropertyChanged(); }
    }

    public ICommand RegistrarImpresoraCommand => CrearRegistrarCommand("Impresora");

    public ICommand VerHistorialCommand => new Command(async () =>
        await Shell.Current.DisplayAlertAsync("Historial", "Historial de cartuchos — próximamente.", "OK"));

    public ICommand GenerarReporteCommand => new Command(async () =>
        await Shell.Current.DisplayAlertAsync("Reporte", "Generación de reporte — próximamente.", "OK"));

    public ICommand RegistrarCartuchoCommand => new Command(async () =>
        await Shell.Current.DisplayAlertAsync(
            "Cartucho",
            "El registro de cambios de cartucho se conectará cuando el endpoint esté disponible en la API.",
            "OK"));

    protected override void ActualizarContadores()
    {
        TotalImpresoras = Equipos.Count;
    }
}
