using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class SwitchPuertoService : ISwitchPuertoService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public SwitchPuertoService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<List<SwitchPuertoDto>> ObtenerPorSwitchAsync(int switchEquipoId)
    {
        return await _context.SwitchPuertos
            .Include(p => p.SwitchEquipo)
            .Include(p => p.EquipoConectado)
            .Include(p => p.UbicacionDestino)
            .Where(p => p.SwitchEquipoId == switchEquipoId)
            .OrderBy(p => p.NumeroPuerto)
            .Select(p => new SwitchPuertoDto
            {
                Id = p.Id,

                SwitchEquipoId = p.SwitchEquipoId,
                CodigoSwitch = p.SwitchEquipo.Codigo,
                NombreSwitch = p.SwitchEquipo.Nombre,

                NumeroPuerto = p.NumeroPuerto,

                EstaActivo = p.EstaActivo,
                EstaDanado = p.EstaDanado,
                EstaOcupado = p.EstaOcupado,

                EquipoConectadoId = p.EquipoConectadoId,
                CodigoEquipoConectado = p.EquipoConectado != null ? p.EquipoConectado.Codigo : null,
                NombreEquipoConectado = p.EquipoConectado != null ? p.EquipoConectado.Nombre : null,

                UbicacionDestinoId = p.UbicacionDestinoId,
                UbicacionDestino = p.UbicacionDestino != null ? p.UbicacionDestino.Nombre : null,

                Observaciones = p.Observaciones,

                FechaCreacion = p.FechaCreacion,
                FechaActualizacion = p.FechaActualizacion
            })
            .ToListAsync();
    }

    public async Task<SwitchPuertoDto?> ObtenerPorIdAsync(int id)
    {
        return await _context.SwitchPuertos
            .Include(p => p.SwitchEquipo)
            .Include(p => p.EquipoConectado)
            .Include(p => p.UbicacionDestino)
            .Where(p => p.Id == id)
            .Select(p => new SwitchPuertoDto
            {
                Id = p.Id,

                SwitchEquipoId = p.SwitchEquipoId,
                CodigoSwitch = p.SwitchEquipo.Codigo,
                NombreSwitch = p.SwitchEquipo.Nombre,

                NumeroPuerto = p.NumeroPuerto,

                EstaActivo = p.EstaActivo,
                EstaDanado = p.EstaDanado,
                EstaOcupado = p.EstaOcupado,

                EquipoConectadoId = p.EquipoConectadoId,
                CodigoEquipoConectado = p.EquipoConectado != null ? p.EquipoConectado.Codigo : null,
                NombreEquipoConectado = p.EquipoConectado != null ? p.EquipoConectado.Nombre : null,

                UbicacionDestinoId = p.UbicacionDestinoId,
                UbicacionDestino = p.UbicacionDestino != null ? p.UbicacionDestino.Nombre : null,

                Observaciones = p.Observaciones,

                FechaCreacion = p.FechaCreacion,
                FechaActualizacion = p.FechaActualizacion
            })
            .FirstOrDefaultAsync();
    }

    public async Task<SwitchPuertoDto> CrearAsync(int switchEquipoId, CrearSwitchPuertoDto dto)
    {
        await ValidarSwitchAsync(switchEquipoId);
        await ValidarRelacionesAsync(switchEquipoId, dto.EquipoConectadoId, dto.UbicacionDestinoId);
        ValidarEstadoPuerto(dto.NumeroPuerto, dto.EstaDanado, dto.EstaOcupado, dto.EquipoConectadoId);

        var switchEquipo = await _context.Equipos
            .FirstAsync(e => e.Id == switchEquipoId);

        var puertoDuplicado = await _context.SwitchPuertos
            .AnyAsync(p => p.SwitchEquipoId == switchEquipoId && p.NumeroPuerto == dto.NumeroPuerto);

        if (puertoDuplicado)
            throw new InvalidOperationException("Este switch ya tiene registrado ese número de puerto.");

        var puerto = new SwitchPuerto
        {
            SwitchEquipoId = switchEquipoId,
            NumeroPuerto = dto.NumeroPuerto,
            EstaActivo = dto.EstaActivo,
            EstaDanado = dto.EstaDanado,
            EstaOcupado = dto.EstaOcupado,
            EquipoConectadoId = dto.EquipoConectadoId,
            UbicacionDestinoId = dto.UbicacionDestinoId,
            Observaciones = dto.Observaciones,
            FechaCreacion = DateTime.UtcNow
        };

        _context.SwitchPuertos.Add(puerto);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Creación de puerto de switch",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "SwitchPuertos",
            RegistroId = puerto.Id,
            Descripcion =
                $"Se creó el puerto {puerto.NumeroPuerto} para el switch '{switchEquipo.Codigo} - {switchEquipo.Nombre}'. " +
                $"Activo: {puerto.EstaActivo}. Dañado: {puerto.EstaDanado}. Ocupado: {puerto.EstaOcupado}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creado = await ObtenerPorIdAsync(puerto.Id);

        return creado!;
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarSwitchPuertoDto dto)
    {
        var puerto = await _context.SwitchPuertos
            .Include(p => p.SwitchEquipo)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (puerto == null)
            return false;

        await ValidarRelacionesAsync(puerto.SwitchEquipoId, dto.EquipoConectadoId, dto.UbicacionDestinoId);
        ValidarEstadoPuerto(dto.NumeroPuerto, dto.EstaDanado, dto.EstaOcupado, dto.EquipoConectadoId);

        var puertoDuplicado = await _context.SwitchPuertos
            .AnyAsync(p =>
                p.SwitchEquipoId == puerto.SwitchEquipoId &&
                p.NumeroPuerto == dto.NumeroPuerto &&
                p.Id != id);

        if (puertoDuplicado)
            throw new InvalidOperationException("Este switch ya tiene otro puerto con ese número.");

        var numeroAnterior = puerto.NumeroPuerto;
        var danadoAnterior = puerto.EstaDanado;
        var ocupadoAnterior = puerto.EstaOcupado;

        puerto.NumeroPuerto = dto.NumeroPuerto;
        puerto.EstaActivo = dto.EstaActivo;
        puerto.EstaDanado = dto.EstaDanado;
        puerto.EstaOcupado = dto.EstaOcupado;
        puerto.EquipoConectadoId = dto.EquipoConectadoId;
        puerto.UbicacionDestinoId = dto.UbicacionDestinoId;
        puerto.Observaciones = dto.Observaciones;
        puerto.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Actualización de puerto de switch",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "SwitchPuertos",
            RegistroId = puerto.Id,
            Descripcion =
                $"Se actualizó un puerto del switch '{puerto.SwitchEquipo.Codigo} - {puerto.SwitchEquipo.Nombre}'. " +
                $"Puerto anterior: {numeroAnterior}. Puerto actual: {puerto.NumeroPuerto}. " +
                $"Dañado anterior: {danadoAnterior}. Dañado actual: {puerto.EstaDanado}. " +
                $"Ocupado anterior: {ocupadoAnterior}. Ocupado actual: {puerto.EstaOcupado}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var puerto = await _context.SwitchPuertos
            .Include(p => p.SwitchEquipo)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (puerto == null)
            return false;

        var puertoId = puerto.Id;
        var numeroPuerto = puerto.NumeroPuerto;
        var codigoSwitch = puerto.SwitchEquipo.Codigo;
        var nombreSwitch = puerto.SwitchEquipo.Nombre;

        _context.SwitchPuertos.Remove(puerto);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Eliminación de puerto de switch",
            Modulo = "DetallesTecnicos",
            TablaAfectada = "SwitchPuertos",
            RegistroId = puertoId,
            Descripcion =
                $"Se eliminó el puerto {numeroPuerto} del switch '{codigoSwitch} - {nombreSwitch}'.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    private async Task ValidarSwitchAsync(int switchEquipoId)
    {
        var existeSwitch = await _context.Equipos
            .Include(e => e.TipoEquipo)
            .AnyAsync(e =>
                e.Id == switchEquipoId &&
                e.Activo &&
                e.TipoEquipo.Nombre == "Switch");

        if (!existeSwitch)
            throw new InvalidOperationException("El equipo indicado no existe, no está activo o no es de tipo Switch.");
    }

    private async Task ValidarRelacionesAsync(
        int switchEquipoId,
        int? equipoConectadoId,
        int? ubicacionDestinoId)
    {
        if (equipoConectadoId.HasValue)
        {
            if (equipoConectadoId.Value == switchEquipoId)
                throw new InvalidOperationException("Un switch no puede conectarse a sí mismo en uno de sus puertos.");

            var equipoConectadoExiste = await _context.Equipos
                .AnyAsync(e => e.Id == equipoConectadoId.Value && e.Activo);

            if (!equipoConectadoExiste)
                throw new InvalidOperationException("El equipo conectado no existe o no está activo.");
        }

        if (ubicacionDestinoId.HasValue)
        {
            var ubicacionExiste = await _context.Ubicaciones
                .AnyAsync(u => u.Id == ubicacionDestinoId.Value && u.Activo);

            if (!ubicacionExiste)
                throw new InvalidOperationException("La ubicación destino no existe o no está activa.");
        }
    }

    private static void ValidarEstadoPuerto(
        int numeroPuerto,
        bool estaDanado,
        bool estaOcupado,
        int? equipoConectadoId)
    {
        if (numeroPuerto <= 0)
            throw new InvalidOperationException("El número de puerto debe ser mayor que cero.");

        if (estaDanado && estaOcupado)
            throw new InvalidOperationException("Un puerto dañado no debería marcarse como ocupado.");

        if (equipoConectadoId.HasValue && !estaOcupado)
            throw new InvalidOperationException("Si hay un equipo conectado, el puerto debe marcarse como ocupado.");
    }
}