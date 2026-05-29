using ControlEquiposElectronicos.DTOs.Auditoria;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IAuditoriaApiService
{
    Task<List<HistorialOperacionDto>> ObtenerTodosAsync();
    Task<List<HistorialOperacionDto>> ObtenerPorModuloAsync(string modulo);
}