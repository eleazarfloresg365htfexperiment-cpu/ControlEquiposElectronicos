using ControlEquiposElectronicos.DTOs.Catalogos;
using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;

namespace ControlEquiposElectronicos.ViewModels;

public class EquiposViewModel : BaseViewModel
{
    private readonly IEquipoApiService _equipoApiService;
    private readonly ICatalogoApiService _catalogoApiService;

    private List<EquipoListadoDto> _todosLosEquipos = new();
    public ObservableCollection<EquipoListadoDto> Equipos { get; set; } = new();
    public ObservableCollection<CatalogoItemDto> Tipos { get; set; } = new();
    public ObservableCollection<CatalogoItemDto> Ubicaciones { get; set; } = new();
    public ObservableCollection<CatalogoItemDto> Estados { get; set; } = new();

    private string _textoBusqueda = string.Empty;
    public string TextoBusqueda
    {
        get => _textoBusqueda;
        set
        {
            _textoBusqueda = value;
            OnPropertyChanged();
            FiltrarEquipos();
        }
    }

    private CatalogoItemDto? _tipoSeleccionado;
    public CatalogoItemDto? TipoSeleccionado
    {
        get => _tipoSeleccionado;
        set
        {
            _tipoSeleccionado = value;
            OnPropertyChanged();
            FiltrarEquipos();
        }
    }

    private CatalogoItemDto? _ubicacionSeleccionada;
    public CatalogoItemDto? UbicacionSeleccionada
    {
        get => _ubicacionSeleccionada;
        set
        {
            _ubicacionSeleccionada = value;
            OnPropertyChanged();
            FiltrarEquipos();
        }
    }

    private CatalogoItemDto? _estadoSeleccionado;
    public CatalogoItemDto? EstadoSeleccionado
    {
        get => _estadoSeleccionado;
        set
        {
            _estadoSeleccionado = value;
            OnPropertyChanged();
            FiltrarEquipos();
        }
    }

    public EquiposViewModel(IEquipoApiService equipoApiService, ICatalogoApiService catalogoApiService)
    {
        Title = "Gestión de Equipos";
        _equipoApiService = equipoApiService;
        _catalogoApiService = catalogoApiService;
    }

    public async Task CargarEquiposAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var lista = await _equipoApiService.ObtenerTodosAsync();
            _todosLosEquipos = lista;
            Equipos.Clear();
            foreach (var equipo in lista)
                Equipos.Add(equipo);

            var tipos = await _catalogoApiService.ObtenerTiposEquipoAsync();
            Tipos.Clear();
            foreach (var item in tipos) Tipos.Add(item);

            var ubicaciones = await _catalogoApiService.ObtenerUbicacionesAsync();
            Ubicaciones.Clear();
            foreach (var item in ubicaciones) Ubicaciones.Add(item);

            var estados = await _catalogoApiService.ObtenerEstadosEquipoAsync();
            Estados.Clear();
            foreach (var item in estados) Estados.Add(item);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo cargar los equipos: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void FiltrarEquipos()
    {
        var filtrados = _todosLosEquipos.Where(e =>
            (string.IsNullOrWhiteSpace(TextoBusqueda) ||
             e.Codigo.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
             e.Nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
             e.Tipo.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
             e.Ubicacion.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase)) &&
            (TipoSeleccionado == null || e.Tipo == TipoSeleccionado.Nombre) &&
            (UbicacionSeleccionada == null || e.Ubicacion == UbicacionSeleccionada.Nombre) &&
            (EstadoSeleccionado == null || e.Estado == EstadoSeleccionado.Nombre)
        ).ToList();

        Equipos.Clear();
        foreach (var equipo in filtrados)
            Equipos.Add(equipo);
    }

    public void FiltrarPorEstado(string estado)
    {
        var filtrados = _todosLosEquipos
            .Where(e => e.Estado.Contains(estado, StringComparison.OrdinalIgnoreCase))
            .ToList();

        Equipos.Clear();
        foreach (var equipo in filtrados)
            Equipos.Add(equipo);
    }
}