using ControlEquiposElectronicos.DTOs.Catalogos;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

public class CatalogoApiService : ICatalogoApiService
{
    private readonly IApiService _apiService;

    public CatalogoApiService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<CatalogoItemDto>> ObtenerCategoriasAsync()
    {
        return await _apiService.GetAsync<List<CatalogoItemDto>>("CategoriasEquipo") ?? new();
    }

    public async Task<List<CatalogoItemDto>> ObtenerTiposEquipoAsync()
    {
        return await _apiService.GetAsync<List<CatalogoItemDto>>("TiposEquipo") ?? new();
    }

    public async Task<List<CatalogoItemDto>> ObtenerEstadosEquipoAsync()
    {
        return await _apiService.GetAsync<List<CatalogoItemDto>>("EstadosEquipo") ?? new();
    }

    public async Task<List<CatalogoItemDto>> ObtenerUbicacionesAsync()
    {
        return await _apiService.GetAsync<List<CatalogoItemDto>>("Ubicaciones") ?? new();
    }
}
