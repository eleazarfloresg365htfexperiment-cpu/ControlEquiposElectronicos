using ControlEquiposElectronicos.Api.DTOs.Usuarios;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IUsuarioService
{
    Task<List<UsuarioDto>> ObtenerTodosAsync();

    Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto);
}