using ControlEquiposElectronicos.DTOs.Reportes;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IExportService
{
    Task<string> ExportarReportesExcelAsync(List<ReporteFallaListadoDto> reportes);
    Task<string> ExportarReportesPdfAsync(List<ReporteFallaListadoDto> reportes);
}