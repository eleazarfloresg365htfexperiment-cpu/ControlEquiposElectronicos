using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Net.Http.Json;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class ElectricidadViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly HttpClient _http;

    public ElectricidadViewModel()
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

    private bool _cargando;
    public bool Cargando
    {
        get => _cargando;
        set { _cargando = value; OnPropertyChanged(); }
    }

    public ObservableCollection<EquipoItem> ListaUps { get; } = new();

    async void CargarDatos()
    {
        try
        {
            Cargando = true;
            var equipos = await _http.GetFromJsonAsync<List<EquipoItem>>("api/Equipos");
            if (equipos != null)
            {
                ListaUps.Clear();
                var ups = equipos.Where(e => e.TipoEquipo == "UPS").ToList();
                foreach (var e in ups)
                    ListaUps.Add(e);
                TotalUps = ups.Count;
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

    public ICommand RegistrarUpsCommand => new Command(async () =>
        await Shell.Current.GoToAsync("RegistrarEquipoPage?tipo=UPS"));

    public ICommand RegistrarMantenimientoCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Mantenimiento", "Registrar mantenimiento - próximamente", "OK"));

    public ICommand BuscarCommand => new Command(CargarDatos);

    public ICommand VerHistorialCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Historial", "Ver historial del UPS", "OK"));

    public ICommand ReportarFallaCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Falla", "Reportar falla eléctrica", "OK"));

    public ICommand ActualizarCommand => new Command(CargarDatos);
}
