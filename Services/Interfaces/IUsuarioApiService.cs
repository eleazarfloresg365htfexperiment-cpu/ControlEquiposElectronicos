using ControlEquiposElectronicos.DTOs.Usuarios;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IUsuarioApiService
{
    Task<List<UsuarioListadoDto>> ObtenerTodosAsync();

    Task<bool> CrearAsync(CrearUsuarioDto usuario);

    // Devuelve (éxito, mensajeError). El error viene directo de la API.
    Task<(bool Exito, string? Error)> CambiarRolAsync(int usuarioId, string nuevoRol);
}
