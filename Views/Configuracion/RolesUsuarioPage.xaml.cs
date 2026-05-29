using ControlEquiposElectronicos.DTOs.Usuarios;
using ControlEquiposElectronicos.Services.Interfaces;

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
        catch (Exception)
        {
            _todosLosUsuarios = new List<UsuarioListadoDto>();
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

        if (string.IsNullOrEmpty(texto))
        {
            UsuariosCollection.ItemsSource = _todosLosUsuarios;
            return;
        }

        var filtrados = _todosLosUsuarios.Where(u =>
            u.NombreCompleto.ToLower().Contains(texto) ||
            u.Nickname.ToLower().Contains(texto) ||
            u.Rol.ToLower().Contains(texto)
        ).ToList();

        UsuariosCollection.ItemsSource = filtrados;
    }

    // Tapping the card still shows a summary (informational, non-destructive)
    private async void OnUsuarioTapped(object? sender, TappedEventArgs e)
    {
        if (sender is not BindableObject bindable) return;
        if (bindable.BindingContext is not UsuarioListadoDto usuario) return;

        var estado = usuario.Activo ? "Activo" : "Inactivo";
        await DisplayAlert(usuario.NombreCompleto,
            $"Usuario: @{usuario.Nickname}\nRol actual: {usuario.Rol}\nEstado: {estado}\n\nUsa los botones para cambiar el rol.",
            "Cerrar");
    }

    // ── Botones de cambio de rol ──────────────────────────────────────────────

    private async void OnAsignarAdministradorClicked(object? sender, EventArgs e)
        => await CambiarRol(sender, "Administrador");

    private async void OnAsignarTecnicoClicked(object? sender, EventArgs e)
        => await CambiarRol(sender, "Tecnico");

    private async void OnAsignarConsultaClicked(object? sender, EventArgs e)
        => await CambiarRol(sender, "Consulta");

    // ── Lógica central ───────────────────────────────────────────────────────

    private async Task CambiarRol(object? sender, string nuevoRol)
    {
        // Obtener el usuario desde el CommandParameter del botón
        if (sender is not Button boton) return;
        if (boton.BindingContext is not UsuarioListadoDto usuario) return;

        // No hacer nada si ya tiene ese rol
        if (usuario.Rol.Equals(nuevoRol, StringComparison.OrdinalIgnoreCase))
        {
            await DisplayAlert("Sin cambios",
                $"{usuario.NombreCompleto} ya tiene el rol «{nuevoRol}».", "Entendido");
            return;
        }

        // Confirmar antes de aplicar
        var rolMostrar = nuevoRol == "Tecnico" ? "Técnico" : nuevoRol;
        bool confirmar = await DisplayAlert(
            "Cambiar rol",
            $"¿Cambiar el rol de {usuario.NombreCompleto} a «{rolMostrar}»?",
            "Sí, cambiar", "Cancelar");

        if (!confirmar) return;

        // Llamar a la API
        bool exito = await _usuarioApi.CambiarRolAsync(usuario.UsuarioId, nuevoRol);

        if (exito)
        {
            await DisplayAlert("Rol actualizado",
                $"El rol de {usuario.NombreCompleto} ahora es «{rolMostrar}».", "Aceptar");

            // Recargar la lista para reflejar el cambio
            await CargarUsuariosAsync();

            // Restaurar el filtro de búsqueda si había texto
            var textoBusqueda = BusquedaEntry.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                var filtrados = _todosLosUsuarios.Where(u =>
                    u.NombreCompleto.ToLower().Contains(textoBusqueda.ToLower()) ||
                    u.Nickname.ToLower().Contains(textoBusqueda.ToLower()) ||
                    u.Rol.ToLower().Contains(textoBusqueda.ToLower())
                ).ToList();
                UsuariosCollection.ItemsSource = filtrados;
            }
        }
        else
        {
            await DisplayAlert("Error",
                "No se pudo cambiar el rol. Verifica tu conexión e intenta de nuevo.", "Entendido");
        }
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
