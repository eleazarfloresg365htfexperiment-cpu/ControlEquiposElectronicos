using ControlEquiposElectronicos.DTOs.Checklist;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IChecklistApiService
{
    Task<List<ChecklistTecnicoDto>> ObtenerTodosAsync();
    Task<ChecklistTecnicoDto?> ObtenerPorIdAsync(int id);
    Task<PrepararChecklistUbicacionDto?> PrepararPorUbicacionAsync(int ubicacionId);
    Task<ChecklistTecnicoDto?> IniciarAsync(IniciarChecklistTecnicoDto dto);
    Task<(ChecklistTecnicoDto? Checklist, string? ErrorMessage)> IniciarConDetalleAsync(IniciarChecklistTecnicoDto dto);
    Task<bool> GuardarAvanceAsync(int id, GuardarChecklistTecnicoDto dto);
    Task<bool> FinalizarAsync(int id, FinalizarChecklistTecnicoDto dto);

    Task<List<PlantillaChecklistDto>> ObtenerPlantillasAsync();
    Task<PlantillaChecklistDto?> CrearPlantillaAsync(CrearPlantillaChecklistDto dto);
    Task<(PlantillaChecklistDto? Plantilla, string? ErrorMessage)> CrearPlantillaConDetalleAsync(CrearPlantillaChecklistDto dto);
    Task<PlantillaChecklistItemDto?> AgregarItemPlantillaAsync(int plantillaId, CrearPlantillaChecklistItemDto dto);
}
