using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Net.Http.Json;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class RedViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly HttpClient _http;

    public RedViewModel()
    {
        // Ignora certificado SSL en desarrollo
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
        };
        _http = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:7212/")
        };
        CargarDatos();
    }

    private bool _cargando;
    public bool Cargando
    {
        get => _cargando;
        set { _cargando = value; OnPropertyChanged(); }
    }

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

    public ObservableCollection<EquipoItem> Dispositivos { get; } = new();

    async void CargarDatos()
    {
        try
        {
            Cargando = true;
            var equipos = await _http.GetFromJsonAsync<List<EquipoItem>>("api/Equipos");
            if (equipos != null)
            {
                Dispositivos.Clear();
                var filtrados = string.IsNullOrWhiteSpace(BusquedaTexto)
                    ? equipos
                    : equipos.Where(e =>
                        e.Nombre.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                        e.Codigo.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                        e.Ubicacion.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase)).ToList();

                // Solo equipos de red
                var redItems = filtrados.Where(e =>
                    e.TipoEquipo == "Router" ||
                    e.TipoEquipo == "Switch" ||
                    e.TipoEquipo == "Repetidor").ToList();

                foreach (var e in redItems)
                    Dispositivos.Add(e);

                TotalRouters = Dispositivos.Count(e => e.TipoEquipo == "Router");
                TotalSwitches = Dispositivos.Count(e => e.TipoEquipo == "Switch");
                TotalRepetidores = Dispositivos.Count(e => e.TipoEquipo == "Repetidor");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sin conexión",
                $"No se pudo conectar a la API.\n{ex.Message}", "OK");
        }
        finally
        {
            Cargando = false;
        }
    }

    public ICommand RegistrarRouterCommand => new Command(async () =>
        await Shell.Current.GoToAsync("RegistrarEquipoPage?tipo=Router"));

    public ICommand RegistrarSwitchCommand => new Command(async () =>
        await Shell.Current.GoToAsync("RegistrarEquipoPage?tipo=Switch"));

    public ICommand RegistrarRepetidorCommand => new Command(async () =>
        await Shell.Current.GoToAsync("RegistrarEquipoPage?tipo=Repetidor"));

    public ICommand BuscarCommand => new Command(CargarDatos);

    public ICommand VerDetalleCommand => new Command(async (item) =>
    {
        if (item is EquipoItem e)
            await Shell.Current.DisplayAlert("Detalle",
                $"Nombre: {e.Nombre}\nCódigo: {e.Codigo}\nUbicación: {e.Ubicacion}\nEstado: {e.Estado}", "OK");
    });

    public ICommand VerPuertosCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Puertos", "Ver puertos del switch", "OK"));

    public ICommand ReportarFallaCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Falla", "Reportar falla de red", "OK"));

    public ICommand ActualizarCommand => new Command(CargarDatos);
}

public class EquipoItem
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
    public string TipoEquipo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
