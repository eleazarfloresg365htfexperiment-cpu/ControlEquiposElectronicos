using ControlEquiposElectronicos.DTOs.Mantenimientos;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

public class MantenimientoApiService : IMantenimientoApiService
{
    private readonly IApiService _apiService;

    public MantenimientoApiService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<MantenimientoListadoDto>> ObtenerTodosAsync()
    {
        return await _apiService.GetAsync<List<MantenimientoListadoDto>>("Mantenimientos") ?? new();
    }

    public async Task<bool> CrearAsync(CrearMantenimientoDto dto)
    {
        var resultado = await _apiService.PostAsync<CrearMantenimientoDto, MantenimientoListadoDto>("Mantenimientos", dto);
        return resultado != null;
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarMantenimientoDto dto)
    {
        return await _apiService.PutAsync($"Mantenimientos/{id}", dto);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        return await _apiService.DeleteAsync($"Mantenimientos/{id}");
    }
}