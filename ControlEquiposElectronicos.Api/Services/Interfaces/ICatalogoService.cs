using ControlEquiposElectronicos.Api.DTOs.Catalogos;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface ICatalogoService
{
    Task<List<CategoriaEquipoDto>> ObtenerCategoriasEquipoAsync();
    Task<List<TipoEquipoDto>> ObtenerTiposEquipoAsync();
    Task<List<EstadoEquipoDto>> ObtenerEstadosEquipoAsync();
    Task<List<UbicacionDto>> ObtenerUbicacionesAsync();
    Task<List<EstadoReporteDto>> ObtenerEstadosReporteAsync();
    Task<List<TipoMantenimientoDto>> ObtenerTiposMantenimientoAsync();
}