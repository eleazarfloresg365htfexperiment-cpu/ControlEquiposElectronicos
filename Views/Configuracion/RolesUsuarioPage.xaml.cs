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
            // Si la API falla, mostrar lista vacía sin crashear
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

        // Filtrar por nombre, nickname o rol
        var filtrados = _todosLosUsuarios.Where(u =>
            u.NombreCompleto.ToLower().Contains(texto) ||
            u.Nickname.ToLower().Contains(texto) ||
            u.Rol.ToLower().Contains(texto)
        ).ToList();

        UsuariosCollection.ItemsSource = filtrados;
    }

    private async void OnUsuarioTapped(object? sender, TappedEventArgs e)
    {
        // Obtener el usuario del binding
        if (sender is not BindableObject bindable) return;
        if (bindable.BindingContext is not UsuarioListadoDto usuario) return;

        var rolMostrar = usuario.Rol == "Tecnico" ? "Técnico" : usuario.Rol;
        var estado = usuario.Activo ? "Activo" : "Inactivo";

        await DisplayAlert(usuario.NombreCompleto,
            $"Usuario: @{usuario.Nickname}\n" +
            $"Rol actual: {rolMostrar}\n" +
            $"Estado: {estado}\n\n" +
            "Para cambiar el rol de este usuario, la API del sistema debe ofrecer un endpoint de actualización. Función pendiente.",
            "Entendido");
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
