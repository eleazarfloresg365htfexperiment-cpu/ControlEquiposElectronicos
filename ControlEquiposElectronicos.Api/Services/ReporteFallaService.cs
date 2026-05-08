using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.ReportesFallas;
using ControlEquiposElectronicos.Api.Entities.Reportes;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class ReporteFallaService : IReporteFallaService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public ReporteFallaService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<List<ReporteFallaListadoDto>> ObtenerTodosAsync()
    {
        var query =
            from reporte in _context.ReportesFalla
            join equipo in _context.Equipos on reporte.EquipoId equals equipo.Id
            join usuario in _context.Usuarios on reporte.UsuarioReportaId equals usuario.UsuarioId
            join estado in _context.EstadosReporte on reporte.EstadoReporteId equals estado.Id
            orderby reporte.FechaReporte descending
            select new ReporteFallaListadoDto
            {
                Id = reporte.Id,

                EquipoId = equipo.Id,
                CodigoEquipo = equipo.Codigo,
                NombreEquipo = equipo.Nombre,

                UsuarioReportaId = usuario.UsuarioId,
                UsuarioReporta = usuario.Nickname,

                EstadoReporteId = estado.Id,
                Estado = estado.Nombre,

                Titulo = reporte.Titulo,
                Prioridad = reporte.Prioridad,

                FechaReporte = reporte.FechaReporte,
                FechaCierre = reporte.FechaCierre
            };

        return await query.ToListAsync();
    }

    public async Task<ReporteFallaDetalleDto?> ObtenerPorIdAsync(int id)
    {
        var query =
            from reporte in _context.ReportesFalla
            join equipo in _context.Equipos on reporte.EquipoId equals equipo.Id
            join usuario in _context.Usuarios on reporte.UsuarioReportaId equals usuario.UsuarioId
            join estado in _context.EstadosReporte on reporte.EstadoReporteId equals estado.Id
            where reporte.Id == id
            select new ReporteFallaDetalleDto
            {
                Id = reporte.Id,

                EquipoId = equipo.Id,
                CodigoEquipo = equipo.Codigo,
                NombreEquipo = equipo.Nombre,

                UsuarioReportaId = usuario.UsuarioId,
                UsuarioReporta = usuario.Nickname,

                EstadoReporteId = estado.Id,
                Estado = estado.Nombre,

                Titulo = reporte.Titulo,
                Descripcion = reporte.Descripcion,
                Prioridad = reporte.Prioridad,

                FechaReporte = reporte.FechaReporte,
                FechaCierre = reporte.FechaCierre,

                ObservacionesCierre = reporte.ObservacionesCierre
            };

        return await query.FirstOrDefaultAsync();
    }

    public async Task<ReporteFallaDetalleDto> CrearAsync(CrearReporteFallaDto dto)
    {
        await ValidarRelacionesAsync(dto.EquipoId, dto.UsuarioReportaId, dto.EstadoReporteId);

        var reporte = new ReporteFalla
        {
            EquipoId = dto.EquipoId,
            UsuarioReportaId = dto.UsuarioReportaId,
            EstadoReporteId = dto.EstadoReporteId,
            Titulo = dto.Titulo.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            Prioridad = dto.Prioridad.Trim(),
            FechaReporte = DateTime.UtcNow,
            FechaCierre = null,
            ObservacionesCierre = null
        };

        _context.ReportesFalla.Add(reporte);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = reporte.UsuarioReportaId,
            Accion = "Creación de reporte de falla",
            Modulo = "ReportesFalla",
            TablaAfectada = "ReportesFalla",
            RegistroId = reporte.Id,
            Descripcion =
                $"Se creó el reporte de falla '{reporte.Titulo}' para el equipo Id {reporte.EquipoId}. " +
                $"Prioridad: {reporte.Prioridad}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creado = await ObtenerPorIdAsync(reporte.Id);
        return creado!;
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarReporteFallaDto dto)
    {
        var reporte = await _context.ReportesFalla
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reporte == null)
            return false;

        reporte.Titulo = dto.Titulo.Trim();
        reporte.Descripcion = dto.Descripcion.Trim();
        reporte.Prioridad = dto.Prioridad.Trim();

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = reporte.UsuarioReportaId,
            Accion = "Actualización de reporte de falla",
            Modulo = "ReportesFalla",
            TablaAfectada = "ReportesFalla",
            RegistroId = reporte.Id,
            Descripcion =
                $"Se actualizó el reporte de falla '{reporte.Titulo}'. " +
                $"Prioridad actual: {reporte.Prioridad}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(int id, CambiarEstadoReporteFallaDto dto)
    {
        var reporte = await _context.ReportesFalla
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reporte == null)
            return false;

        var estado = await _context.EstadosReporte
            .FirstOrDefaultAsync(e => e.Id == dto.EstadoReporteId && e.Activo);

        if (estado == null)
            throw new InvalidOperationException("El estado de reporte no existe o no está activo.");

        var estadoAnteriorId = reporte.EstadoReporteId;

        reporte.EstadoReporteId = dto.EstadoReporteId;

        var nombreEstado = estado.Nombre.ToLower();

        if (nombreEstado == "cerrado" ||
            nombreEstado == "cancelado" ||
            nombreEstado == "resuelto")
        {
            reporte.FechaCierre = DateTime.UtcNow;
            reporte.ObservacionesCierre = dto.ObservacionesCierre;
        }
        else
        {
            reporte.FechaCierre = null;
            reporte.ObservacionesCierre = null;
        }

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = reporte.UsuarioReportaId,
            Accion = "Cambio de estado de reporte de falla",
            Modulo = "ReportesFalla",
            TablaAfectada = "ReportesFalla",
            RegistroId = reporte.Id,
            Descripcion =
                $"Se cambió el estado del reporte '{reporte.Titulo}'. " +
                $"Estado anterior Id: {estadoAnteriorId}. Estado nuevo: {estado.Nombre}. " +
                $"Observaciones de cierre: {reporte.ObservacionesCierre ?? "Sin observaciones"}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    private async Task ValidarRelacionesAsync(
        int equipoId,
        int usuarioReportaId,
        int estadoReporteId)
    {
        var equipoExiste = await _context.Equipos
            .AnyAsync(e => e.Id == equipoId && e.Activo);

        if (!equipoExiste)
            throw new InvalidOperationException("El equipo no existe o no está activo.");

        var usuarioExiste = await _context.Usuarios
            .AnyAsync(u => u.UsuarioId == usuarioReportaId && u.Activo);

        if (!usuarioExiste)
            throw new InvalidOperationException("El usuario que reporta no existe o no está activo.");

        var estadoExiste = await _context.EstadosReporte
            .AnyAsync(e => e.Id == estadoReporteId && e.Activo);

        if (!estadoExiste)
            throw new InvalidOperationException("El estado de reporte no existe o no está activo.");
    }
}