using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.Entities.Auditoria;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly AppDbContext _context;

    public AuditoriaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<HistorialOperacionDto>> ObtenerTodosAsync()
    {
        var query =
            from historial in _context.HistorialOperaciones
            join usuarioTemp in _context.Usuarios
                on historial.UsuarioId equals (int?)usuarioTemp.UsuarioId into usuarioJoin
            from usuario in usuarioJoin.DefaultIfEmpty()
            orderby historial.FechaOperacion descending
            select new HistorialOperacionDto
            {
                Id = historial.Id,

                UsuarioId = historial.UsuarioId,
                Usuario = usuario != null ? usuario.Nickname : null,

                Accion = historial.Accion,
                Modulo = historial.Modulo,

                TablaAfectada = historial.TablaAfectada,
                RegistroId = historial.RegistroId,

                Descripcion = historial.Descripcion,
                DireccionIP = historial.DireccionIP,

                FechaOperacion = historial.FechaOperacion
            };

        return await query.ToListAsync();
    }

    public async Task<HistorialOperacionDto?> ObtenerPorIdAsync(int id)
    {
        var query =
            from historial in _context.HistorialOperaciones
            join usuarioTemp in _context.Usuarios
                on historial.UsuarioId equals (int?)usuarioTemp.UsuarioId into usuarioJoin
            from usuario in usuarioJoin.DefaultIfEmpty()
            where historial.Id == id
            select new HistorialOperacionDto
            {
                Id = historial.Id,

                UsuarioId = historial.UsuarioId,
                Usuario = usuario != null ? usuario.Nickname : null,

                Accion = historial.Accion,
                Modulo = historial.Modulo,

                TablaAfectada = historial.TablaAfectada,
                RegistroId = historial.RegistroId,

                Descripcion = historial.Descripcion,
                DireccionIP = historial.DireccionIP,

                FechaOperacion = historial.FechaOperacion
            };

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<HistorialOperacionDto>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        var query =
            from historial in _context.HistorialOperaciones
            join usuarioTemp in _context.Usuarios
                on historial.UsuarioId equals (int?)usuarioTemp.UsuarioId into usuarioJoin
            from usuario in usuarioJoin.DefaultIfEmpty()
            where historial.UsuarioId == usuarioId
            orderby historial.FechaOperacion descending
            select new HistorialOperacionDto
            {
                Id = historial.Id,

                UsuarioId = historial.UsuarioId,
                Usuario = usuario != null ? usuario.Nickname : null,

                Accion = historial.Accion,
                Modulo = historial.Modulo,

                TablaAfectada = historial.TablaAfectada,
                RegistroId = historial.RegistroId,

                Descripcion = historial.Descripcion,
                DireccionIP = historial.DireccionIP,

                FechaOperacion = historial.FechaOperacion
            };

        return await query.ToListAsync();
    }

    public async Task<List<HistorialOperacionDto>> ObtenerPorModuloAsync(string modulo)
    {
        var moduloNormalizado = modulo.Trim().ToLower();

        var query =
            from historial in _context.HistorialOperaciones
            join usuarioTemp in _context.Usuarios
                on historial.UsuarioId equals (int?)usuarioTemp.UsuarioId into usuarioJoin
            from usuario in usuarioJoin.DefaultIfEmpty()
            where historial.Modulo.ToLower() == moduloNormalizado
            orderby historial.FechaOperacion descending
            select new HistorialOperacionDto
            {
                Id = historial.Id,

                UsuarioId = historial.UsuarioId,
                Usuario = usuario != null ? usuario.Nickname : null,

                Accion = historial.Accion,
                Modulo = historial.Modulo,

                TablaAfectada = historial.TablaAfectada,
                RegistroId = historial.RegistroId,

                Descripcion = historial.Descripcion,
                DireccionIP = historial.DireccionIP,

                FechaOperacion = historial.FechaOperacion
            };

        return await query.ToListAsync();
    }

    public async Task<HistorialOperacionDto> RegistrarAsync(CrearHistorialOperacionDto dto)
    {
        if (dto.UsuarioId.HasValue)
        {
            var usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.UsuarioId == dto.UsuarioId.Value && u.Activo);

            if (!usuarioExiste)
                throw new InvalidOperationException("El usuario indicado no existe o no está activo.");
        }

        var historial = new HistorialOperacion
        {
            UsuarioId = dto.UsuarioId,
            Accion = dto.Accion.Trim(),
            Modulo = dto.Modulo.Trim(),
            TablaAfectada = dto.TablaAfectada,
            RegistroId = dto.RegistroId,
            Descripcion = dto.Descripcion,
            DireccionIP = dto.DireccionIP,
            FechaOperacion = DateTime.UtcNow
        };

        _context.HistorialOperaciones.Add(historial);
        await _context.SaveChangesAsync();

        var creado = await ObtenerPorIdAsync(historial.Id);
        return creado!;
    }
}