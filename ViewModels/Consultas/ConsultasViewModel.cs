using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Consultas;

public class ConsultasViewModel : BaseViewModel
{
    private readonly IEquipoApiService _equipoApiService;

    public ObservableCollection<EquipoListadoDto> Resultados { get; set; } = new();

    private string _busqueda = string.Empty;
    public string Busqueda
    {
        get => _busqueda;
        set { _busqueda = value; OnPropertyChanged(); }
    }

    private string _categoriaFiltro = string.Empty;
    public string CategoriaFiltro
    {
        get => _categoriaFiltro;
        set { _categoriaFiltro = value; OnPropertyChanged(); }
    }

    private string _estadoFiltro = string.Empty;
    public string EstadoFiltro
    {
        get => _estadoFiltro;
        set { _estadoFiltro = value; OnPropertyChanged(); }
    }

    private string _ubicacionFiltro = string.Empty;
    public string UbicacionFiltro
    {
        get => _ubicacionFiltro;
        set { _ubicacionFiltro = value; OnPropertyChanged(); }
    }

    private List<EquipoListadoDto> _todos = new();

    public ICommand BuscarCommand { get; }

    public ConsultasViewModel(IEquipoApiService equipoApiService)
    {
        Title = "Consultas";
        _equipoApiService = equipoApiService;
        BuscarCommand = new Command(async () => await BuscarAsync());
    }

    public async Task CargarAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            _todos = await _equipoApiService.ObtenerTodosAsync();
            Resultados.Clear();
            foreach (var e in _todos)
                Resultados.Add(e);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally { IsBusy = false; }
    }

    private async Task BuscarAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var filtrados = _todos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(Busqueda))
                filtrados = filtrados.Where(e =>
                    e.Codigo.Contains(Busqueda, StringComparison.OrdinalIgnoreCase) ||
                    e.Nombre.Contains(Busqueda, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(CategoriaFiltro))
                filtrados = filtrados.Where(e =>
                    e.Categoria.Equals(CategoriaFiltro, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(EstadoFiltro))
                filtrados = filtrados.Where(e =>
                    e.Estado.Equals(EstadoFiltro, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(UbicacionFiltro))
                filtrados = filtrados.Where(e =>
                    e.Ubicacion.Equals(UbicacionFiltro, StringComparison.OrdinalIgnoreCase));

            Resultados.Clear();
            foreach (var e in filtrados)
                Resultados.Add(e);
        }
        finally { IsBusy = false; }
    }
}