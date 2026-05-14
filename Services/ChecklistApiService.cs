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
        return await _apiService.GetAsync<PrepararChecklistUbicacionDto>($"ChecklistTecnico/preparar/ubicacion/{ubicacionId}");
    }

    public async Task<ChecklistTecnicoDto?> IniciarAsync(IniciarChecklistTecnicoDto dto)
    {
        return await _apiService.PostAsync<IniciarChecklistTecnicoDto, ChecklistTecnicoDto>("ChecklistTecnico/iniciar", dto);
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
}
