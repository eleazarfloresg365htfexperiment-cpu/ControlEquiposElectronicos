using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Views.Login;

namespace ControlEquiposElectronicos.Views.Configuracion;

public partial class ConfiguracionPage : ContentPage
{
    private readonly SesionService _sesion;

    public ConfiguracionPage()
    {
        InitializeComponent();
        _sesion = IPlatformApplication.Current!.Services.GetRequiredService<SesionService>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        VerificarAcceso();
    }

    private void VerificarAcceso()
    {
        var usuario = _sesion.UsuarioActual;
        bool esAdministrador = usuario != null && usuario.Rol == "Administrador";

        // Solo los administradores ven el contenido; los demás ven el aviso
        ContenidoConfig.IsVisible = esAdministrador;
        AccesoRestringido.IsVisible = !esAdministrador;

        if (esAdministrador && usuario != null)
        {
            NombreLabel.Text = usuario.Nombre;
            RolLabel.Text = usuario.Rol;
        }
    }

    private async void OnPermisosClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("PermisosRol");
    }

    private async void OnRegistrarUsuarioClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("RegistroUsuario");
    }

    private async void OnGestionarRolesUsuarioClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("RolesUsuario");
    }
}
