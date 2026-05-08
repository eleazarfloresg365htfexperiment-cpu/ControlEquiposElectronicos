using ControlEquiposElectronicos.Api.DTOs.Equipos;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IDetalleRedService
{
    Task<DetalleRedDto?> ObtenerPorEquipoIdAsync(int equipoId);
    Task<DetalleRedDto> CrearAsync(int equipoId, CrearDetalleRedDto dto);
    Task<bool> ActualizarAsync(int equipoId, ActualizarDetalleRedDto dto);
    Task<bool> EliminarAsync(int equipoId);
}