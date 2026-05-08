using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class DetalleComputadoraService : IDetalleComputadoraService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public DetalleComputadoraService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<DetalleComputadoraDto?> ObtenerPorEquipoIdAsync(int equipoId)
    {
        return await _context.DetallesComputadora
            .Include(d => d.Equipo)
            .Where(d => d.EquipoId == equipoId)
            .Select(d => new DetalleComputadoraDto
            {
                Id = d.Id,
                EquipoId = d.EquipoId,
                CodigoEquipo = d.Equipo.Codigo,
                NombreEquipo = d.Equipo.Nombre,
                Procesador = d.Procesador,
                RamGB = d.RamGB,
                SerialRam = d.SerialRam,
                AlmacenamientoTipo = d.AlmacenamientoTipo,
                AlmacenamientoCapacidadGB = d.AlmacenamientoCapacidadGB,
                SistemaOperativo = d.SistemaOperativo,
                NumeroEquipo = d.NumeroEquipo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DetalleComputadoraDto> CrearAsync(int equipoId, CrearDetalleComputadoraDto dto)
    {
        var equipo = await _context.Equipos
            .FirstOrDefaultAsync(e => e.Id == equipoId);

        if (equipo == null)
            throw new InvalidOperationException("El equipo no existe.");

        var yaTieneDetalle = await _context.DetallesComputadora
            .AnyAsync(d => d.EquipoId == equipoId);

        if (yaTieneDetalle)
            throw new InvalidOperationException("Este equipo ya tiene detalle de computadora registrado.");

        var detalle = new DetalleComputadora
        {
            EquipoId = equipoId,
            Procesador = dto.Procesador,
            RamGB = dto.RamGB,
            SerialRam = dto.SerialRam,
            AlmacenamientoTipo = dto.AlmacenamientoTipo,
            AlmacenamientoCapacidadGB = dto.AlmacenamientoCapacidadGB,
            SistemaOperativo = dto.SistemaOperativo,
            NumeroEquipo = dto.NumeroEquipo
        };

        _context.DetallesComputadora.Add(detalle);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Creación de detalle de computadora",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleComputadora",
            RegistroId = detalle.Id,
            Descripcion =
                $"Se creó el detalle de computadora para el equipo '{equipo.Codigo} - {equipo.Nombre}'. " +
                $"Procesador: {detalle.Procesador}. RAM: {detalle.RamGB} GB. " +
                $"Almacenamiento: {detalle.AlmacenamientoTipo} {detalle.AlmacenamientoCapacidadGB} GB.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creado = await ObtenerPorEquipoIdAsync(equipoId);

        return creado!;
    }

    public async Task<bool> ActualizarAsync(int equipoId, ActualizarDetalleComputadoraDto dto)
    {
        var detalle = await _context.DetallesComputadora
            .Include(d => d.Equipo)
            .FirstOrDefaultAsync(d => d.EquipoId == equipoId);

        if (detalle == null)
            return false;

        var procesadorAnterior = detalle.Procesador;
        var ramAnterior = detalle.RamGB;
        var almacenamientoAnterior = $"{detalle.AlmacenamientoTipo} {detalle.AlmacenamientoCapacidadGB} GB";
        var sistemaAnterior = detalle.SistemaOperativo;

        detalle.Procesador = dto.Procesador;
        detalle.RamGB = dto.RamGB;
        detalle.SerialRam = dto.SerialRam;
        detalle.AlmacenamientoTipo = dto.AlmacenamientoTipo;
        detalle.AlmacenamientoCapacidadGB = dto.AlmacenamientoCapacidadGB;
        detalle.SistemaOperativo = dto.SistemaOperativo;
        detalle.NumeroEquipo = dto.NumeroEquipo;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Actualización de detalle de computadora",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleComputadora",
            RegistroId = detalle.Id,
            Descripcion =
                $"Se actualizó el detalle de computadora del equipo '{detalle.Equipo.Codigo} - {detalle.Equipo.Nombre}'. " +
                $"Procesador anterior: {procesadorAnterior}. Procesador actual: {detalle.Procesador}. " +
                $"RAM anterior: {ramAnterior} GB. RAM actual: {detalle.RamGB} GB. " +
                $"Almacenamiento anterior: {almacenamientoAnterior}. " +
                $"Almacenamiento actual: {detalle.AlmacenamientoTipo} {detalle.AlmacenamientoCapacidadGB} GB. " +
                $"Sistema anterior: {sistemaAnterior}. Sistema actual: {detalle.SistemaOperativo}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> EliminarAsync(int equipoId)
    {
        var detalle = await _context.DetallesComputadora
            .Include(d => d.Equipo)
            .FirstOrDefaultAsync(d => d.EquipoId == equipoId);

        if (detalle == null)
            return false;

        var detalleId = detalle.Id;
        var codigoEquipo = detalle.Equipo.Codigo;
        var nombreEquipo = detalle.Equipo.Nombre;
        var procesador = detalle.Procesador;
        var ram = detalle.RamGB;

        _context.DetallesComputadora.Remove(detalle);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Eliminación de detalle de computadora",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleComputadora",
            RegistroId = detalleId,
            Descripcion =
                $"Se eliminó el detalle de computadora del equipo '{codigoEquipo} - {nombreEquipo}'. " +
                $"Procesador registrado: {procesador}. RAM registrada: {ram} GB.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }
}