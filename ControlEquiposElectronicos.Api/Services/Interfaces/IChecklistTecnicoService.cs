using ControlEquiposElectronicos.Api.DTOs.Checklist;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IChecklistTecnicoService
{
    Task<List<ChecklistTecnicoDto>> ObtenerTodosAsync();

    Task<ChecklistTecnicoDto?> ObtenerPorIdAsync(int id);

    Task<List<ChecklistTecnicoDto>> ObtenerPorUbicacionAsync(int ubicacionId);

    Task<List<ChecklistTecnicoDto>> ObtenerPorTecnicoAsync(int tecnicoId);

    Task<List<ChecklistTecnicoDto>> ObtenerPorEquipoAsync(int equipoId);

    Task<PrepararChecklistUbicacionDto> PrepararPorUbicacionAsync(int ubicacionId);

    Task<ChecklistTecnicoDto> IniciarAsync(IniciarChecklistTecnicoDto dto);

    Task<bool> GuardarAvanceAsync(int id, GuardarChecklistTecnicoDto dto);

    Task<bool> FinalizarAsync(int id, FinalizarChecklistTecnicoDto dto);

    Task<bool> CancelarAsync(int id);
}