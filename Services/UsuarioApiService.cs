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

    public async Task<bool> CrearAsync(CrearUsuarioDto usuario)
    {
        // La API devuelve el usuario creado; si no es null, se creó correctamente
        var creado = await _apiService.PostAsync<CrearUsuarioDto, UsuarioListadoDto>("Usuarios", usuario);
        return creado != null;
    }
}
 