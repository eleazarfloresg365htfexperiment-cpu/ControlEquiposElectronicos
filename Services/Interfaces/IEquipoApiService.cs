using ControlEquiposElectronicos.DTOs.Equipos;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IEquipoApiService
{
    Task<List<EquipoListadoDto>> ObtenerTodosAsync();

    Task<EquipoListadoDto?> ObtenerPorIdAsync(int id);

    Task<EquipoListadoDto?> CrearAsync(CrearEquipoDto dto);

    Task<bool> ActualizarAsync(int id, ActualizarEquipoDto dto);

    Task<bool> EliminarAsync(int id);
    Task<List<EquipoPerifericoDto>> ObtenerPerifericosDePcAsync(int equipoPrincipalId);
    Task<EquipoPerifericoDto?> AsignarPerifericoAsync(AsignarPerifericoDto dto);
    Task<bool> QuitarPerifericoAsync(int asignacionId);
}