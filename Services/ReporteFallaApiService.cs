using ControlEquiposElectronicos.DTOs.Reportes;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

public class ReporteFallaApiService : IReporteFallaApiService
{
    private readonly IApiService _apiService;

    public ReporteFallaApiService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<ReporteFallaListadoDto>> ObtenerTodosAsync()
    {
        return await _apiService.GetAsync<List<ReporteFallaListadoDto>>("ReportesFalla") ?? new();
    }
}
