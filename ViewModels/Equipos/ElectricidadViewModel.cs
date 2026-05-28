using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class ElectricidadViewModel : INotifyPropertyChanged
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

    private int _totalUps;
    public int TotalUps
    {
        get => _totalUps;
        set { _totalUps = value; OnPropertyChanged(); }
    }

    private int _totalMantenimientos;
    public int TotalMantenimientos
    {
        get => _totalMantenimientos;
        set { _totalMantenimientos = value; OnPropertyChanged(); }
    }

    public ObservableCollection<object> ListaUps { get; } = new();

    public ICommand RegistrarUpsCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Electricidad", "Registrar UPS - próximamente", "OK"));

    public ICommand RegistrarMantenimientoCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Electricidad", "Registrar mantenimiento - próximamente", "OK"));

    public ICommand BuscarCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Buscar", $"Buscando: {BusquedaTexto}", "OK"));

    public ICommand VerHistorialCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Historial", "Ver historial del UPS", "OK"));

    public ICommand ReportarFallaCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Falla", "Reportar falla eléctrica", "OK"));

    public ICommand ActualizarCommand => new Command(() =>
    {
        ListaUps.Clear();
        TotalUps = 0;
        TotalMantenimientos = 0;
    });
}
