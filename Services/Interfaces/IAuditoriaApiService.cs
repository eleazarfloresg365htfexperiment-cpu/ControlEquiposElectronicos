using ControlEquiposElectronicos.DTOs.Auditoria;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IAuditoriaApiService
{
    Task<List<AuditoriaDto>> ObtenerTodosAsync();
    Task<List<AuditoriaDto>> ObtenerPorModuloAsync(string modulo);
}