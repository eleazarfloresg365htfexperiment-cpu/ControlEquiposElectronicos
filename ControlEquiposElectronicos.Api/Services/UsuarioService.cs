using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Usuarios;
using ControlEquiposElectronicos.Api.Entities.Seguridad;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context;
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

        var rol = await _context.Roles
            .FirstOrDefaultAsync(r => r.NombreRol.ToLower() == rolNombre && r.Activo);

        if (rol == null)
            throw new InvalidOperationException("El rol indicado no existe o no está activo.");

        var (nombres, apellidos) = SepararNombreCompleto(dto.NombreCompleto);

        var usuario = new Usuario
        {
            Nombres = nombres,
            Apellidos = apellidos,

            // Importante:
            // NombreUsuario tiene índice único en SQL Server.
            // Nickname es el nombre visible/de acceso usado por MAUI.
            // Para evitar errores de duplicado por valor vacío, ambos se llenan.
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

        return new UsuarioDto
        {
            UsuarioId = usuario.UsuarioId,
            NombreCompleto = (usuario.Nombres + " " + usuario.Apellidos).Trim(),
            Nickname = usuario.Nickname,
            Telefono = usuario.Telefono,
            Correo = usuario.Correo,
            RolId = rol.RolId,
            Rol = rol.NombreRol,
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