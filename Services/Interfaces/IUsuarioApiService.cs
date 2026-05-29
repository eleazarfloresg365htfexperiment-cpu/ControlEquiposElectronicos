using ControlEquiposElectronicos.DTOs.Usuarios;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IUsuarioApiService
{
    Task<List<UsuarioListadoDto>> ObtenerTodosAsync();

    // Crea un usuario en la API. Devuelve true si se creó correctamente.
    Task<bool> CrearAsync(CrearUsuarioDto usuario);

    // Cambia el rol de un usuario. Devuelve true si se actualizó correctamente.
    Task<bool> CambiarRolAsync(int usuarioId, string nuevoRol);
}
