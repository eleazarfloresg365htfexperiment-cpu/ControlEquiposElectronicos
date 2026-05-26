using ControlEquiposElectronicos.Api.Services.Interfaces;

namespace ControlEquiposElectronicos.Api.Services;

public class UsuarioActualService : IUsuarioActualService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioActualService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int ObtenerUsuarioId()
    {
        // Temporal mientras no usamos autenticación con token/JWT.
        // Por ahora usamos el usuario de prueba con Id = 1.
        return 1;
    }

    public string? ObtenerDireccionIP()
    {
        return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    }
}