using ControlEquiposElectronicos.DTOs.Catalogos;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.Services.Interfaces;
using Microsoft.Maui.Layouts;
using System.Collections.ObjectModel;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class EquipoFormularioViewModel : BaseViewModel
{
    private readonly IEquipoApiService _equipoApiService;
    private readonly ICatalogoApiService _catalagoApiService;

    public ObservableCollection<CatalogoItemDto> Categorias { get; set; } = new();
    public ObservableCollection<CatalogoItemDto> Tipos { get; set; } = new();
    public ObservableCollection<CatalogoItemDto> Estados { get; set; } = new();
    public ObservableCollection<CatalogoItemDto> Ubicaciones { get; set; } = new();

    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set;  } = string.Empty;
    public string NumeroSerie { get; set;  } = string.Empty;
    public string Observacones { get; set;  } = string.Empty;

    public CatalogoItemDto? CategoriaSeleccionada { get; set; }
    public CatalogoItemDto? TipoSeleccionado { get; set; }
    public CatalogoItemDto? EstadoSeleccionado { get; set; }
    public CatalogoItemDto? UbicacionSeleccionada { get; set; }

    public EquipoFormularioViewModel(IEquipoApiService equipoApiService, ICatalogoApiService catalogoApiService)
    {
        Title = "Registrar equipo";
            _equipoApiService = equipoApiService;
            _catalagoApiService = catalogoApiService;
    }

    public async Task CargarCatalogosAsync()
    {
        try
        {
            IsBusy = true;
            var categorias = await _catalagoApiService.ObtenerCategoriasAsync();
            var tipos = await _catalagoApiService.ObtenerTiposEquipoAsync();
            var estados = await _catalagoApiService.ObtenerEstadosEquipoAsync();
            var ubicaciones = await _catalagoApiService.ObtenerUbicacionesAsync();

            Categorias.Clear();
            foreach (var item in categorias) Categorias.Add(item);
            Tipos.Clear();
            foreach (var item in tipos) Tipos.Add(item);
            Estados.Clear();
            foreach (var item in estados) Estados.Add(item);
            Ubicaciones.Clear();
            foreach (var item in ubicaciones) Ubicaciones.Add(item);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudieron cargar los catálogos: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task<bool> GuardarEquiposAsync()
    {
        if (string.IsNullOrWhiteSpace(Codigo) || string.IsNullOrWhiteSpace(Nombre) ||
            string.IsNullOrWhiteSpace(Marca) || CategoriaSeleccionada == null ||
            TipoSeleccionado == null || EstadoSeleccionado == null || UbicacionSeleccionada == null)
        {
            await Shell.Current.DisplayAlert("Campos incompletos", "Por favor completa todos los campos obligatorios", "OK");
            return false;
        }
        try
        {
            IsBusy = true;
            var dto = new CrearEquipoDto
            {
                Codigo = Codigo,
                Nombre = Nombre,
                Marca = Marca,
                Modelo = Modelo,
                NumeroSerie = NumeroSerie,
                Observaciones = Observacones,
                CategoriaEquipoId = CategoriaSeleccionada.Id,
                TipoEquipoId = TipoSeleccionado.Id,
                EstadoEquipoId = EstadoSeleccionado.Id,
                UbicacionId = UbicacionSeleccionada.Id,
                Activo = true
            };
            var resultado = await _equipoApiService.CrearAsync(dto);
            return resultado != null;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo guardar el equipo: {ex.Message}", "OK");
            return false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void LimpiarFormulario()
    {
        Codigo = string.Empty;
        Nombre = string.Empty;
        Marca = string.Empty;
        Modelo = string.Empty;
        NumeroSerie = string.Empty;
        Observacones = string.Empty;
        CategoriaSeleccionada = null;
        TipoSeleccionado = null;
        EstadoSeleccionado = null;
        UbicacionSeleccionada = null;
    }

    public void CargarDatosParaEditar(EquipoListadoDto equipo)
    {
        Codigo = equipo.Codigo;
        Nombre = equipo.Nombre;
        Marca = equipo.Marca ?? string.Empty;
        Modelo = equipo.Modelo ?? string.Empty;
        NumeroSerie = equipo.NumeroSerie ?? string.Empty;
        Observacones = equipo.Observaciones ?? string.Empty;
    }

    public async Task<bool> ActualizarEquipoAsync(int id)
    {
        if (string.IsNullOrWhiteSpace(Codigo) || string.IsNullOrWhiteSpace(Nombre) ||
            string.IsNullOrWhiteSpace(Marca) || CategoriaSeleccionada == null ||
            TipoSeleccionado == null || EstadoSeleccionado == null || UbicacionSeleccionada == null)
        {
            await Shell.Current.DisplayAlert("Campos incompletos", "Por favor llena todos los campos obligatorios", "OK");
            return false;
        }
        try
        {
            IsBusy = true;
            var dto = new ActualizarEquipoDto
            {
                Codigo = Codigo,
                Nombre = Nombre,
                Marca = Marca,
                Modelo = Modelo,
                NumeroSerie = NumeroSerie,
                Observaciones = Observacones,
                CategoriaEquipoId = CategoriaSeleccionada.Id,
                TipoEquipoId = TipoSeleccionado.Id,
                EstadoEquipoId = EstadoSeleccionado.Id,
                UbicacionId = UbicacionSeleccionada.Id,
                Activo = true
            };

            return await _equipoApiService.ActualizarAsync(id, dto);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo actualiar el equipo: {ex.Message}", "OK");
            return false;
        }
        finally
        {
            IsBusy = false;
        }
    }
    public async Task<ControlEquiposElectronicos.DTOs.Equipos.EquipoListadoDto?> ObtenerEquipoPorIdAsync(int id)
    {
        try
        {
            return await _equipoApiService.ObtenerPorIdAsync(id);
        }
        catch
        {
            return null;
        }
    }
}