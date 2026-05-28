using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class ImpresionViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private string _busquedaTexto = string.Empty;
    public string BusquedaTexto
    {
        get => _busquedaTexto;
        set { _busquedaTexto = value; OnPropertyChanged(); }
    }

    private int _totalImpresoras;
    public int TotalImpresoras
    {
        get => _totalImpresoras;
        set { _totalImpresoras = value; OnPropertyChanged(); }
    }

    private int _totalCambiosCartucho;
    public int TotalCambiosCartucho
    {
        get => _totalCambiosCartucho;
        set { _totalCambiosCartucho = value; OnPropertyChanged(); }
    }

    public ObservableCollection<object> Impresoras { get; } = new();

    public ICommand RegistrarImpresoraCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Impresión", "Registrar impresora - próximamente", "OK"));

    public ICommand RegistrarCartuchoCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Impresión", "Registrar cartucho - próximamente", "OK"));

    public ICommand BuscarCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Buscar", $"Buscando: {BusquedaTexto}", "OK"));

    public ICommand VerHistorialCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Historial", "Ver historial de cartuchos", "OK"));

    public ICommand ReportarFallaCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Falla", "Reportar falla de impresión", "OK"));

    public ICommand GenerarReporteCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Reporte", "Generar reporte de impresora", "OK"));

    public ICommand ActualizarCommand => new Command(() =>
    {
        Impresoras.Clear();
        TotalImpresoras = 0;
        TotalCambiosCartucho = 0;
    });
}
