using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

/// <summary>
/// Implementación del servicio de navegación.
/// El MainShell se registra como proveedor al arrancar.
/// </summary>
public class NavigationService : INavigationService
{
    private INavigation? _navigation;
    private Action<string>? _navegarAModulo;

    // El MainShell llama esto al inicializarse
    public void Inicializar(INavigation navigation, Action<string> navegarAModulo)
    {
        _navigation = navigation;
        _navegarAModulo = navegarAModulo;
    }

    public Task PushAsync(Page page)
    {
        if (_navigation == null)
            throw new InvalidOperationException("NavigationService no inicializado.");
        return _navigation.PushModalAsync(page);
    }

    public Task PopAsync()
    {
        if (_navigation == null)
            throw new InvalidOperationException("NavigationService no inicializado.");
        return _navigation.PopModalAsync();
    }

    public void NavegarAModulo(string modulo)
    {
        _navegarAModulo?.Invoke(modulo);
    }
}
