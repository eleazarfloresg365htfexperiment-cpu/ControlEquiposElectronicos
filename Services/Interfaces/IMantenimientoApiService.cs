using ControlEquiposElectronicos.DTOs.Mantenimientos;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IMantenimientoApiService
{
    Task<List<MantenimientoListadoDto>> ObtenerTodosAsync();
}
