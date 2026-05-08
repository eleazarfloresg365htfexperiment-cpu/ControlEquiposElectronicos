using ControlEquiposElectronicos.Api.DTOs.Equipos;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IDetalleImpresoraService
{
    Task<DetalleImpresoraDto?> ObtenerPorEquipoIdAsync(int equipoId);
    Task<DetalleImpresoraDto> CrearAsync(int equipoId, CrearDetalleImpresoraDto dto);
    Task<bool> ActualizarAsync(int equipoId, ActualizarDetalleImpresoraDto dto);
    Task<bool> EliminarAsync(int equipoId);
}