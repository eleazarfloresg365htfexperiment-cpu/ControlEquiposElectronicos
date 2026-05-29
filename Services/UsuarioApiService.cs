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
        var creado = await _apiService.PostAsync<CrearUsuarioDto, UsuarioListadoDto>("Usuarios", usuario);
        return creado != null;
    }

    // Llama PATCH api/Usuarios/{id}/rol y devuelve el error exacto si falla
    public async Task<(bool Exito, string? Error)> CambiarRolAsync(int usuarioId, string nuevoRol)
    {
        var dto = new ActualizarRolUsuarioDto { Rol = nuevoRol };
        return await _apiService.PatchWithErrorAsync($"Usuarios/{usuarioId}/rol", dto);
    }
}
 