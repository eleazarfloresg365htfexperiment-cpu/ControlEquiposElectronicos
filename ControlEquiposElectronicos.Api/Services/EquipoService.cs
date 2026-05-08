using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class EquipoService : IEquipoService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public EquipoService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<List<EquipoListadoDto>> ObtenerTodosAsync()
    {
        return await _context.Equipos
            .Include(e => e.CategoriaEquipo)
            .Include(e => e.TipoEquipo)
            .Include(e => e.EstadoEquipo)
            .Include(e => e.Ubicacion)
            .OrderBy(e => e.Codigo)
            .Select(e => new EquipoListadoDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nombre = e.Nombre,
                Categoria = e.CategoriaEquipo.Nombre,
                Tipo = e.TipoEquipo.Nombre,
                Estado = e.EstadoEquipo.Nombre,
                Ubicacion = e.Ubicacion != null ? e.Ubicacion.Nombre : null,
                Marca = e.Marca,
                Modelo = e.Modelo,
                NumeroSerie = e.NumeroSerie,
                Activo = e.Activo
            })
            .ToListAsync();
    }

    public async Task<EquipoDetalleDto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Equipos
            .Include(e => e.CategoriaEquipo)
            .Include(e => e.TipoEquipo)
            .Include(e => e.EstadoEquipo)
            .Include(e => e.Ubicacion)
            .Where(e => e.Id == id)
            .Select(e => new EquipoDetalleDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nombre = e.Nombre,

                CategoriaEquipoId = e.CategoriaEquipoId,
                Categoria = e.CategoriaEquipo.Nombre,

                TipoEquipoId = e.TipoEquipoId,
                Tipo = e.TipoEquipo.Nombre,

                EstadoEquipoId = e.EstadoEquipoId,
                Estado = e.EstadoEquipo.Nombre,

                UbicacionId = e.UbicacionId,
                Ubicacion = e.Ubicacion != null ? e.Ubicacion.Nombre : null,

                Marca = e.Marca,
                Modelo = e.Modelo,
                NumeroSerie = e.NumeroSerie,

                FechaCompra = e.FechaCompra,
                FechaInstalacion = e.FechaInstalacion,

                Observaciones = e.Observaciones,

                Activo = e.Activo,
                FechaCreacion = e.FechaCreacion,
                FechaActualizacion = e.FechaActualizacion
            })
            .FirstOrDefaultAsync();
    }

    public async Task<EquipoDetalleDto> CrearAsync(CrearEquipoDto dto)
    {
        await ValidarRelacionesAsync(
            dto.CategoriaEquipoId,
            dto.TipoEquipoId,
            dto.EstadoEquipoId,
            dto.UbicacionId);

        var existeCodigo = await _context.Equipos
            .AnyAsync(e => e.Codigo == dto.Codigo);

        if (existeCodigo)
            throw new InvalidOperationException("Ya existe un equipo con ese código.");

        if (!string.IsNullOrWhiteSpace(dto.NumeroSerie))
        {
            var existeSerie = await _context.Equipos
                .AnyAsync(e => e.NumeroSerie == dto.NumeroSerie);

            if (existeSerie)
                throw new InvalidOperationException("Ya existe un equipo con ese número de serie.");
        }

        var equipo = new Equipo
        {
            Codigo = dto.Codigo.Trim(),
            Nombre = dto.Nombre.Trim(),
            CategoriaEquipoId = dto.CategoriaEquipoId,
            TipoEquipoId = dto.TipoEquipoId,
            EstadoEquipoId = dto.EstadoEquipoId,
            UbicacionId = dto.UbicacionId,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            NumeroSerie = dto.NumeroSerie,
            FechaCompra = dto.FechaCompra,
            FechaInstalacion = dto.FechaInstalacion,
            Observaciones = dto.Observaciones,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        _context.Equipos.Add(equipo);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Creación de equipo",
            Modulo = "Equipos",
            TablaAfectada = "Equipos",
            RegistroId = equipo.Id,
            Descripcion =
                $"Se creó el equipo '{equipo.Codigo} - {equipo.Nombre}'. " +
                $"CategoríaId: {equipo.CategoriaEquipoId}. TipoEquipoId: {equipo.TipoEquipoId}. " +
                $"EstadoEquipoId: {equipo.EstadoEquipoId}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creado = await ObtenerPorIdAsync(equipo.Id);

        return creado!;
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarEquipoDto dto)
    {
        var equipo = await _context.Equipos.FirstOrDefaultAsync(e => e.Id == id);

        if (equipo == null)
            return false;

        var codigoAnterior = equipo.Codigo;
        var nombreAnterior = equipo.Nombre;
        var estadoAnteriorId = equipo.EstadoEquipoId;
        var ubicacionAnteriorId = equipo.UbicacionId;

        await ValidarRelacionesAsync(
            dto.CategoriaEquipoId,
            dto.TipoEquipoId,
            dto.EstadoEquipoId,
            dto.UbicacionId);

        var existeCodigo = await _context.Equipos
            .AnyAsync(e => e.Codigo == dto.Codigo && e.Id != id);

        if (existeCodigo)
            throw new InvalidOperationException("Ya existe otro equipo con ese código.");

        if (!string.IsNullOrWhiteSpace(dto.NumeroSerie))
        {
            var existeSerie = await _context.Equipos
                .AnyAsync(e => e.NumeroSerie == dto.NumeroSerie && e.Id != id);

            if (existeSerie)
                throw new InvalidOperationException("Ya existe otro equipo con ese número de serie.");
        }

        equipo.Codigo = dto.Codigo.Trim();
        equipo.Nombre = dto.Nombre.Trim();
        equipo.CategoriaEquipoId = dto.CategoriaEquipoId;
        equipo.TipoEquipoId = dto.TipoEquipoId;
        equipo.EstadoEquipoId = dto.EstadoEquipoId;
        equipo.UbicacionId = dto.UbicacionId;
        equipo.Marca = dto.Marca;
        equipo.Modelo = dto.Modelo;
        equipo.NumeroSerie = dto.NumeroSerie;
        equipo.FechaCompra = dto.FechaCompra;
        equipo.FechaInstalacion = dto.FechaInstalacion;
        equipo.Observaciones = dto.Observaciones;
        equipo.Activo = dto.Activo;
        equipo.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Actualización de equipo",
            Modulo = "Equipos",
            TablaAfectada = "Equipos",
            RegistroId = equipo.Id,
            Descripcion =
                $"Se actualizó el equipo Id {equipo.Id}. " +
                $"Código anterior: {codigoAnterior}. Código actual: {equipo.Codigo}. " +
                $"Nombre anterior: {nombreAnterior}. Nombre actual: {equipo.Nombre}. " +
                $"Estado anterior Id: {estadoAnteriorId}. Estado actual Id: {equipo.EstadoEquipoId}. " +
                $"Ubicación anterior Id: {ubicacionAnteriorId?.ToString() ?? "Sin ubicación"}. " +
                $"Ubicación actual Id: {equipo.UbicacionId?.ToString() ?? "Sin ubicación"}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> DesactivarAsync(int id)
    {
        var equipo = await _context.Equipos.FirstOrDefaultAsync(e => e.Id == id);

        if (equipo == null)
            return false;

        equipo.Activo = false;
        equipo.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Desactivación de equipo",
            Modulo = "Equipos",
            TablaAfectada = "Equipos",
            RegistroId = equipo.Id,
            Descripcion =
                $"Se desactivó el equipo '{equipo.Codigo} - {equipo.Nombre}'.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    private async Task ValidarRelacionesAsync(
        int categoriaEquipoId,
        int tipoEquipoId,
        int estadoEquipoId,
        int? ubicacionId)
    {
        var existeCategoria = await _context.CategoriasEquipo
            .AnyAsync(c => c.Id == categoriaEquipoId);

        if (!existeCategoria)
            throw new InvalidOperationException("La categoría de equipo no existe.");

        var existeTipo = await _context.TiposEquipo
            .AnyAsync(t => t.Id == tipoEquipoId && t.CategoriaEquipoId == categoriaEquipoId);

        if (!existeTipo)
            throw new InvalidOperationException("El tipo de equipo no existe o no pertenece a la categoría seleccionada.");

        var existeEstado = await _context.EstadosEquipo
            .AnyAsync(e => e.Id == estadoEquipoId && e.Activo);

        if (!existeEstado)
            throw new InvalidOperationException("El estado de equipo no existe o no está activo.");

        if (ubicacionId.HasValue)
        {
            var existeUbicacion = await _context.Ubicaciones
                .AnyAsync(u => u.Id == ubicacionId.Value && u.Activo);

            if (!existeUbicacion)
                throw new InvalidOperationException("La ubicación no existe o no está activa.");
        }
    }
}