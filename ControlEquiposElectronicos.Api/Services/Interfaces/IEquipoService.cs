using ControlEquiposElectronicos.Api.DTOs.Equipos;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IEquipoService
{
    Task<List<EquipoListadoDto>> ObtenerTodosAsync();
    Task<EquipoDetalleDto?> ObtenerPorIdAsync(int id);
    Task<EquipoDetalleDto> CrearAsync(CrearEquipoDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarEquipoDto dto);
    Task<bool> DesactivarAsync(int id);
}