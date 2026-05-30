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

    public async Task<(bool Success, string? ErrorMessage)> CrearAsync(CrearReporteFallaDto dto)
    {
        var (response, error) = await _apiService.PostWithErrorAsync<CrearReporteFallaDto, ReporteFallaDetalleDto>(
            "ReportesFalla", dto);

        if (response != null && response.Id > 0)
            return (true, null);

        if (!string.IsNullOrWhiteSpace(error))
            return (false, error);

        return (false, "No se pudo registrar el reporte. Verifica que la API esté en ejecución.");
    }

    public async Task<bool> EliminarAsync(int id)
    {
        return await _apiService.DeleteAsync($"ReportesFalla/{id}");
    }
}
