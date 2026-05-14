using ControlEquiposElectronicos.DTOs.Usuarios;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

public class UsuarioApiService : IUsuarioApiService
{
    private readonly IApiService _apiService;

    public UsuarioApiService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<UsuarioListadoDto>> ObtenerTodosAsync()
    {
        return await _apiService.GetAsync<List<UsuarioListadoDto>>("Usuarios") ?? new();
    }
}
