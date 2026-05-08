using ControlEquiposElectronicos.Api.DTOs.Equipos;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface ISwitchPuertoService
{
    Task<List<SwitchPuertoDto>> ObtenerPorSwitchAsync(int switchEquipoId);
    Task<SwitchPuertoDto?> ObtenerPorIdAsync(int id);
    Task<SwitchPuertoDto> CrearAsync(int switchEquipoId, CrearSwitchPuertoDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarSwitchPuertoDto dto);
    Task<bool> EliminarAsync(int id);
}