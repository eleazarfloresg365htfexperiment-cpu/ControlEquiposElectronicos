using ControlEquiposElectronicos.Api.DTOs.Checklist;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IPlantillaChecklistService
{
    Task<List<PlantillaChecklistDto>> ObtenerTodasAsync();

    Task<PlantillaChecklistDto?> ObtenerPorIdAsync(int id);

    Task<PlantillaChecklistDto?> ObtenerPorTipoEquipoAsync(int tipoEquipoId);

    Task<PlantillaChecklistDto> CrearAsync(CrearPlantillaChecklistDto dto);

    Task<bool> ActualizarAsync(int id, ActualizarPlantillaChecklistDto dto);

    Task<bool> DesactivarAsync(int id);

    Task<List<PlantillaChecklistItemDto>> ObtenerItemsAsync(int plantillaId);

    Task<PlantillaChecklistItemDto> AgregarItemAsync(int plantillaId, CrearPlantillaChecklistItemDto dto);

    Task<bool> ActualizarItemAsync(int itemId, ActualizarPlantillaChecklistItemDto dto);

    Task<bool> DesactivarItemAsync(int itemId);
}