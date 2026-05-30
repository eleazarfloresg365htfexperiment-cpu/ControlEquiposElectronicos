using Microsoft.Extensions.DependencyInjection;

namespace ControlEquiposElectronicos.Helpers;

public static class ServiceHelper
{
    public static T GetRequiredService<T>()
    {
        var services = Application.Current?.Handler?.MauiContext?.Services;
        if (services == null)
            throw new InvalidOperationException($"No se pudo resolver el servicio {typeof(T).Name}.");

        return services.GetRequiredService<T>();
    }
}
