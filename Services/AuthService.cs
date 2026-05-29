using ControlEquiposElectronicos.DTOs.Auth;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

public class AuthService : IAuthService
{
    private readonly IApiService _apiService;

    public AuthService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<LoginResponseDto?> LoginAsync(string nickname, string contrasena)
    {
        var datos = new LoginRequestDto
        {
            Nickname = nickname,
            Contrasena = contrasena
        };

        // Si la API devuelve null o falla (credenciales incorrectas), retorna null
        return await _apiService.PostAsync<LoginRequestDto, LoginResponseDto>("Auth/login", datos);
    }
}
 