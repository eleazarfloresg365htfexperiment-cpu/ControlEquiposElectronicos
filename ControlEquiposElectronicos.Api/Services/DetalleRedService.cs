using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class DetalleRedService : IDetalleRedService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public DetalleRedService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<DetalleRedDto?> ObtenerPorEquipoIdAsync(int equipoId)
    {
        return await _context.DetallesRed
            .Include(d => d.Equipo)
            .Include(d => d.EquipoProveedor)
            .Where(d => d.EquipoId == equipoId)
            .Select(d => new DetalleRedDto
            {
                Id = d.Id,

                EquipoId = d.EquipoId,
                CodigoEquipo = d.Equipo.Codigo,
                NombreEquipo = d.Equipo.Nombre,

                DireccionIP = d.DireccionIP,
                MacAddress = d.MacAddress,

                CantidadPuertos = d.CantidadPuertos,
                PuertosDanados = d.PuertosDanados,

                EquipoProveedorId = d.EquipoProveedorId,
                CodigoEquipoProveedor = d.EquipoProveedor != null ? d.EquipoProveedor.Codigo : null,
                NombreEquipoProveedor = d.EquipoProveedor != null ? d.EquipoProveedor.Nombre : null,

                ObservacionesRed = d.ObservacionesRed
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DetalleRedDto> CrearAsync(int equipoId, CrearDetalleRedDto dto)
    {
        var equipo = await _context.Equipos
            .FirstOrDefaultAsync(e => e.Id == equipoId);

        if (equipo == null)
            throw new InvalidOperationException("El equipo no existe.");

        var yaTieneDetalle = await _context.DetallesRed
            .AnyAsync(d => d.EquipoId == equipoId);

        if (yaTieneDetalle)
            throw new InvalidOperationException("Este equipo ya tiene detalle de red registrado.");

        await ValidarDetalleRedAsync(equipoId, dto.EquipoProveedorId, dto.CantidadPuertos, dto.PuertosDanados);

        var detalle = new DetalleRed
        {
            EquipoId = equipoId,
            DireccionIP = dto.DireccionIP,
            MacAddress = dto.MacAddress,
            CantidadPuertos = dto.CantidadPuertos,
            PuertosDanados = dto.PuertosDanados,
            EquipoProveedorId = dto.EquipoProveedorId,
            ObservacionesRed = dto.ObservacionesRed
        };

        _context.DetallesRed.Add(detalle);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Creación de detalle de red",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleRed",
            RegistroId = detalle.Id,
            Descripcion =
                $"Se creó el detalle de red para el equipo '{equipo.Codigo} - {equipo.Nombre}'. " +
                $"IP: {detalle.DireccionIP}. MAC: {detalle.MacAddress}. " +
                $"Puertos: {detalle.CantidadPuertos}. Puertos dañados: {detalle.PuertosDanados}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creado = await ObtenerPorEquipoIdAsync(equipoId);

        return creado!;
    }

    public async Task<bool> ActualizarAsync(int equipoId, ActualizarDetalleRedDto dto)
    {
        var detalle = await _context.DetallesRed
            .Include(d => d.Equipo)
            .FirstOrDefaultAsync(d => d.EquipoId == equipoId);

        if (detalle == null)
            return false;

        await ValidarDetalleRedAsync(equipoId, dto.EquipoProveedorId, dto.CantidadPuertos, dto.PuertosDanados);

        var ipAnterior = detalle.DireccionIP;
        var macAnterior = detalle.MacAddress;
        var puertosDanadosAnterior = detalle.PuertosDanados;

        detalle.DireccionIP = dto.DireccionIP;
        detalle.MacAddress = dto.MacAddress;
        detalle.CantidadPuertos = dto.CantidadPuertos;
        detalle.PuertosDanados = dto.PuertosDanados;
        detalle.EquipoProveedorId = dto.EquipoProveedorId;
        detalle.ObservacionesRed = dto.ObservacionesRed;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Actualización de detalle de red",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleRed",
            RegistroId = detalle.Id,
            Descripcion =
                $"Se actualizó el detalle de red del equipo '{detalle.Equipo.Codigo} - {detalle.Equipo.Nombre}'. " +
                $"IP anterior: {ipAnterior}. IP actual: {detalle.DireccionIP}. " +
                $"MAC anterior: {macAnterior}. MAC actual: {detalle.MacAddress}. " +
                $"Puertos dañados anterior: {puertosDanadosAnterior}. Puertos dañados actual: {detalle.PuertosDanados}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> EliminarAsync(int equipoId)
    {
        var detalle = await _context.DetallesRed
            .Include(d => d.Equipo)
            .FirstOrDefaultAsync(d => d.EquipoId == equipoId);

        if (detalle == null)
            return false;

        var detalleId = detalle.Id;
        var codigoEquipo = detalle.Equipo.Codigo;
        var nombreEquipo = detalle.Equipo.Nombre;
        var direccionIP = detalle.DireccionIP;
        var macAddress = detalle.MacAddress;

        _context.DetallesRed.Remove(detalle);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Eliminación de detalle de red",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "DetalleRed",
            RegistroId = detalleId,
            Descripcion =
                $"Se eliminó el detalle de red del equipo '{codigoEquipo} - {nombreEquipo}'. " +
                $"IP registrada: {direccionIP}. MAC registrada: {macAddress}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    private async Task ValidarDetalleRedAsync(
        int equipoId,
        int? equipoProveedorId,
        int? cantidadPuertos,
        int? puertosDanados)
    {
        if (equipoProveedorId.HasValue)
        {
            if (equipoProveedorId.Value == equipoId)
                throw new InvalidOperationException("Un equipo no puede ser proveedor de red de sí mismo.");

            var proveedorExiste = await _context.Equipos
                .AnyAsync(e => e.Id == equipoProveedorId.Value);

            if (!proveedorExiste)
                throw new InvalidOperationException("El equipo proveedor no existe.");
        }

        if (cantidadPuertos.HasValue && cantidadPuertos.Value < 0)
            throw new InvalidOperationException("La cantidad de puertos no puede ser negativa.");

        if (puertosDanados.HasValue && puertosDanados.Value < 0)
            throw new InvalidOperationException("La cantidad de puertos dañados no puede ser negativa.");

        if (cantidadPuertos.HasValue &&
            puertosDanados.HasValue &&
            puertosDanados.Value > cantidadPuertos.Value)
        {
            throw new InvalidOperationException("Los puertos dañados no pueden ser mayores que la cantidad total de puertos.");
        }
    }
}