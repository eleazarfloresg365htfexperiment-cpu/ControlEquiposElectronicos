using ControlEquiposElectronicos.Api.DTOs.ReportesFallas;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IReporteFallaService
{
    Task<List<ReporteFallaListadoDto>> ObtenerTodosAsync();
    Task<ReporteFallaDetalleDto?> ObtenerPorIdAsync(int id);
    Task<ReporteFallaDetalleDto> CrearAsync(CrearReporteFallaDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarReporteFallaDto dto);
    Task<bool> CambiarEstadoAsync(int id, CambiarEstadoReporteFallaDto dto);
}