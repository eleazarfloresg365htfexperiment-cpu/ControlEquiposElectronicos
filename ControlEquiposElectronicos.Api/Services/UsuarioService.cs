using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Usuarios;
using ControlEquiposElectronicos.Api.Entities.Auditoria;
using ControlEquiposElectronicos.Api.Entities.Seguridad;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;
    private readonly IUsuarioActualService _usuarioActualService;

    public UsuarioService(
        AppDbContext context,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<List<UsuarioDto>> ObtenerTodosAsync()
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .OrderBy(u => u.Nickname)
            .Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                NombreCompleto = (u.Nombres + " " + u.Apellidos).Trim(),
                Nickname = u.Nickname,
                Telefono = u.Telefono,
                Correo = u.Correo,
                RolId = u.RolId,
                Rol = u.Rol.NombreRol,
                Activo = u.Activo,
                FechaCreacion = u.FechaCreacion,
                FechaActualizacion = null
            })
            .ToListAsync();
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .Where(u => u.UsuarioId == id)
            .Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                NombreCompleto = (u.Nombres + " " + u.Apellidos).Trim(),
                Nickname = u.Nickname,
                Telefono = u.Telefono,
                Correo = u.Correo,
                RolId = u.RolId,
                Rol = u.Rol.NombreRol,
                Activo = u.Activo,
                FechaCreacion = u.FechaCreacion,
                FechaActualizacion = null
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto)
    {
        var nickname = dto.Nickname.Trim();

        if (string.IsNullOrWhiteSpace(nickname))
            throw new InvalidOperationException("El nickname es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Contrasena))
            throw new InvalidOperationException("La contraseña es obligatoria.");

        var existeUsuario = await _context.Usuarios
            .AnyAsync(u => u.Nickname == nickname || u.NombreUsuario == nickname);

        if (existeUsuario)
            throw new InvalidOperationException("Ya existe un usuario con ese nickname o nombre de usuario.");

        var rolNombre = dto.Rol.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(rolNombre))
            throw new InvalidOperationException("El rol es obligatorio.");

        var rol = await _context.Roles
            .FirstOrDefaultAsync(r => r.NombreRol.ToLower() == rolNombre && r.Activo);

        if (rol == null)
            throw new InvalidOperationException("El rol indicado no existe o no está activo.");

        var (nombres, apellidos) = SepararNombreCompleto(dto.NombreCompleto);

        var usuario = new Usuario
        {
            Nombres = nombres,
            Apellidos = apellidos,

            // NombreUsuario tiene índice único en SQL Server.
            // Nickname es el nombre visible/de acceso usado por MAUI.
            // Por eso ambos se llenan con el mismo valor.
            NombreUsuario = nickname,
            Nickname = nickname,

            PasswordHash = dto.Contrasena.Trim(),
            Telefono = dto.Telefono?.Trim(),
            Correo = dto.Correo?.Trim(),
            RolId = rol.RolId,
            Activo = dto.Activo,
            FechaCreacion = DateTime.UtcNow
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        await RegistrarAuditoriaCreacionUsuarioAsync(usuario, rol);

        return MapearUsuarioDto(usuario, rol.NombreRol);
    }

    public async Task<UsuarioDto?> CambiarRolAsync(int id, ActualizarRolUsuarioDto dto)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.UsuarioId == id);

        if (usuario == null)
            return null;

        var rolNombre = dto.Rol.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(rolNombre))
            throw new InvalidOperationException("El rol es obligatorio.");

        var nuevoRol = await _context.Roles
            .FirstOrDefaultAsync(r => r.NombreRol.ToLower() == rolNombre && r.Activo);

        if (nuevoRol == null)
            throw new InvalidOperationException("El rol indicado no existe o no está activo.");

        var rolAnteriorNombre = usuario.Rol?.NombreRol ?? "Sin rol";

        if (usuario.RolId == nuevoRol.RolId)
            throw new InvalidOperationException("El usuario ya tiene asignado ese rol.");

        usuario.RolId = nuevoRol.RolId;

        await _context.SaveChangesAsync();

        await RegistrarAuditoriaCambioRolUsuarioAsync(
            usuario,
            rolAnteriorNombre,
            nuevoRol.NombreRol);

        return MapearUsuarioDto(usuario, nuevoRol.NombreRol);
    }

    private async Task RegistrarAuditoriaCreacionUsuarioAsync(Usuario usuario, Rol rol)
    {
        var fechaLocal = DateTime.Now;

        var historial = new HistorialOperacion
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Creación de usuario",
            Modulo = "Usuarios",
            TablaAfectada = "Usuarios",
            RegistroId = usuario.UsuarioId,
            Descripcion =
                $"Se creó el usuario '{usuario.Nickname}' " +
                $"con nombre completo '{(usuario.Nombres + " " + usuario.Apellidos).Trim()}', " +
                $"rol '{rol.NombreRol}', " +
                $"estado '{(usuario.Activo ? "Activo" : "Inactivo")}', " +
                $"el día {fechaLocal:dd/MM/yyyy} a las {fechaLocal:HH:mm:ss}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP(),
            FechaOperacion = DateTime.UtcNow
        };

        _context.HistorialOperaciones.Add(historial);

        await _context.SaveChangesAsync();
    }

    private async Task RegistrarAuditoriaCambioRolUsuarioAsync(
        Usuario usuario,
        string rolAnterior,
        string rolNuevo)
    {
        var fechaLocal = DateTime.Now;

        var historial = new HistorialOperacion
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Cambio de rol de usuario",
            Modulo = "Usuarios",
            TablaAfectada = "Usuarios",
            RegistroId = usuario.UsuarioId,
            Descripcion =
                $"Se cambió el rol del usuario '{usuario.Nickname}' " +
                $"con nombre completo '{(usuario.Nombres + " " + usuario.Apellidos).Trim()}' " +
                $"de '{rolAnterior}' a '{rolNuevo}', " +
                $"el día {fechaLocal:dd/MM/yyyy} a las {fechaLocal:HH:mm:ss}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP(),
            FechaOperacion = DateTime.UtcNow
        };

        _context.HistorialOperaciones.Add(historial);

        await _context.SaveChangesAsync();
    }

    private static UsuarioDto MapearUsuarioDto(Usuario usuario, string nombreRol)
    {
        return new UsuarioDto
        {
            UsuarioId = usuario.UsuarioId,
            NombreCompleto = (usuario.Nombres + " " + usuario.Apellidos).Trim(),
            Nickname = usuario.Nickname,
            Telefono = usuario.Telefono,
            Correo = usuario.Correo,
            RolId = usuario.RolId,
            Rol = nombreRol,
            Activo = usuario.Activo,
            FechaCreacion = usuario.FechaCreacion,
            FechaActualizacion = null
        };
    }

    private static (string Nombres, string Apellidos) SepararNombreCompleto(string nombreCompleto)
    {
        var partes = nombreCompleto
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (partes.Length == 0)
            throw new InvalidOperationException("El nombre completo es obligatorio.");

        if (partes.Length == 1)
            return (partes[0], "No especificado");

        if (partes.Length == 2)
            return (partes[0], partes[1]);

        if (partes.Length == 3)
            return (partes[0], string.Join(" ", partes.Skip(1)));

        var nombres = string.Join(" ", partes.Take(2));
        var apellidos = string.Join(" ", partes.Skip(2));

        return (nombres, apellidos);
    }
}