using ControlEquiposElectronicos.DTOs.Mantenimientos;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IMantenimientoApiService
{
    Task<List<MantenimientoListadoDto>> ObtenerTodosAsync();
    Task<bool> CrearAsync(CrearMantenimientoDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarMantenimientoDto dto);
    Task<bool> EliminarAsync(int id);
}