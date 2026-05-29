using ControlEquiposElectronicos.DTOs.Usuarios;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IUsuarioApiService
{
    Task<List<UsuarioListadoDto>> ObtenerTodosAsync();

    // Crea un usuario en la API. Devuelve true si se creó correctamente.
    Task<bool> CrearAsync(CrearUsuarioDto usuario);
}
