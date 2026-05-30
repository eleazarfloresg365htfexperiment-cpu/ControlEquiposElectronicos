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
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
            return 1;

        if (httpContext.Request.Headers.TryGetValue("X-Usuario-Id", out var usuarioIdHeader))
        {
            var valor = usuarioIdHeader.FirstOrDefault();

            if (int.TryParse(valor, out var usuarioId) && usuarioId > 0)
                return usuarioId;
        }

        // Temporal para pruebas desde Swagger/Postman.
        return 1;
    }

    public string? ObtenerDireccionIP()
    {
        return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    }
}