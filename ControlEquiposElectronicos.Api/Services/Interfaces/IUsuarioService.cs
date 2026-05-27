using ControlEquiposElectronicos.Api.DTOs.Usuarios;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IUsuarioService
{
    Task<List<UsuarioDto>> ObtenerTodosAsync();

    Task<UsuarioDto?> ObtenerPorIdAsync(int id);

    Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto);
}