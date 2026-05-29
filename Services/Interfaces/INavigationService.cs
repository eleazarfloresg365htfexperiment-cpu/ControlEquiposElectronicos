namespace ControlEquiposElectronicos.Services.Interfaces;

/// <summary>
/// Servicio de navegación desacoplado de Shell.
/// Permite que las páginas naveguen sin depender de Shell.Current.
/// </summary>
public interface INavigationService
{
    // Navega a una página modal (push)
    Task PushAsync(Page page);

    // Cierra la página modal actual (pop)
    Task PopAsync();

    // Navega al módulo principal por nombre (reemplaza GoToAsync("//Modulo"))
    void NavegarAModulo(string modulo);
}
 