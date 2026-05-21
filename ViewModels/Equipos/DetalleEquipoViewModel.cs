using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class DetalleEquiposViewModel : BaseViewModel
{
    private readonly IEquipoApiService _equipoApiService;
    private EquipoListadoDto? _equipo;
    public EquipoListadoDto? Equipo
    {
        get => _equipo;
        set
        {
            _equipo = value;
            OnPropertyChanged();
        }
    }
    public DetalleEquiposViewModel(IEquipoApiService equipoApiService)
    {
        Title = "Detalle del equipo";
        _equipoApiService = equipoApiService;
    }
    
    public async Task CargarEquipoAsync(int id)
    {
        try
        {
            IsBusy = true;
            Equipo = await _equipoApiService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo cargar el equipo: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
    public async Task<bool> DesactivarEquipoAsync()
    {
        if (Equipo == null) return false;
        try
        {
            IsBusy = true;
            return await _equipoApiService.EliminarAsync(Equipo.Id);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo desactivar el equipo: {ex.Message}", "OK");
            return false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task CambiarEstadoAsync(string nuevoEstado)
    {
        if (Equipo == null) return;
        try
        {
            IsBusy = true;
            var dto = new ActualizarEquipoDto
            {
                Codigo = Equipo.Codigo,
                Nombre = Equipo.Nombre,
                Marca = Equipo.Marca ?? string.Empty,
                Modelo = Equipo.Modelo ?? string.Empty,
                NumeroSerie = Equipo.NumeroSerie ?? string.Empty,
                Observaciones = Equipo.Observaciones ?? string.Empty,
                CategoriaEquipoId = Equipo.CategoriaEquipoId,
                TipoEquipoId = Equipo.TipoEquipoId,
                EstadoEquipoId = Equipo.EstadoEquipoId,
                UbicacionId = Equipo.UbicacionId,
                Activo = true
            };
            await _equipoApiService.ActualizarAsync(Equipo.Id, dto);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo cambiar el estado: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}