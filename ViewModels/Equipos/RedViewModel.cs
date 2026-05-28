using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class RedViewModel : INotifyPropertyChanged
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

    private int _totalRouters;
    public int TotalRouters
    {
        get => _totalRouters;
        set { _totalRouters = value; OnPropertyChanged(); }
    }

    private int _totalSwitches;
    public int TotalSwitches
    {
        get => _totalSwitches;
        set { _totalSwitches = value; OnPropertyChanged(); }
    }

    private int _totalRepetidores;
    public int TotalRepetidores
    {
        get => _totalRepetidores;
        set { _totalRepetidores = value; OnPropertyChanged(); }
    }

    public ObservableCollection<object> Dispositivos { get; } = new();

    public ICommand RegistrarRouterCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Red", "Registrar Router - próximamente", "OK"));

    public ICommand RegistrarSwitchCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Red", "Registrar Switch - próximamente", "OK"));

    public ICommand RegistrarRepetidorCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Red", "Registrar Repetidor - próximamente", "OK"));

    public ICommand BuscarCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Buscar", $"Buscando: {BusquedaTexto}", "OK"));

    public ICommand VerDetalleCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Detalle", "Ver detalle del dispositivo", "OK"));

    public ICommand VerPuertosCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Puertos", "Ver puertos del switch", "OK"));

    public ICommand ReportarFallaCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Falla", "Reportar falla de red", "OK"));

    public ICommand ActualizarCommand => new Command(() =>
    {
        Dispositivos.Clear();
        TotalRouters = 0;
        TotalSwitches = 0;
        TotalRepetidores = 0;
    });
}
