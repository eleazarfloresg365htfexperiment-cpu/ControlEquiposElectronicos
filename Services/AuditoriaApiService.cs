using ControlEquiposElectronicos.DTOs.Auditoria;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

public class AuditoriaApiService : IAuditoriaApiService
{
    private readonly IApiService _apiService;

    public AuditoriaApiService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<HistorialOperacionDto>> ObtenerTodosAsync()
    {
        return await _apiService.GetAsync<List<HistorialOperacionDto>>("Auditoria") ?? new();
    }

    public async Task<List<HistorialOperacionDto>> ObtenerPorModuloAsync(string modulo)
    {
        return await _apiService.GetAsync<List<HistorialOperacionDto>>($"Auditoria/modulo/{modulo}") ?? new();
    }
}
