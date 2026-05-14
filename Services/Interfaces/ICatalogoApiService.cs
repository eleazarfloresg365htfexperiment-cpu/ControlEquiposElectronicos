using ControlEquiposElectronicos.DTOs.Catalogos;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface ICatalogoApiService
{
    Task<List<CatalogoItemDto>> ObtenerCategoriasAsync();
    Task<List<CatalogoItemDto>> ObtenerTiposEquipoAsync();
    Task<List<CatalogoItemDto>> ObtenerEstadosEquipoAsync();
    Task<List<CatalogoItemDto>> ObtenerUbicacionesAsync();
}
