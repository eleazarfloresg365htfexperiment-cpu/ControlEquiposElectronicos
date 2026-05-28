using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Net.Http.Json;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class ImpresionViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly HttpClient _http;

    public ImpresionViewModel()
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

    private bool _cargando;
    public bool Cargando
    {
        get => _cargando;
        set { _cargando = value; OnPropertyChanged(); }
    }

    public ObservableCollection<EquipoItem> Impresoras { get; } = new();

    async void CargarDatos()
    {
        try
        {
            Cargando = true;
            var equipos = await _http.GetFromJsonAsync<List<EquipoItem>>("api/Equipos");
            if (equipos != null)
            {
                Impresoras.Clear();
                var impresoras = equipos.Where(e => e.TipoEquipo == "Impresora").ToList();
                foreach (var e in impresoras)
                    Impresoras.Add(e);
                TotalImpresoras = impresoras.Count;
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

    public ICommand RegistrarImpresoraCommand => new Command(async () =>
        await Shell.Current.GoToAsync("RegistrarEquipoPage?tipo=Impresora"));

    public ICommand RegistrarCartuchoCommand => new Command(async () =>
        await Shell.Current.DisplayAlert("Cartucho", "Registrar cambio de cartucho - próximamente", "OK"));

    public ICommand BuscarCommand => new Command(CargarDatos);

    public ICommand VerHistorialCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Historial", "Ver historial de cartuchos", "OK"));

    public ICommand ReportarFallaCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Falla", "Reportar falla de impresión", "OK"));

    public ICommand GenerarReporteCommand => new Command(async (item) =>
        await Shell.Current.DisplayAlert("Reporte", "Generar reporte de impresora", "OK"));

    public ICommand ActualizarCommand => new Command(CargarDatos);
}
