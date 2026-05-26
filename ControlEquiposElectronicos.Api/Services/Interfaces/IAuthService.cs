using ControlEquiposElectronicos.Api.DTOs.Auth;

namespace ControlEquiposElectronicos.Api.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
}