using ControlEquiposElectronicos.DTOs.Auditoria;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Views.Configuracion;

public partial class AuditoriaPage : ContentPage
{
    private readonly IAuditoriaApiService _auditoriaApi;
    private readonly SesionService _sesion;
    private List<HistorialOperacionDto> _todosLosRegistros = new();

    public AuditoriaPage()
    {
        InitializeComponent();
        var services = IPlatformApplication.Current!.Services;
        _auditoriaApi = services.GetRequiredService<IAuditoriaApiService>();
        _sesion = services.GetRequiredService<SesionService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Solo OP y Administrador pueden ver la auditoría
        var rol = _sesion.UsuarioActual?.Rol;
        if (rol != "OP" && rol != "Administrador")
        {
            await DisplayAlert("Acceso restringido",
                "Solo OP y Administrador pueden ver la auditoría.", "Entendido");
            await Shell.Current.GoToAsync("..");
            return;
        }

        await CargarAsync();
        ModuloPicker.SelectedIndex = 0; // "Todos" por defecto
    }

    private async Task CargarAsync()
    {
        try
        {
            CargandoIndicator.IsRunning = true;
            CargandoIndicator.IsVisible = true;

            _todosLosRegistros = await _auditoriaApi.ObtenerTodosAsync();
            AuditoriaCollection.ItemsSource = _todosLosRegistros;
        }
        catch
        {
            _todosLosRegistros = new();
            AuditoriaCollection.ItemsSource = _todosLosRegistros;
            await DisplayAlert("Error", "No se pudo cargar el historial de auditoría.", "Entendido");
        }
        finally
        {
            CargandoIndicator.IsRunning = false;
            CargandoIndicator.IsVisible = false;
        }
    }

    private void OnBusquedaTextChanged(object? sender, TextChangedEventArgs e)
    {
        AplicarFiltros();
    }

    private void OnModuloFiltrado(object? sender, EventArgs e)
    {
        AplicarFiltros();
    }

    private void AplicarFiltros()
    {
        var texto = BusquedaEntry.Text?.Trim().ToLower() ?? string.Empty;
        var modulo = ModuloPicker.SelectedItem?.ToString() ?? "Todos";

        var filtrados = _todosLosRegistros.AsEnumerable();

        if (modulo != "Todos")
            filtrados = filtrados.Where(r =>
                r.Modulo.Equals(modulo, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(texto))
            filtrados = filtrados.Where(r =>
                (r.Accion?.ToLower().Contains(texto) ?? false) ||
                (r.Modulo?.ToLower().Contains(texto) ?? false) ||
                (r.Usuario?.ToLower().Contains(texto) ?? false) ||
                (r.Descripcion?.ToLower().Contains(texto) ?? false));

        AuditoriaCollection.ItemsSource = filtrados.ToList();
    }

    private async void OnRegistroTapped(object? sender, TappedEventArgs e)
    {
        if (sender is not BindableObject bindable) return;
        if (bindable.BindingContext is not HistorialOperacionDto r) return;

        var fecha = r.FechaOperacion.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
        var usuario = string.IsNullOrEmpty(r.Usuario) ? "Sistema" : $"@{r.Usuario}";
        var tabla = string.IsNullOrEmpty(r.TablaAfectada) ? "-" : r.TablaAfectada;
        var ip = string.IsNullOrEmpty(r.DireccionIP) ? "-" : r.DireccionIP;
        var desc = string.IsNullOrEmpty(r.Descripcion) ? "Sin descripción." : r.Descripcion;

        await DisplayAlert(
            $"{r.Accion}",
            $"Módulo: {r.Modulo}\n" +
            $"Usuario: {usuario}\n" +
            $"Fecha: {fecha}\n" +
            $"Tabla: {tabla}\n" +
            $"ID registro: {r.RegistroId?.ToString() ?? "-"}\n" +
            $"IP: {ip}\n\n" +
            $"Detalle:\n{desc}",
            "Cerrar");
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}
