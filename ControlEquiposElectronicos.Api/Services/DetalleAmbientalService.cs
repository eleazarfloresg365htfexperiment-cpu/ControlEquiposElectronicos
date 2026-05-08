using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class DetalleAmbientalService : IDetalleAmbientalService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public DetalleAmbientalService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<DetalleAmbientalDto?> ObtenerPorEquipoIdAsync(int equipoId)
    {
        return await _context.DetallesAmbiental
            .Include(d => d.Equipo)
            .Where(d => d.EquipoId == equipoId)
            .Select(d => new DetalleAmbientalDto
            {
                Id = d.Id,
                EquipoId = d.EquipoId,
                CodigoEquipo = d.Equipo.Codigo,
                NombreEquipo = d.Equipo.Nombre,
                TipoAmbiental = d.TipoAmbiental,
                BTU = d.BTU,
                FechaUltimoServicio = d.FechaUltimoServicio
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DetalleAmbientalDto> CrearAsync(int equipoId, CrearDetalleAmbientalDto dto)
    {
        var equipo = await _context.Equipos
            .FirstOrDefaultAsync(e => e.Id == equipoId);

        if (equipo == null)
            throw new InvalidOperationException("El equipo no existe.");

        var yaTieneDetalle = await _context.DetallesAmbiental
            .AnyAsync(d => d.EquipoId == equipoId);

        if (yaTieneDetalle)
            throw new InvalidOperationException("Este equipo ya tiene detalle ambiental registrado.");

        var detalle = new DetalleAmbiental
        {
            EquipoId = equipoId,
            TipoAmbiental = dto.TipoAmbiental,
            BTU = dto.BTU,
            FechaUltimoServicio = dto.FechaUltimoServicio
        };

        _context.DetallesAmbiental.Add(detalle);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Creación de detalle ambiental",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleAmbiental",
            RegistroId = detalle.Id,
            Descripcion =
                $"Se creó el detalle ambiental para el equipo '{equipo.Codigo} - {equipo.Nombre}'. " +
                $"Tipo ambiental: {detalle.TipoAmbiental}. BTU: {detalle.BTU}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creado = await ObtenerPorEquipoIdAsync(equipoId);

        return creado!;
    }

    public async Task<bool> ActualizarAsync(int equipoId, ActualizarDetalleAmbientalDto dto)
    {
        var detalle = await _context.DetallesAmbiental
            .Include(d => d.Equipo)
            .FirstOrDefaultAsync(d => d.EquipoId == equipoId);

        if (detalle == null)
            return false;

        var tipoAnterior = detalle.TipoAmbiental;
        var btuAnterior = detalle.BTU;

        detalle.TipoAmbiental = dto.TipoAmbiental;
        detalle.BTU = dto.BTU;
        detalle.FechaUltimoServicio = dto.FechaUltimoServicio;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Actualización de detalle ambiental",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleAmbiental",
            RegistroId = detalle.Id,
            Descripcion =
                $"Se actualizó el detalle ambiental del equipo '{detalle.Equipo.Codigo} - {detalle.Equipo.Nombre}'. " +
                $"Tipo anterior: {tipoAnterior}. Tipo actual: {detalle.TipoAmbiental}. " +
                $"BTU anterior: {btuAnterior}. BTU actual: {detalle.BTU}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> EliminarAsync(int equipoId)
    {
        var detalle = await _context.DetallesAmbiental
            .Include(d => d.Equipo)
            .FirstOrDefaultAsync(d => d.EquipoId == equipoId);

        if (detalle == null)
            return false;

        var detalleId = detalle.Id;
        var codigoEquipo = detalle.Equipo.Codigo;
        var nombreEquipo = detalle.Equipo.Nombre;
        var tipoAmbiental = detalle.TipoAmbiental;
        var btu = detalle.BTU;

        _context.DetallesAmbiental.Remove(detalle);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Eliminación de detalle ambiental",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleAmbiental",
            RegistroId = detalleId,
            Descripcion =
                $"Se eliminó el detalle ambiental del equipo '{codigoEquipo} - {nombreEquipo}'. " +
                $"Tipo registrado: {tipoAmbiental}. BTU registrado: {btu}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }
}