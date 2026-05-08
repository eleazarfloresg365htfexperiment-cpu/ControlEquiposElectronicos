using ControlEquiposElectronicos.Api.DTOs.Auditoria;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IAuditoriaService
{
    Task<List<HistorialOperacionDto>> ObtenerTodosAsync();
    Task<HistorialOperacionDto?> ObtenerPorIdAsync(int id);
    Task<List<HistorialOperacionDto>> ObtenerPorUsuarioAsync(int usuarioId);
    Task<List<HistorialOperacionDto>> ObtenerPorModuloAsync(string modulo);

    Task<HistorialOperacionDto> RegistrarAsync(CrearHistorialOperacionDto dto);
}