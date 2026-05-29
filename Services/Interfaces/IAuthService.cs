using ControlEquiposElectronicos.DTOs.Auth;

namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IAuthService
{
    // Intenta iniciar sesión. Devuelve los datos del usuario si las credenciales son correctas, null si no.
    Task<LoginResponseDto?> LoginAsync(string nickname, string contrasena);
}
