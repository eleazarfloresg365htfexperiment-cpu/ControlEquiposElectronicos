using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Catalogos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class CatalogoService : ICatalogoService
{
    private readonly AppDbContext _context;

    public CatalogoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaEquipoDto>> ObtenerCategoriasEquipoAsync()
    {
        return await _context.CategoriasEquipo
            .OrderBy(c => c.Nombre)
            .Select(c => new CategoriaEquipoDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            })
            .ToListAsync();
    }

    public async Task<List<TipoEquipoDto>> ObtenerTiposEquipoAsync()
    {
        return await _context.TiposEquipo
            .Include(t => t.CategoriaEquipo)
            .Where(t => t.Activo)
            .OrderBy(t => t.CategoriaEquipo.Nombre)
            .ThenBy(t => t.Nombre)
            .Select(t => new TipoEquipoDto
            {
                Id = t.Id,
                CategoriaEquipoId = t.CategoriaEquipoId,
                Categoria = t.CategoriaEquipo.Nombre,
                Nombre = t.Nombre,
                Descripcion = t.Descripcion,
                Activo = t.Activo
            })
            .ToListAsync();
    }

    public async Task<List<EstadoEquipoDto>> ObtenerEstadosEquipoAsync()
    {
        return await _context.EstadosEquipo
            .Where(e => e.Activo)
            .OrderBy(e => e.Nombre)
            .Select(e => new EstadoEquipoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Activo = e.Activo
            })
            .ToListAsync();
    }

    public async Task<List<UbicacionDto>> ObtenerUbicacionesAsync()
    {
        return await _context.Ubicaciones
            .Where(u => u.Activo)
            .OrderBy(u => u.TipoUbicacion)
            .ThenBy(u => u.Nombre)
            .Select(u => new UbicacionDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                TipoUbicacion = u.TipoUbicacion,
                Descripcion = u.Descripcion,
                Activo = u.Activo
            })
            .ToListAsync();
    }

    public async Task<List<EstadoReporteDto>> ObtenerEstadosReporteAsync()
    {
        return await _context.EstadosReporte
            .Where(e => e.Activo)
            .OrderBy(e => e.Nombre)
            .Select(e => new EstadoReporteDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Activo = e.Activo
            })
            .ToListAsync();
    }

    public async Task<List<TipoMantenimientoDto>> ObtenerTiposMantenimientoAsync()
    {
        return await _context.TiposMantenimiento
            .Where(t => t.Activo)
            .OrderBy(t => t.Nombre)
            .Select(t => new TipoMantenimientoDto
            {
                Id = t.Id,
                Nombre = t.Nombre,
                Descripcion = t.Descripcion,
                Activo = t.Activo
            })
            .ToListAsync();
    }
}