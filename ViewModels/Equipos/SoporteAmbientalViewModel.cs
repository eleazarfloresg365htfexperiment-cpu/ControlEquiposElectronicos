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
                var filtrados = string.IsNullOrWhiteSpace(BusquedaTexto)
                    ? equipos
                    : equipos.Where(e =>
                        e.Nombre.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                        e.Ubicacion.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase)).ToList();

                var lista = filtrados.Where(e =>
                    e.TipoEquipo == "AireAcondicionado" ||
                    e.TipoEquipo == "Ambientador").ToList();

                foreach (var e in lista)
                    EquiposAmbientales.Add(e);

                TotalAires = EquiposAmbientales.Count(e => e.TipoEquipo == "AireAcondicionado");
                TotalAmbientadores = EquiposAmbientales.Count(e => e.TipoEquipo == "Ambientador");
            }
        }
        catch { }
        finally { Cargando = false; }
    }

    public ICommand RegistrarAireCommand => new Command(async () =>
        await Shell.Current.GoToAsync("RegistrarEquipoPage?tipo=AireAcondicionado"));

    public ICommand RegistrarAmbientadorCommand => new Command(async () =>
        await Shell.Current.GoToAsync("RegistrarEquipoPage?tipo=Ambientador"));

    public ICommand BuscarCommand => new Command(CargarDatos);

    public ICommand RegistrarMantenimientoCommand => new Command(async (item) =>
    {
        if (item is EquipoItem e)
        {
            try
            {
                var detalle = await _http.GetFromJsonAsync<object>(
                    $"api/equipos/{e.Id}/detalle-ambiental");
                await Shell.Current.DisplayAlert("Ambiental",
                    $"Detalle de {e.Nombre} cargado.", "OK");
            }
            catch
            {
                await Shell.Current.DisplayAlert("Mantenimiento",
                    $"Equipo: {e.Nombre}\nCódigo: {e.Codigo}", "OK");
            }
        }
    });

    public ICommand ReportarProblemaCommand => new Command(async (item) =>
    {
        if (item is EquipoItem e)
        {
            try
            {
                var payload = new
                {
                    equipoId = e.Id,
                    descripcion = "Problema ambiental reportado desde la app",
                    tipoFalla = "Ambiental"
                };
                await _http.PostAsJsonAsync("api/ReportesFalla", payload);
                await Shell.Current.DisplayAlert("✅ Reportado",
                    $"Problema en {e.Nombre} reportado.", "OK");
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo reportar.", "OK");
            }
        }
    });

    public ICommand RegistrarRecargaCommand => new Command(async (item) =>
    {
        if (item is EquipoItem e)
            await Shell.Current.DisplayAlert("Recarga",
                $"Registrar recarga para {e.Nombre}", "OK");
    });

    public ICommand ActualizarCommand => new Command(CargarDatos);
}
