using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Reclasificaciones;
using ControlEquiposElectronicos.Api.Entities.Auditoria;
using ControlEquiposElectronicos.Api.Entities.Reportes;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class ReclasificacionEquipoService : IReclasificacionEquipoService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public ReclasificacionEquipoService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<List<ReclasificacionEquipoListadoDto>> ObtenerTodasAsync()
    {
        var query =
            from reclasificacion in _context.ReclasificacionesEquipo
            join equipo in _context.Equipos on reclasificacion.EquipoId equals equipo.Id
            join usuario in _context.Usuarios on reclasificacion.UsuarioId equals usuario.UsuarioId

            join estadoAnteriorTemp in _context.EstadosEquipo
                on reclasificacion.EstadoAnteriorId equals (int?)estadoAnteriorTemp.Id into estadoAnteriorJoin
            from estadoAnterior in estadoAnteriorJoin.DefaultIfEmpty()

            join estadoNuevoTemp in _context.EstadosEquipo
                on reclasificacion.EstadoNuevoId equals (int?)estadoNuevoTemp.Id into estadoNuevoJoin
            from estadoNuevo in estadoNuevoJoin.DefaultIfEmpty()

            join ubicacionAnteriorTemp in _context.Ubicaciones
                on reclasificacion.UbicacionAnteriorId equals (int?)ubicacionAnteriorTemp.Id into ubicacionAnteriorJoin
            from ubicacionAnterior in ubicacionAnteriorJoin.DefaultIfEmpty()

            join ubicacionNuevaTemp in _context.Ubicaciones
                on reclasificacion.UbicacionNuevaId equals (int?)ubicacionNuevaTemp.Id into ubicacionNuevaJoin
            from ubicacionNueva in ubicacionNuevaJoin.DefaultIfEmpty()

            orderby reclasificacion.FechaReclasificacion descending
            select new ReclasificacionEquipoListadoDto
            {
                Id = reclasificacion.ReclasificacionEquipoId,

                EquipoId = equipo.Id,
                CodigoActualEquipo = equipo.Codigo,
                NombreEquipo = equipo.Nombre,

                UsuarioId = usuario.UsuarioId,
                Usuario = usuario.Nickname,

                CodigoAnterior = reclasificacion.CodigoAnterior,
                CodigoNuevo = reclasificacion.CodigoNuevo,

                EstadoAnteriorId = reclasificacion.EstadoAnteriorId,
                EstadoAnterior = estadoAnterior != null ? estadoAnterior.Nombre : null,

                EstadoNuevoId = reclasificacion.EstadoNuevoId,
                EstadoNuevo = estadoNuevo != null ? estadoNuevo.Nombre : null,

                UbicacionAnteriorId = reclasificacion.UbicacionAnteriorId,
                UbicacionAnterior = ubicacionAnterior != null ? ubicacionAnterior.Nombre : null,

                UbicacionNuevaId = reclasificacion.UbicacionNuevaId,
                UbicacionNueva = ubicacionNueva != null ? ubicacionNueva.Nombre : null,

                Motivo = reclasificacion.Motivo,
                TipoReclasificacion = reclasificacion.TipoReclasificacion,

                FechaReclasificacion = reclasificacion.FechaReclasificacion
            };

        return await query.ToListAsync();
    }

    public async Task<ReclasificacionEquipoDetalleDto?> ObtenerPorIdAsync(int id)
    {
        var query =
            from reclasificacion in _context.ReclasificacionesEquipo
            join equipo in _context.Equipos on reclasificacion.EquipoId equals equipo.Id
            join usuario in _context.Usuarios on reclasificacion.UsuarioId equals usuario.UsuarioId

            join estadoAnteriorTemp in _context.EstadosEquipo
                on reclasificacion.EstadoAnteriorId equals (int?)estadoAnteriorTemp.Id into estadoAnteriorJoin
            from estadoAnterior in estadoAnteriorJoin.DefaultIfEmpty()

            join estadoNuevoTemp in _context.EstadosEquipo
                on reclasificacion.EstadoNuevoId equals (int?)estadoNuevoTemp.Id into estadoNuevoJoin
            from estadoNuevo in estadoNuevoJoin.DefaultIfEmpty()

            join ubicacionAnteriorTemp in _context.Ubicaciones
                on reclasificacion.UbicacionAnteriorId equals (int?)ubicacionAnteriorTemp.Id into ubicacionAnteriorJoin
            from ubicacionAnterior in ubicacionAnteriorJoin.DefaultIfEmpty()

            join ubicacionNuevaTemp in _context.Ubicaciones
                on reclasificacion.UbicacionNuevaId equals (int?)ubicacionNuevaTemp.Id into ubicacionNuevaJoin
            from ubicacionNueva in ubicacionNuevaJoin.DefaultIfEmpty()

            where reclasificacion.ReclasificacionEquipoId == id
            select new ReclasificacionEquipoDetalleDto
            {
                Id = reclasificacion.ReclasificacionEquipoId,

                EquipoId = equipo.Id,
                CodigoActualEquipo = equipo.Codigo,
                NombreEquipo = equipo.Nombre,

                UsuarioId = usuario.UsuarioId,
                Usuario = usuario.Nickname,

                CodigoAnterior = reclasificacion.CodigoAnterior,
                CodigoNuevo = reclasificacion.CodigoNuevo,

                EstadoAnteriorId = reclasificacion.EstadoAnteriorId,
                EstadoAnterior = estadoAnterior != null ? estadoAnterior.Nombre : null,

                EstadoNuevoId = reclasificacion.EstadoNuevoId,
                EstadoNuevo = estadoNuevo != null ? estadoNuevo.Nombre : null,

                UbicacionAnteriorId = reclasificacion.UbicacionAnteriorId,
                UbicacionAnterior = ubicacionAnterior != null ? ubicacionAnterior.Nombre : null,

                UbicacionNuevaId = reclasificacion.UbicacionNuevaId,
                UbicacionNueva = ubicacionNueva != null ? ubicacionNueva.Nombre : null,

                Motivo = reclasificacion.Motivo,
                TipoReclasificacion = reclasificacion.TipoReclasificacion,

                FechaReclasificacion = reclasificacion.FechaReclasificacion
            };

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<ReclasificacionEquipoListadoDto>> ObtenerPorEquipoAsync(int equipoId)
    {
        var query =
            from reclasificacion in _context.ReclasificacionesEquipo
            join equipo in _context.Equipos on reclasificacion.EquipoId equals equipo.Id
            join usuario in _context.Usuarios on reclasificacion.UsuarioId equals usuario.UsuarioId

            join estadoAnteriorTemp in _context.EstadosEquipo
                on reclasificacion.EstadoAnteriorId equals (int?)estadoAnteriorTemp.Id into estadoAnteriorJoin
            from estadoAnterior in estadoAnteriorJoin.DefaultIfEmpty()

            join estadoNuevoTemp in _context.EstadosEquipo
                on reclasificacion.EstadoNuevoId equals (int?)estadoNuevoTemp.Id into estadoNuevoJoin
            from estadoNuevo in estadoNuevoJoin.DefaultIfEmpty()

            join ubicacionAnteriorTemp in _context.Ubicaciones
                on reclasificacion.UbicacionAnteriorId equals (int?)ubicacionAnteriorTemp.Id into ubicacionAnteriorJoin
            from ubicacionAnterior in ubicacionAnteriorJoin.DefaultIfEmpty()

            join ubicacionNuevaTemp in _context.Ubicaciones
                on reclasificacion.UbicacionNuevaId equals (int?)ubicacionNuevaTemp.Id into ubicacionNuevaJoin
            from ubicacionNueva in ubicacionNuevaJoin.DefaultIfEmpty()

            where reclasificacion.EquipoId == equipoId
            orderby reclasificacion.FechaReclasificacion descending
            select new ReclasificacionEquipoListadoDto
            {
                Id = reclasificacion.ReclasificacionEquipoId,

                EquipoId = equipo.Id,
                CodigoActualEquipo = equipo.Codigo,
                NombreEquipo = equipo.Nombre,

                UsuarioId = usuario.UsuarioId,
                Usuario = usuario.Nickname,

                CodigoAnterior = reclasificacion.CodigoAnterior,
                CodigoNuevo = reclasificacion.CodigoNuevo,

                EstadoAnteriorId = reclasificacion.EstadoAnteriorId,
                EstadoAnterior = estadoAnterior != null ? estadoAnterior.Nombre : null,

                EstadoNuevoId = reclasificacion.EstadoNuevoId,
                EstadoNuevo = estadoNuevo != null ? estadoNuevo.Nombre : null,

                UbicacionAnteriorId = reclasificacion.UbicacionAnteriorId,
                UbicacionAnterior = ubicacionAnterior != null ? ubicacionAnterior.Nombre : null,

                UbicacionNuevaId = reclasificacion.UbicacionNuevaId,
                UbicacionNueva = ubicacionNueva != null ? ubicacionNueva.Nombre : null,

                Motivo = reclasificacion.Motivo,
                TipoReclasificacion = reclasificacion.TipoReclasificacion,

                FechaReclasificacion = reclasificacion.FechaReclasificacion
            };

        return await query.ToListAsync();
    }

    public async Task<ReclasificacionEquipoDetalleDto> CrearAsync(CrearReclasificacionEquipoDto dto)
    {
        var equipo = await _context.Equipos
            .FirstOrDefaultAsync(e => e.Id == dto.EquipoId && e.Activo);

        if (equipo == null)
            throw new InvalidOperationException("El equipo no existe o no está activo.");

        var usuarioExiste = await _context.Usuarios
            .AnyAsync(u => u.UsuarioId == dto.UsuarioId && u.Activo);

        if (!usuarioExiste)
            throw new InvalidOperationException("El usuario no existe o no está activo.");

        if (dto.EstadoNuevoId.HasValue)
        {
            var estadoExiste = await _context.EstadosEquipo
                .AnyAsync(e => e.Id == dto.EstadoNuevoId.Value && e.Activo);

            if (!estadoExiste)
                throw new InvalidOperationException("El nuevo estado no existe o no está activo.");
        }

        if (dto.UbicacionNuevaId.HasValue)
        {
            var ubicacionExiste = await _context.Ubicaciones
                .AnyAsync(u => u.Id == dto.UbicacionNuevaId.Value && u.Activo);

            if (!ubicacionExiste)
                throw new InvalidOperationException("La nueva ubicación no existe o no está activa.");
        }

        var codigoAnterior = equipo.Codigo;
        var estadoAnteriorId = equipo.EstadoEquipoId;
        int? ubicacionAnteriorId = equipo.UbicacionId;

        var codigoNuevo = string.IsNullOrWhiteSpace(dto.CodigoNuevo)
            ? equipo.Codigo
            : dto.CodigoNuevo.Trim();

        if (!string.Equals(codigoAnterior, codigoNuevo, StringComparison.OrdinalIgnoreCase))
        {
            var codigoDuplicado = await _context.Equipos
                .AnyAsync(e => e.Codigo == codigoNuevo && e.Id != equipo.Id);

            if (codigoDuplicado)
                throw new InvalidOperationException("Ya existe otro equipo con el código nuevo indicado.");
        }

        var estadoNuevoId = dto.EstadoNuevoId ?? equipo.EstadoEquipoId;
        int? ubicacionNuevaId = dto.UbicacionNuevaId ?? equipo.UbicacionId;

        var huboCambioCodigo = !string.Equals(equipo.Codigo, codigoNuevo, StringComparison.OrdinalIgnoreCase);
        var huboCambioEstado = equipo.EstadoEquipoId != estadoNuevoId;
        var huboCambioUbicacion = equipo.UbicacionId != ubicacionNuevaId;

        if (!huboCambioCodigo && !huboCambioEstado && !huboCambioUbicacion)
            throw new InvalidOperationException("No se detectó ningún cambio para reclasificar el equipo.");

        var fechaOperacion = DateTime.UtcNow;

        var reclasificacion = new ReclasificacionEquipo
        {
            EquipoId = equipo.Id,
            UsuarioId = dto.UsuarioId,

            CodigoAnterior = codigoAnterior,
            CodigoNuevo = codigoNuevo,

            EstadoAnteriorId = estadoAnteriorId,
            EstadoNuevoId = estadoNuevoId,

            Motivo = dto.Motivo.Trim(),
            FechaReclasificacion = fechaOperacion,
            TipoReclasificacion = dto.TipoReclasificacion.Trim(),

            UbicacionAnteriorId = ubicacionAnteriorId,
            UbicacionNuevaId = ubicacionNuevaId
        };

        _context.ReclasificacionesEquipo.Add(reclasificacion);

        if (huboCambioEstado)
        {
            var historial = new HistorialEstadoEquipo
            {
                EquipoId = equipo.Id,
                EstadoAnteriorId = estadoAnteriorId,
                EstadoNuevoId = estadoNuevoId,
                UsuarioId = dto.UsuarioId,
                Motivo = dto.Motivo.Trim(),
                FechaCambio = fechaOperacion
            };

            _context.HistorialEstadosEquipo.Add(historial);
        }

        equipo.Codigo = codigoNuevo;
        equipo.EstadoEquipoId = estadoNuevoId;
        equipo.UbicacionId = ubicacionNuevaId;
        equipo.FechaActualizacion = fechaOperacion;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = dto.UsuarioId,
            Accion = "Reclasificación de equipo",
            Modulo = "Reclasificaciones",
            TablaAfectada = "ReclasificacionesEquipo",
            RegistroId = reclasificacion.ReclasificacionEquipoId,
            Descripcion =
                $"Se reclasificó el equipo '{codigoAnterior}' a '{codigoNuevo}'. " +
                $"Estado anterior: {estadoAnteriorId}. Estado nuevo: {estadoNuevoId}. " +
                $"Motivo: {dto.Motivo.Trim()}",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creada = await ObtenerPorIdAsync(reclasificacion.ReclasificacionEquipoId);

        return creada!;
    }

    public async Task<List<HistorialEstadoEquipoDto>> ObtenerHistorialEstadosPorEquipoAsync(int equipoId)
    {
        var query =
            from historial in _context.HistorialEstadosEquipo
            join equipo in _context.Equipos on historial.EquipoId equals equipo.Id
            join estadoAnterior in _context.EstadosEquipo on historial.EstadoAnteriorId equals estadoAnterior.Id
            join estadoNuevo in _context.EstadosEquipo on historial.EstadoNuevoId equals estadoNuevo.Id
            join usuario in _context.Usuarios on historial.UsuarioId equals usuario.UsuarioId
            where historial.EquipoId == equipoId
            orderby historial.FechaCambio descending
            select new HistorialEstadoEquipoDto
            {
                Id = historial.Id,

                EquipoId = equipo.Id,
                CodigoEquipo = equipo.Codigo,
                NombreEquipo = equipo.Nombre,

                EstadoAnteriorId = estadoAnterior.Id,
                EstadoAnterior = estadoAnterior.Nombre,

                EstadoNuevoId = estadoNuevo.Id,
                EstadoNuevo = estadoNuevo.Nombre,

                UsuarioId = usuario.UsuarioId,
                Usuario = usuario.Nickname,

                Motivo = historial.Motivo ?? string.Empty,

                FechaCambio = historial.FechaCambio
            };

        return await query.ToListAsync();
    }
}