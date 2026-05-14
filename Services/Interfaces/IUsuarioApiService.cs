using ControlEquiposElectronicos.DTOs.Usuarios;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IUsuarioApiService
{
    Task<List<UsuarioListadoDto>> ObtenerTodosAsync();
}
