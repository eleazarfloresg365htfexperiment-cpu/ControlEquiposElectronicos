using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

public class EquipoApiService : IEquipoApiService
{
    private readonly IApiService _apiService;

    public EquipoApiService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<EquipoListadoDto>> ObtenerTodosAsync()
    {
        return await _apiService.GetAsync<List<EquipoListadoDto>>("Equipos") ?? new();
    }

    public async Task<EquipoListadoDto?> ObtenerPorIdAsync(int id)
    {
        return await _apiService.GetAsync<EquipoListadoDto>($"Equipos/{id}");
    }

    public async Task<EquipoListadoDto?> CrearAsync(CrearEquipoDto dto)
    {
        return await _apiService.PostAsync<CrearEquipoDto, EquipoListadoDto>("Equipos", dto);
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarEquipoDto dto)
    {
        return await _apiService.PutAsync($"Equipos/{id}", dto);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        return await _apiService.DeleteAsync($"Equipos/{id}");
    }
}