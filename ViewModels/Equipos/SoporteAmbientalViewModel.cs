using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class SoporteAmbientalViewModel : INotifyPropertyChanged
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

    private int _totalAires;
    public int TotalAires
    {
        get => _totalAires;
        set { _totalAires = value; OnPropertyChanged(); }
    }

    private int _totalAmbientadores;
    public int TotalAmbientadores
    {
        get => _totalAmbientadores;
        set { _totalAmbientadores = value; OnPropertyChanged(); }
    }

    public ObservableCollection<object> EquiposAmbientales { get; } = new();

    public ICommand RegistrarAireCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Ambiental", "Registrar aire acondicionado - próximamente", "OK"));

    public ICommand RegistrarAmbientadorCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Ambiental", "Registrar ambientador - próximamente", "OK"));

    public ICommand BuscarCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Buscar", $"Buscando: {BusquedaTexto}", "OK"));

    public ICommand RegistrarMantenimientoCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Mantenimiento", "Registrar mantenimiento ambiental", "OK"));

    public ICommand ReportarProblemaCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Problema", "Reportar problema ambiental", "OK"));

    public ICommand RegistrarRecargaCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Recarga", "Registrar recarga o reemplazo", "OK"));

    public ICommand ActualizarCommand => new Command(() =>
    {
        EquiposAmbientales.Clear();
        TotalAires = 0;
        TotalAmbientadores = 0;
    });
}
