using ControlEquiposElectronicos.DTOs.Reportes;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IReporteFallaApiService
{
    Task<List<ReporteFallaListadoDto>> ObtenerTodosAsync();
    Task<(bool Success, string? ErrorMessage)> CrearAsync(CrearReporteFallaDto dto);
    Task<bool> EliminarAsync(int id);
}
