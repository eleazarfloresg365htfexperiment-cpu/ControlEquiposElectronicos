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
                var filtrados = string.IsNullOrWhiteSpace(BusquedaTexto)
                    ? equipos
                    : equipos.Where(e =>
                        e.Nombre.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                        e.Codigo.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase)).ToList();

                var lista = filtrados.Where(e => e.TipoEquipo == "UPS").ToList();
                foreach (var e in lista)
                    ListaUps.Add(e);

                TotalUps = ListaUps.Count;
            }
        }
        catch { }
        finally { Cargando = false; }
    }

    public ICommand RegistrarUpsCommand => new Command(async () =>
        await Shell.Current.GoToAsync("RegistrarEquipoPage?tipo=UPS"));

    public ICommand RegistrarMantenimientoCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Mantenimiento", "Registrar mantenimiento - próximamente", "OK"));

    public ICommand BuscarCommand => new Command(CargarDatos);

    public ICommand VerHistorialCommand => new Command(async (item) =>
    {
        if (item is EquipoItem e)
        {
            try
            {
                var detalle = await _http.GetFromJsonAsync<object>(
                    $"api/equipos/{e.Id}/detalle-ups");
                await Shell.Current.DisplayAlert("Detalle UPS",
                    $"Equipo: {e.Nombre}\nEstado: {e.Estado}", "OK");
            }
            catch
            {
                await Shell.Current.DisplayAlert("Detalle",
                    $"Equipo: {e.Nombre}\nCódigo: {e.Codigo}", "OK");
            }
        }
    });

    public ICommand ReportarFallaCommand => new Command(async (item) =>
    {
        if (item is EquipoItem e)
        {
            try
            {
                var payload = new
                {
                    equipoId = e.Id,
                    descripcion = "Falla eléctrica reportada desde la app",
                    tipoFalla = "UPS"
                };
                await _http.PostAsJsonAsync("api/ReportesFalla", payload);
                await Shell.Current.DisplayAlert("✅ Falla reportada",
                    $"Se reportó falla en {e.Nombre}", "OK");
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo reportar la falla.", "OK");
            }
        }
    });

    public ICommand ActualizarCommand => new Command(CargarDatos);
}
