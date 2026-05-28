using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

public class ChecklistApiService : IChecklistApiService
{
    private readonly IApiService _apiService;

    public ChecklistApiService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<ChecklistTecnicoDto>> ObtenerTodosAsync()
    {
        return await _apiService.GetAsync<List<ChecklistTecnicoDto>>("ChecklistTecnico") ?? new();
    }

    public async Task<ChecklistTecnicoDto?> ObtenerPorIdAsync(int id)
    {
        return await _apiService.GetAsync<ChecklistTecnicoDto>($"ChecklistTecnico/{id}");
    }

    public async Task<PrepararChecklistUbicacionDto?> PrepararPorUbicacionAsync(int ubicacionId)
    {
        var resultado = await _apiService.GetAsync<PrepararChecklistUbicacionDto>($"ChecklistTecnico/preparar/ubicacion/{ubicacionId}");
        if (resultado != null)
            return resultado;

        // Compatibilidad con variantes de ruta en diferentes despliegues de API.
        return await _apiService.GetAsync<PrepararChecklistUbicacionDto>($"ChecklistTecnico/ubicacion/{ubicacionId}/preparar");
    }

    public async Task<ChecklistTecnicoDto?> IniciarAsync(IniciarChecklistTecnicoDto dto)
    {
        var (checklist, _) = await IniciarConDetalleAsync(dto);
        return checklist;
    }

    public async Task<(ChecklistTecnicoDto? Checklist, string? ErrorMessage)> IniciarConDetalleAsync(
        IniciarChecklistTecnicoDto dto)
    {
        return await _apiService.PostWithErrorAsync<IniciarChecklistTecnicoDto, ChecklistTecnicoDto>(
            "ChecklistTecnico/iniciar",
            dto);
    }

    public async Task<bool> GuardarAvanceAsync(int id, GuardarChecklistTecnicoDto dto)
    {
        return await _apiService.PatchAsync($"ChecklistTecnico/{id}/guardar-avance", dto);
    }

    public async Task<bool> FinalizarAsync(int id, FinalizarChecklistTecnicoDto dto)
    {
        return await _apiService.PatchAsync($"ChecklistTecnico/{id}/finalizar", dto);
    }

    public async Task<List<PlantillaChecklistDto>> ObtenerPlantillasAsync()
    {
        return await _apiService.GetAsync<List<PlantillaChecklistDto>>("PlantillasChecklist") ?? new();
    }

    public async Task<PlantillaChecklistDto?> CrearPlantillaAsync(CrearPlantillaChecklistDto dto)
    {
        return await _apiService.PostAsync<CrearPlantillaChecklistDto, PlantillaChecklistDto>("PlantillasChecklist", dto);
    }

    public async Task<PlantillaChecklistItemDto?> AgregarItemPlantillaAsync(int plantillaId, CrearPlantillaChecklistItemDto dto)
    {
        return await _apiService.PostAsync<CrearPlantillaChecklistItemDto, PlantillaChecklistItemDto>($"PlantillasChecklist/{plantillaId}/items", dto);
    }
}
