using ControlEquiposElectronicos.Api.DTOs.Equipos;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IDetalleUPSService
{
    Task<DetalleUPSDto?> ObtenerPorEquipoIdAsync(int equipoId);
    Task<DetalleUPSDto> CrearAsync(int equipoId, CrearDetalleUPSDto dto);
    Task<bool> ActualizarAsync(int equipoId, ActualizarDetalleUPSDto dto);
    Task<bool> EliminarAsync(int equipoId);
}
