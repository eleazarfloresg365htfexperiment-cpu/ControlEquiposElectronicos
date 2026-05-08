using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class DetalleUPSService : IDetalleUPSService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public DetalleUPSService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<DetalleUPSDto?> ObtenerPorEquipoIdAsync(int equipoId)
    {
        return await _context.DetallesUPS
            .Include(d => d.Equipo)
            .Where(d => d.EquipoId == equipoId)
            .Select(d => new DetalleUPSDto
            {
                Id = d.Id,
                EquipoId = d.EquipoId,
                CodigoEquipo = d.Equipo.Codigo,
                NombreEquipo = d.Equipo.Nombre,
                CapacidadVA = d.CapacidadVA,
                FechaCambioBateria = d.FechaCambioBateria,
                EstadoBateria = d.EstadoBateria
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DetalleUPSDto> CrearAsync(int equipoId, CrearDetalleUPSDto dto)
    {
        var equipo = await _context.Equipos
            .FirstOrDefaultAsync(e => e.Id == equipoId);

        if (equipo == null)
            throw new InvalidOperationException("El equipo no existe.");

        var yaTieneDetalle = await _context.DetallesUPS
            .AnyAsync(d => d.EquipoId == equipoId);

        if (yaTieneDetalle)
            throw new InvalidOperationException("Este equipo ya tiene detalle de UPS registrado.");

        var detalle = new DetalleUPS
        {
            EquipoId = equipoId,
            CapacidadVA = dto.CapacidadVA,
            FechaCambioBateria = dto.FechaCambioBateria,
            EstadoBateria = dto.EstadoBateria
        };

        _context.DetallesUPS.Add(detalle);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Creación de detalle de UPS",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleUPS",
            RegistroId = detalle.Id,
            Descripcion =
                $"Se creó el detalle de UPS para el equipo '{equipo.Codigo} - {equipo.Nombre}'. " +
                $"Capacidad: {detalle.CapacidadVA}. Estado de batería: {detalle.EstadoBateria}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creado = await ObtenerPorEquipoIdAsync(equipoId);

        return creado!;
    }

    public async Task<bool> ActualizarAsync(int equipoId, ActualizarDetalleUPSDto dto)
    {
        var detalle = await _context.DetallesUPS
            .Include(d => d.Equipo)
            .FirstOrDefaultAsync(d => d.EquipoId == equipoId);

        if (detalle == null)
            return false;

        var capacidadAnterior = detalle.CapacidadVA;
        var estadoBateriaAnterior = detalle.EstadoBateria;

        detalle.CapacidadVA = dto.CapacidadVA;
        detalle.FechaCambioBateria = dto.FechaCambioBateria;
        detalle.EstadoBateria = dto.EstadoBateria;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Actualización de detalle de UPS",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleUPS",
            RegistroId = detalle.Id,
            Descripcion =
                $"Se actualizó el detalle de UPS del equipo '{detalle.Equipo.Codigo} - {detalle.Equipo.Nombre}'. " +
                $"Capacidad anterior: {capacidadAnterior}. Capacidad actual: {detalle.CapacidadVA}. " +
                $"Estado batería anterior: {estadoBateriaAnterior}. Estado batería actual: {detalle.EstadoBateria}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> EliminarAsync(int equipoId)
    {
        var detalle = await _context.DetallesUPS
            .Include(d => d.Equipo)
            .FirstOrDefaultAsync(d => d.EquipoId == equipoId);

        if (detalle == null)
            return false;

        var detalleId = detalle.Id;
        var codigoEquipo = detalle.Equipo.Codigo;
        var nombreEquipo = detalle.Equipo.Nombre;
        var capacidad = detalle.CapacidadVA;
        var estadoBateria = detalle.EstadoBateria;

        _context.DetallesUPS.Remove(detalle);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Eliminación de detalle de UPS",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleUPS",
            RegistroId = detalleId,
            Descripcion =
                $"Se eliminó el detalle de UPS del equipo '{codigoEquipo} - {nombreEquipo}'. " +
                $"Capacidad registrada: {capacidad}. Estado batería registrado: {estadoBateria}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }
}