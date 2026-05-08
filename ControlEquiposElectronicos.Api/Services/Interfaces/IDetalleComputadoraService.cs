using ControlEquiposElectronicos.Api.DTOs.Equipos;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IDetalleComputadoraService
{
    Task<DetalleComputadoraDto?> ObtenerPorEquipoIdAsync(int equipoId);
    Task<DetalleComputadoraDto> CrearAsync(int equipoId, CrearDetalleComputadoraDto dto);
    Task<bool> ActualizarAsync(int equipoId, ActualizarDetalleComputadoraDto dto);
    Task<bool> EliminarAsync(int equipoId);
}