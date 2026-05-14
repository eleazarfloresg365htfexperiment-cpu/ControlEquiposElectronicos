using ControlEquiposElectronicos.DTOs.Reportes;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IReporteFallaApiService
{
    Task<List<ReporteFallaListadoDto>> ObtenerTodosAsync();
}
