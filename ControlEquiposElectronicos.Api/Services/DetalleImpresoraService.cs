using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class DetalleImpresoraService : IDetalleImpresoraService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public DetalleImpresoraService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<DetalleImpresoraDto?> ObtenerPorEquipoIdAsync(int equipoId)
    {
        return await _context.DetallesImpresora
            .Include(d => d.Equipo)
            .Where(d => d.EquipoId == equipoId)
            .Select(d => new DetalleImpresoraDto
            {
                Id = d.Id,
                EquipoId = d.EquipoId,
                CodigoEquipo = d.Equipo.Codigo,
                NombreEquipo = d.Equipo.Nombre,
                TipoImpresora = d.TipoImpresora,
                TipoCartucho = d.TipoCartucho,
                ModeloCartucho = d.ModeloCartucho,
                FechaUltimoCambioCartucho = d.FechaUltimoCambioCartucho,
                ContadorImpresiones = d.ContadorImpresiones
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DetalleImpresoraDto> CrearAsync(int equipoId, CrearDetalleImpresoraDto dto)
    {
        var equipo = await _context.Equipos
            .FirstOrDefaultAsync(e => e.Id == equipoId);

        if (equipo == null)
            throw new InvalidOperationException("El equipo no existe.");

        var yaTieneDetalle = await _context.DetallesImpresora
            .AnyAsync(d => d.EquipoId == equipoId);

        if (yaTieneDetalle)
            throw new InvalidOperationException("Este equipo ya tiene detalle de impresora registrado.");

        var detalle = new DetalleImpresora
        {
            EquipoId = equipoId,
            TipoImpresora = dto.TipoImpresora,
            TipoCartucho = dto.TipoCartucho,
            ModeloCartucho = dto.ModeloCartucho,
            FechaUltimoCambioCartucho = dto.FechaUltimoCambioCartucho,
            ContadorImpresiones = dto.ContadorImpresiones
        };

        _context.DetallesImpresora.Add(detalle);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Creación de detalle de impresora",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleImpresora",
            RegistroId = detalle.Id,
            Descripcion =
                $"Se creó el detalle de impresora para el equipo '{equipo.Codigo} - {equipo.Nombre}'. " +
                $"Tipo: {detalle.TipoImpresora}. Cartucho: {detalle.ModeloCartucho}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creado = await ObtenerPorEquipoIdAsync(equipoId);

        return creado!;
    }

    public async Task<bool> ActualizarAsync(int equipoId, ActualizarDetalleImpresoraDto dto)
    {
        var detalle = await _context.DetallesImpresora
            .Include(d => d.Equipo)
            .FirstOrDefaultAsync(d => d.EquipoId == equipoId);

        if (detalle == null)
            return false;

        var tipoAnterior = detalle.TipoImpresora;
        var cartuchoAnterior = detalle.ModeloCartucho;
        var contadorAnterior = detalle.ContadorImpresiones;

        detalle.TipoImpresora = dto.TipoImpresora;
        detalle.TipoCartucho = dto.TipoCartucho;
        detalle.ModeloCartucho = dto.ModeloCartucho;
        detalle.FechaUltimoCambioCartucho = dto.FechaUltimoCambioCartucho;
        detalle.ContadorImpresiones = dto.ContadorImpresiones;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Actualización de detalle de impresora",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleImpresora",
            RegistroId = detalle.Id,
            Descripcion =
                $"Se actualizó el detalle de impresora del equipo '{detalle.Equipo.Codigo} - {detalle.Equipo.Nombre}'. " +
                $"Tipo anterior: {tipoAnterior}. Tipo actual: {detalle.TipoImpresora}. " +
                $"Cartucho anterior: {cartuchoAnterior}. Cartucho actual: {detalle.ModeloCartucho}. " +
                $"Contador anterior: {contadorAnterior}. Contador actual: {detalle.ContadorImpresiones}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> EliminarAsync(int equipoId)
    {
        var detalle = await _context.DetallesImpresora
            .Include(d => d.Equipo)
            .FirstOrDefaultAsync(d => d.EquipoId == equipoId);

        if (detalle == null)
            return false;

        var detalleId = detalle.Id;
        var codigoEquipo = detalle.Equipo.Codigo;
        var nombreEquipo = detalle.Equipo.Nombre;
        var tipoImpresora = detalle.TipoImpresora;
        var modeloCartucho = detalle.ModeloCartucho;

        _context.DetallesImpresora.Remove(detalle);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Eliminación de detalle de impresora",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleImpresora",
            RegistroId = detalleId,
            Descripcion =
                $"Se eliminó el detalle de impresora del equipo '{codigoEquipo} - {nombreEquipo}'. " +
                $"Tipo registrado: {tipoImpresora}. Cartucho registrado: {modeloCartucho}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }
}