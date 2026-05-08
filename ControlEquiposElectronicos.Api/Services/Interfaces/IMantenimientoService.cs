using ControlEquiposElectronicos.Api.DTOs.Mantenimientos;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IMantenimientoService
{
    Task<List<MantenimientoListadoDto>> ObtenerTodosAsync();
    Task<MantenimientoDetalleDto?> ObtenerPorIdAsync(int id);
    Task<List<MantenimientoListadoDto>> ObtenerPorEquipoAsync(int equipoId);

    Task<MantenimientoDetalleDto> CrearAsync(CrearMantenimientoDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarMantenimientoDto dto);
    Task<bool> FinalizarAsync(int id, FinalizarMantenimientoDto dto);

    Task<MantenimientoRepuestoDto> AgregarRepuestoAsync(int mantenimientoId, CrearMantenimientoRepuestoDto dto);
    Task<bool> EliminarRepuestoAsync(int repuestoId);
}
