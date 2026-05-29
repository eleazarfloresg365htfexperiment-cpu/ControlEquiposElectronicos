using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.EquipoPerifericos;
using ControlEquiposElectronicos.Api.Entities.Auditoria;
using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class EquipoPerifericoService : IEquipoPerifericoService
{
    private readonly AppDbContext _context;
    private readonly IUsuarioActualService _usuarioActualService;

    public EquipoPerifericoService(
        AppDbContext context,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<List<EquipoPerifericoDto>> ObtenerPorEquipoPrincipalAsync(int equipoPrincipalId)
    {
        return await _context.EquipoPerifericos
            .Include(ep => ep.EquipoPrincipal)
            .Include(ep => ep.Periferico)
            .Where(ep => ep.EquipoPrincipalId == equipoPrincipalId && ep.Activo)
            .OrderBy(ep => ep.Periferico.Codigo)
            .Select(ep => new EquipoPerifericoDto
            {
                Id = ep.Id,
                EquipoPrincipalId = ep.EquipoPrincipalId,
                CodigoEquipoPrincipal = ep.EquipoPrincipal.Codigo,
                NombreEquipoPrincipal = ep.EquipoPrincipal.Nombre,
                PerifericoId = ep.PerifericoId,
                CodigoPeriferico = ep.Periferico.Codigo,
                NombrePeriferico = ep.Periferico.Nombre,
                FechaAsignacion = ep.FechaAsignacion,
                FechaDesasignacion = ep.FechaDesasignacion,
                Observaciones = ep.Observaciones,
                Activo = ep.Activo
            })
            .ToListAsync();
    }

    public async Task<List<EquipoPerifericoDto>> ObtenerPorPerifericoAsync(int perifericoId)
    {
        return await _context.EquipoPerifericos
            .Include(ep => ep.EquipoPrincipal)
            .Include(ep => ep.Periferico)
            .Where(ep => ep.PerifericoId == perifericoId)
            .OrderByDescending(ep => ep.FechaAsignacion)
            .Select(ep => new EquipoPerifericoDto
            {
                Id = ep.Id,
                EquipoPrincipalId = ep.EquipoPrincipalId,
                CodigoEquipoPrincipal = ep.EquipoPrincipal.Codigo,
                NombreEquipoPrincipal = ep.EquipoPrincipal.Nombre,
                PerifericoId = ep.PerifericoId,
                CodigoPeriferico = ep.Periferico.Codigo,
                NombrePeriferico = ep.Periferico.Nombre,
                FechaAsignacion = ep.FechaAsignacion,
                FechaDesasignacion = ep.FechaDesasignacion,
                Observaciones = ep.Observaciones,
                Activo = ep.Activo
            })
            .ToListAsync();
    }

    public async Task<EquipoPerifericoDto> AsignarAsync(AsignarPerifericoDto dto)
    {
        if (dto.EquipoPrincipalId == dto.PerifericoId)
            throw new InvalidOperationException("Un equipo no puede asignarse como periférico de sí mismo.");

        var equipoPrincipal = await _context.Equipos
            .FirstOrDefaultAsync(e => e.Id == dto.EquipoPrincipalId && e.Activo);

        if (equipoPrincipal == null)
            throw new InvalidOperationException("El equipo principal no existe o está inactivo.");

        var periferico = await _context.Equipos
            .FirstOrDefaultAsync(e => e.Id == dto.PerifericoId && e.Activo);

        if (periferico == null)
            throw new InvalidOperationException("El periférico no existe o está inactivo.");

        var yaAsignado = await _context.EquipoPerifericos
            .AnyAsync(ep =>
                ep.EquipoPrincipalId == dto.EquipoPrincipalId &&
                ep.PerifericoId == dto.PerifericoId &&
                ep.Activo);

        if (yaAsignado)
            throw new InvalidOperationException("Este periférico ya está asignado a esa PC.");

        var perifericoAsignadoAOtroEquipo = await _context.EquipoPerifericos
            .AnyAsync(ep =>
                ep.PerifericoId == dto.PerifericoId &&
                ep.Activo);

        if (perifericoAsignadoAOtroEquipo)
            throw new InvalidOperationException("Este periférico ya está asignado a otro equipo.");

        var asignacion = new EquipoPeriferico
        {
            EquipoPrincipalId = dto.EquipoPrincipalId,
            PerifericoId = dto.PerifericoId,
            FechaAsignacion = DateTime.UtcNow,
            Observaciones = dto.Observaciones?.Trim(),
            Activo = true
        };

        _context.EquipoPerifericos.Add(asignacion);

        await _context.SaveChangesAsync();

        await RegistrarAuditoriaAsync(
            "Asignación de periférico",
            asignacion.Id,
            $"Se asignó el periférico '{periferico.Codigo} - {periferico.Nombre}' al equipo principal '{equipoPrincipal.Codigo} - {equipoPrincipal.Nombre}'.");

        return new EquipoPerifericoDto
        {
            Id = asignacion.Id,
            EquipoPrincipalId = equipoPrincipal.Id,
            CodigoEquipoPrincipal = equipoPrincipal.Codigo,
            NombreEquipoPrincipal = equipoPrincipal.Nombre,
            PerifericoId = periferico.Id,
            CodigoPeriferico = periferico.Codigo,
            NombrePeriferico = periferico.Nombre,
            FechaAsignacion = asignacion.FechaAsignacion,
            FechaDesasignacion = asignacion.FechaDesasignacion,
            Observaciones = asignacion.Observaciones,
            Activo = asignacion.Activo
        };
    }

    public async Task<bool> QuitarAsignacionAsync(int id)
    {
        var asignacion = await _context.EquipoPerifericos
            .Include(ep => ep.EquipoPrincipal)
            .Include(ep => ep.Periferico)
            .FirstOrDefaultAsync(ep => ep.Id == id && ep.Activo);

        if (asignacion == null)
            return false;

        asignacion.Activo = false;
        asignacion.FechaDesasignacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await RegistrarAuditoriaAsync(
            "Desasignación de periférico",
            asignacion.Id,
            $"Se quitó el periférico '{asignacion.Periferico.Codigo} - {asignacion.Periferico.Nombre}' del equipo principal '{asignacion.EquipoPrincipal.Codigo} - {asignacion.EquipoPrincipal.Nombre}'.");

        return true;
    }

    private async Task RegistrarAuditoriaAsync(string accion, int registroId, string descripcion)
    {
        var historial = new HistorialOperacion
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = accion,
            Modulo = "Equipos",
            TablaAfectada = "EquipoPerifericos",
            RegistroId = registroId,
            Descripcion = descripcion,
            DireccionIP = _usuarioActualService.ObtenerDireccionIP(),
            FechaOperacion = DateTime.UtcNow
        };

        _context.HistorialOperaciones.Add(historial);

        await _context.SaveChangesAsync();
    }
}