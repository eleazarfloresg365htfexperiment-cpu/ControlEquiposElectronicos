using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auth;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var nickname = dto.Nickname.Trim();
        var contrasena = dto.Contrasena.Trim();

        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u =>
                u.Nickname == nickname &&
                u.PasswordHash == contrasena &&
                u.Activo);

        if (usuario == null)
            return null;

        var permisos = await _context.RolPermisos
            .Include(rp => rp.Permiso)
            .Where(rp => rp.RolId == usuario.RolId)
            .OrderBy(rp => rp.Permiso.Modulo)
            .ThenBy(rp => rp.Permiso.Accion)
            .Select(rp => new PermisoSesionDto
            {
                PermisoId = rp.Permiso.Id,
                Modulo = rp.Permiso.Modulo,
                Accion = rp.Permiso.Accion,
                Descripcion = rp.Permiso != null ? rp.Permiso.Descripcion : string.Empty
            })
            .ToListAsync();

        return new LoginResponseDto
        {
            UsuarioId = usuario.UsuarioId,
            Nickname = usuario.Nickname,
            NombreCompleto = (usuario.Nombres + " " + usuario.Apellidos).Trim(),
            RolId = usuario.RolId,
            Rol = usuario.Rol?.NombreRol ?? string.Empty,
            Permisos = permisos
        };
    }
}