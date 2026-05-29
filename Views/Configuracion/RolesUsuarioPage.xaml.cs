using ControlEquiposElectronicos.DTOs.Usuarios;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Text.Json;

namespace ControlEquiposElectronicos.Views.Configuracion;

public partial class RolesUsuarioPage : ContentPage
{
    private readonly IUsuarioApiService _usuarioApi;
    private List<UsuarioListadoDto> _todosLosUsuarios = new();

    public RolesUsuarioPage()
    {
        InitializeComponent();
        _usuarioApi = IPlatformApplication.Current!.Services.GetRequiredService<IUsuarioApiService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarUsuariosAsync();
    }

    private async Task CargarUsuariosAsync()
    {
        try
        {
            CargandoIndicator.IsRunning = true;
            CargandoIndicator.IsVisible = true;
            _todosLosUsuarios = await _usuarioApi.ObtenerTodosAsync();
            UsuariosCollection.ItemsSource = _todosLosUsuarios;
        }
        catch
        {
            _todosLosUsuarios = new();
            UsuariosCollection.ItemsSource = _todosLosUsuarios;
        }
        finally
        {
            CargandoIndicator.IsRunning = false;
            CargandoIndicator.IsVisible = false;
        }
    }

    private void OnBusquedaTextChanged(object? sender, TextChangedEventArgs e)
    {
        var texto = (e.NewTextValue ?? string.Empty).Trim().ToLower();
        UsuariosCollection.ItemsSource = string.IsNullOrEmpty(texto)
            ? _todosLosUsuarios
            : _todosLosUsuarios.Where(u =>
                u.NombreCompleto.ToLower().Contains(texto) ||
                u.Nickname.ToLower().Contains(texto) ||
                u.Rol.ToLower().Contains(texto)).ToList();
    }

    private async void OnAsignarOPClicked(object? sender, EventArgs e)
        => await CambiarRol(sender, "OP");

    private async void OnAsignarAdministradorClicked(object? sender, EventArgs e)
        => await CambiarRol(sender, "Administrador");

    private async void OnAsignarTecnicoClicked(object? sender, EventArgs e)
        => await CambiarRol(sender, "Tecnico");

    private async void OnAsignarConsultaClicked(object? sender, EventArgs e)
        => await CambiarRol(sender, "Consulta");

    private async Task CambiarRol(object? sender, string nuevoRol)
    {
        if (sender is not Button boton) return;
        if (boton.BindingContext is not UsuarioListadoDto usuario) return;

        if (usuario.Rol.Equals(nuevoRol, StringComparison.OrdinalIgnoreCase))
        {
            await DisplayAlert("Sin cambios",
                $"{usuario.NombreCompleto} ya tiene el rol «{nuevoRol}».", "Entendido");
            return;
        }

        var rolMostrar = nuevoRol == "Tecnico" ? "Técnico" : nuevoRol;
        bool confirmar = await DisplayAlert("Cambiar rol",
            $"¿Cambiar el rol de {usuario.NombreCompleto} a «{rolMostrar}»?",
            "Sí, cambiar", "Cancelar");
        if (!confirmar) return;

        var (exito, errorJson) = await _usuarioApi.CambiarRolAsync(usuario.UsuarioId, nuevoRol);

        if (exito)
        {
            await DisplayAlert("Rol actualizado",
                $"El rol de {usuario.NombreCompleto} ahora es «{rolMostrar}».", "Aceptar");
            await CargarUsuariosAsync();

            var texto = BusquedaEntry.Text?.Trim().ToLower() ?? string.Empty;
            if (!string.IsNullOrEmpty(texto))
                UsuariosCollection.ItemsSource = _todosLosUsuarios.Where(u =>
                    u.NombreCompleto.ToLower().Contains(texto) ||
                    u.Nickname.ToLower().Contains(texto) ||
                    u.Rol.ToLower().Contains(texto)).ToList();
        }
        else
        {
            var mensajeApi = ExtraerMensaje(errorJson);
            await DisplayAlert("No se pudo cambiar el rol", mensajeApi, "Entendido");
        }
    }

    private static string ExtraerMensaje(string? errorJson)
    {
        if (string.IsNullOrWhiteSpace(errorJson))
            return "Error desconocido. Verifica tu conexión.";
        try
        {
            using var doc = JsonDocument.Parse(errorJson);
            if (doc.RootElement.TryGetProperty("mensaje", out var msg))
                return msg.GetString() ?? errorJson;
        }
        catch { }
        return errorJson;
    }

    // Volver — compatible con TapGestureRecognizer
    private async void OnVolverClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}
