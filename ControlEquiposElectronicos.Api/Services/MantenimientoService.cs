using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Mantenimientos;
using ControlEquiposElectronicos.Api.Entities.Mantenimientos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class MantenimientoService : IMantenimientoService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public MantenimientoService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<List<MantenimientoListadoDto>> ObtenerTodosAsync()
    {
        var query =
            from mantenimiento in _context.Mantenimientos
            join equipo in _context.Equipos on mantenimiento.EquipoId equals equipo.Id
            join tipo in _context.TiposMantenimiento on mantenimiento.TipoMantenimientoId equals tipo.Id
            join tecnico in _context.Usuarios on mantenimiento.TecnicoId equals tecnico.UsuarioId
            orderby mantenimiento.FechaInicio descending
            select new MantenimientoListadoDto
            {
                Id = mantenimiento.Id,

                EquipoId = equipo.Id,
                CodigoEquipo = equipo.Codigo,
                NombreEquipo = equipo.Nombre,

                ReporteFallaId = mantenimiento.ReporteFallaId,

                TipoMantenimientoId = tipo.Id,
                TipoMantenimiento = tipo.Nombre,

                TecnicoId = tecnico.UsuarioId,
                Tecnico = tecnico.Nickname,

                Descripcion = mantenimiento.Descripcion,
                Diagnostico = mantenimiento.Diagnostico,

                FechaInicio = mantenimiento.FechaInicio,
                FechaFin = mantenimiento.FechaFin,

                CostoEstimado = mantenimiento.CostoEstimado,

                EstadoMantenimiento = mantenimiento.EstadoMantenimiento,

                Activo = mantenimiento.Activo
            };

        return await query.ToListAsync();
    }

    public async Task<List<MantenimientoListadoDto>> ObtenerPorEquipoAsync(int equipoId)
    {
        var query =
            from mantenimiento in _context.Mantenimientos
            join equipo in _context.Equipos on mantenimiento.EquipoId equals equipo.Id
            join tipo in _context.TiposMantenimiento on mantenimiento.TipoMantenimientoId equals tipo.Id
            join tecnico in _context.Usuarios on mantenimiento.TecnicoId equals tecnico.UsuarioId
            where mantenimiento.EquipoId == equipoId
            orderby mantenimiento.FechaInicio descending
            select new MantenimientoListadoDto
            {
                Id = mantenimiento.Id,

                EquipoId = equipo.Id,
                CodigoEquipo = equipo.Codigo,
                NombreEquipo = equipo.Nombre,

                ReporteFallaId = mantenimiento.ReporteFallaId,

                TipoMantenimientoId = tipo.Id,
                TipoMantenimiento = tipo.Nombre,

                TecnicoId = tecnico.UsuarioId,
                Tecnico = tecnico.Nickname,

                Descripcion = mantenimiento.Descripcion,
                Diagnostico = mantenimiento.Diagnostico,

                FechaInicio = mantenimiento.FechaInicio,
                FechaFin = mantenimiento.FechaFin,

                CostoEstimado = mantenimiento.CostoEstimado,

                EstadoMantenimiento = mantenimiento.EstadoMantenimiento,

                Activo = mantenimiento.Activo
            };

        return await query.ToListAsync();
    }

    public async Task<MantenimientoDetalleDto?> ObtenerPorIdAsync(int id)
    {
        var mantenimientoDto = await (
            from mantenimiento in _context.Mantenimientos
            join equipo in _context.Equipos on mantenimiento.EquipoId equals equipo.Id
            join tipo in _context.TiposMantenimiento on mantenimiento.TipoMantenimientoId equals tipo.Id
            join tecnico in _context.Usuarios on mantenimiento.TecnicoId equals tecnico.UsuarioId
            where mantenimiento.Id == id
            select new MantenimientoDetalleDto
            {
                Id = mantenimiento.Id,

                EquipoId = equipo.Id,
                CodigoEquipo = equipo.Codigo,
                NombreEquipo = equipo.Nombre,

                ReporteFallaId = mantenimiento.ReporteFallaId,

                TipoMantenimientoId = tipo.Id,
                TipoMantenimiento = tipo.Nombre,

                TecnicoId = tecnico.UsuarioId,
                Tecnico = tecnico.Nickname,

                Descripcion = mantenimiento.Descripcion,
                Diagnostico = mantenimiento.Diagnostico,
                Resultado = mantenimiento.Resultado,

                FechaInicio = mantenimiento.FechaInicio,
                FechaFin = mantenimiento.FechaFin,

                CostoEstimado = mantenimiento.CostoEstimado,

                EstadoMantenimiento = mantenimiento.EstadoMantenimiento,

                Activo = mantenimiento.Activo
            })
            .FirstOrDefaultAsync();

        if (mantenimientoDto == null)
            return null;

        mantenimientoDto.Repuestos = await _context.MantenimientoRepuestos
            .Where(r => r.MantenimientoId == id)
            .OrderByDescending(r => r.FechaRegistro)
            .Select(r => new MantenimientoRepuestoDto
            {
                Id = r.Id,
                MantenimientoId = r.MantenimientoId,
                NombreRepuesto = r.NombreRepuesto,
                Descripcion = r.Descripcion,
                Cantidad = r.Cantidad,
                CostoUnitario = r.CostoUnitario,
                NumeroSerieAnterior = r.NumeroSerieAnterior,
                NumeroSerieNuevo = r.NumeroSerieNuevo,
                FechaRegistro = r.FechaRegistro
            })
            .ToListAsync();

        return mantenimientoDto;
    }

    public async Task<MantenimientoDetalleDto> CrearAsync(CrearMantenimientoDto dto)
    {
        await ValidarRelacionesAsync(
            dto.EquipoId,
            dto.ReporteFallaId,
            dto.TipoMantenimientoId,
            dto.TecnicoId);

        if (dto.CostoEstimado.HasValue && dto.CostoEstimado.Value < 0)
            throw new InvalidOperationException("El costo estimado no puede ser negativo.");

        var mantenimiento = new Mantenimiento
        {
            EquipoId = dto.EquipoId,
            ReporteFallaId = dto.ReporteFallaId,
            TipoMantenimientoId = dto.TipoMantenimientoId,
            TecnicoId = dto.TecnicoId,
            Descripcion = dto.Descripcion.Trim(),
            Diagnostico = dto.Diagnostico,
            Resultado = null,
            FechaInicio = DateTime.UtcNow,
            FechaFin = null,
            CostoEstimado = dto.CostoEstimado,
            EstadoMantenimiento = dto.EstadoMantenimiento.Trim(),
            Activo = true
        };

        _context.Mantenimientos.Add(mantenimiento);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = mantenimiento.TecnicoId,
            Accion = "Creación de mantenimiento",
            Modulo = "Mantenimientos",
            TablaAfectada = "Mantenimientos",
            RegistroId = mantenimiento.Id,
            Descripcion =
                $"Se creó un mantenimiento para el equipo Id {mantenimiento.EquipoId}. " +
                $"TipoMantenimientoId: {mantenimiento.TipoMantenimientoId}. " +
                $"Estado: {mantenimiento.EstadoMantenimiento}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creado = await ObtenerPorIdAsync(mantenimiento.Id);
        return creado!;
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarMantenimientoDto dto)
    {
        var mantenimiento = await _context.Mantenimientos
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mantenimiento == null)
            return false;

        await ValidarRelacionesAsync(
            mantenimiento.EquipoId,
            mantenimiento.ReporteFallaId,
            dto.TipoMantenimientoId,
            dto.TecnicoId);

        if (dto.CostoEstimado.HasValue && dto.CostoEstimado.Value < 0)
            throw new InvalidOperationException("El costo estimado no puede ser negativo.");

        mantenimiento.TipoMantenimientoId = dto.TipoMantenimientoId;
        mantenimiento.TecnicoId = dto.TecnicoId;
        mantenimiento.Descripcion = dto.Descripcion.Trim();
        mantenimiento.Diagnostico = dto.Diagnostico;
        mantenimiento.CostoEstimado = dto.CostoEstimado;
        mantenimiento.EstadoMantenimiento = dto.EstadoMantenimiento.Trim();
        mantenimiento.Activo = dto.Activo;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = mantenimiento.TecnicoId,
            Accion = "Actualización de mantenimiento",
            Modulo = "Mantenimientos",
            TablaAfectada = "Mantenimientos",
            RegistroId = mantenimiento.Id,
            Descripcion =
                $"Se actualizó el mantenimiento Id {mantenimiento.Id} del equipo Id {mantenimiento.EquipoId}. " +
                $"Estado actual: {mantenimiento.EstadoMantenimiento}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> FinalizarAsync(int id, FinalizarMantenimientoDto dto)
    {
        var mantenimiento = await _context.Mantenimientos
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mantenimiento == null)
            return false;

        mantenimiento.Resultado = dto.Resultado;
        mantenimiento.EstadoMantenimiento = dto.EstadoMantenimiento.Trim();
        mantenimiento.FechaFin = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = mantenimiento.TecnicoId,
            Accion = "Finalización de mantenimiento",
            Modulo = "Mantenimientos",
            TablaAfectada = "Mantenimientos",
            RegistroId = mantenimiento.Id,
            Descripcion =
                $"Se finalizó el mantenimiento Id {mantenimiento.Id} del equipo Id {mantenimiento.EquipoId}. " +
                $"Resultado: {mantenimiento.Resultado ?? "Sin resultado"}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<MantenimientoRepuestoDto> AgregarRepuestoAsync(int mantenimientoId, CrearMantenimientoRepuestoDto dto)
    {
        var mantenimiento = await _context.Mantenimientos
            .FirstOrDefaultAsync(m => m.Id == mantenimientoId && m.Activo);

        if (mantenimiento == null)
            throw new InvalidOperationException("El mantenimiento no existe o no está activo.");

        if (dto.Cantidad <= 0)
            throw new InvalidOperationException("La cantidad debe ser mayor que cero.");

        if (dto.CostoUnitario.HasValue && dto.CostoUnitario.Value < 0)
            throw new InvalidOperationException("El costo unitario no puede ser negativo.");

        var repuesto = new MantenimientoRepuesto
        {
            MantenimientoId = mantenimientoId,
            NombreRepuesto = dto.NombreRepuesto.Trim(),
            Descripcion = dto.Descripcion,
            Cantidad = dto.Cantidad,
            CostoUnitario = dto.CostoUnitario,
            NumeroSerieAnterior = dto.NumeroSerieAnterior,
            NumeroSerieNuevo = dto.NumeroSerieNuevo,
            FechaRegistro = DateTime.UtcNow
        };

        _context.MantenimientoRepuestos.Add(repuesto);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = mantenimiento.TecnicoId,
            Accion = "Registro de repuesto en mantenimiento",
            Modulo = "Mantenimientos",
            TablaAfectada = "MantenimientoRepuestos",
            RegistroId = repuesto.Id,
            Descripcion =
                $"Se registró el repuesto '{repuesto.NombreRepuesto}' en el mantenimiento Id {mantenimiento.Id}. " +
                $"Cantidad: {repuesto.Cantidad}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return new MantenimientoRepuestoDto
        {
            Id = repuesto.Id,
            MantenimientoId = repuesto.MantenimientoId,
            NombreRepuesto = repuesto.NombreRepuesto,
            Descripcion = repuesto.Descripcion,
            Cantidad = repuesto.Cantidad,
            CostoUnitario = repuesto.CostoUnitario,
            NumeroSerieAnterior = repuesto.NumeroSerieAnterior,
            NumeroSerieNuevo = repuesto.NumeroSerieNuevo,
            FechaRegistro = repuesto.FechaRegistro
        };
    }

    public async Task<bool> EliminarRepuestoAsync(int repuestoId)
    {
        var repuesto = await _context.MantenimientoRepuestos
            .FirstOrDefaultAsync(r => r.Id == repuestoId);

        if (repuesto == null)
            return false;

        var mantenimiento = await _context.Mantenimientos
            .FirstOrDefaultAsync(m => m.Id == repuesto.MantenimientoId);

        var nombreRepuesto = repuesto.NombreRepuesto;
        var mantenimientoId = repuesto.MantenimientoId;

        _context.MantenimientoRepuestos.Remove(repuesto);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = mantenimiento?.TecnicoId ?? _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Eliminación de repuesto de mantenimiento",
            Modulo = "Mantenimientos",
            TablaAfectada = "MantenimientoRepuestos",
            RegistroId = repuestoId,
            Descripcion =
                $"Se eliminó el repuesto '{nombreRepuesto}' del mantenimiento Id {mantenimientoId}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    private async Task ValidarRelacionesAsync(
        int equipoId,
        int? reporteFallaId,
        int tipoMantenimientoId,
        int tecnicoId)
    {
        var equipoExiste = await _context.Equipos
            .AnyAsync(e => e.Id == equipoId && e.Activo);

        if (!equipoExiste)
            throw new InvalidOperationException("El equipo no existe o no está activo.");

        if (reporteFallaId.HasValue)
        {
            var reporteExiste = await _context.ReportesFalla
                .AnyAsync(r => r.Id == reporteFallaId.Value);

            if (!reporteExiste)
                throw new InvalidOperationException("El reporte de falla no existe.");
        }

        var tipoExiste = await _context.TiposMantenimiento
            .AnyAsync(t => t.Id == tipoMantenimientoId && t.Activo);

        if (!tipoExiste)
            throw new InvalidOperationException("El tipo de mantenimiento no existe o no está activo.");

        var tecnicoExiste = await _context.Usuarios
            .AnyAsync(u => u.UsuarioId == tecnicoId && u.Activo);

        if (!tecnicoExiste)
            throw new InvalidOperationException("El técnico no existe o no está activo.");
    }
}