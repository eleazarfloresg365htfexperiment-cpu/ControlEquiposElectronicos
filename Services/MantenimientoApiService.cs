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
}
