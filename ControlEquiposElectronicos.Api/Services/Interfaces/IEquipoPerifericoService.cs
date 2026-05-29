using ControlEquiposElectronicos.Api.DTOs.EquipoPerifericos;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IEquipoPerifericoService
{
    Task<List<EquipoPerifericoDto>> ObtenerPorEquipoPrincipalAsync(int equipoPrincipalId);

    Task<List<EquipoPerifericoDto>> ObtenerPorPerifericoAsync(int perifericoId);

    Task<EquipoPerifericoDto> AsignarAsync(AsignarPerifericoDto dto);

    Task<bool> QuitarAsignacionAsync(int id);
}