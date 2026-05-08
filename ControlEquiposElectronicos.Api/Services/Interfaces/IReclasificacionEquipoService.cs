using ControlEquiposElectronicos.Api.DTOs.Reclasificaciones;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IReclasificacionEquipoService
{
    Task<List<ReclasificacionEquipoListadoDto>> ObtenerTodasAsync();
    Task<ReclasificacionEquipoDetalleDto?> ObtenerPorIdAsync(int id);
    Task<List<ReclasificacionEquipoListadoDto>> ObtenerPorEquipoAsync(int equipoId);

    Task<ReclasificacionEquipoDetalleDto> CrearAsync(CrearReclasificacionEquipoDto dto);

    Task<List<HistorialEstadoEquipoDto>> ObtenerHistorialEstadosPorEquipoAsync(int equipoId);
}