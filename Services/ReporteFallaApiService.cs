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

    public async Task<bool> CrearAsync(CrearReporteFallaDto dto)
    {
        var resultado = await _apiService.PostAsync<CrearReporteFallaDto, ReporteFallaListadoDto>("ReportesFalla", dto);
        return resultado != null;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        return await _apiService.DeleteAsync($"ReportesFalla/{id}");
    }
}