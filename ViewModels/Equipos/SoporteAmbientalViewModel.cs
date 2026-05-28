using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Net.Http.Json;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class SoporteAmbientalViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly HttpClient _http;

    public SoporteAmbientalViewModel()
    {
        _http = new HttpClient { BaseAddress = new Uri("https://localhost:7212/") };
        CargarDatos();
    }

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

    private bool _cargando;
    public bool Cargando
    {
        get => _cargando;
        set { _cargando = value; OnPropertyChanged(); }
    }

    public ObservableCollection<EquipoItem> EquiposAmbientales { get; } = new();

    async void CargarDatos()
    {
        try
        {
            Cargando = true;
            var equipos = await _http.GetFromJsonAsync<List<EquipoItem>>("api/Equipos");
            if (equipos != null)
            {
                EquiposAmbientales.Clear();
                var ambientales = equipos.Where(e =>
                    e.TipoEquipo == "AireAcondicionado" ||
                    e.TipoEquipo == "Ambientador").ToList();
                foreach (var e in ambientales)
                    EquiposAmbientales.Add(e);
                TotalAires = equipos.Count(e => e.TipoEquipo == "AireAcondicionado");
                TotalAmbientadores = equipos.Count(e => e.TipoEquipo == "Ambientador");
            }
        }
        catch
        {
            // API no disponible aún
        }
        finally
        {
            Cargando = false;
        }
    }

    public ICommand RegistrarAireCommand => new Command(async () =>
        await Shell.Current.GoToAsync("RegistrarEquipoPage?tipo=AireAcondicionado"));

    public ICommand RegistrarAmbientadorCommand => new Command(async () =>
        await Shell.Current.GoToAsync("RegistrarEquipoPage?tipo=Ambientador"));

    public ICommand BuscarCommand => new Command(CargarDatos);

    public ICommand RegistrarMantenimientoCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Mantenimiento", "Registrar mantenimiento ambiental", "OK"));

    public ICommand ReportarProblemaCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Problema", "Reportar problema ambiental", "OK"));

    public ICommand RegistrarRecargaCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Recarga", "Registrar recarga o reemplazo", "OK"));

    public ICommand ActualizarCommand => new Command(CargarDatos);
}
