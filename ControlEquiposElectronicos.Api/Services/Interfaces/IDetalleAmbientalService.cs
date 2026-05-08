using ControlEquiposElectronicos.Api.DTOs.Equipos;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IDetalleAmbientalService
{
    Task<DetalleAmbientalDto?> ObtenerPorEquipoIdAsync(int equipoId);
    Task<DetalleAmbientalDto> CrearAsync(int equipoId, CrearDetalleAmbientalDto dto);
    Task<bool> ActualizarAsync(int equipoId, ActualizarDetalleAmbientalDto dto);
    Task<bool> EliminarAsync(int equipoId);
}