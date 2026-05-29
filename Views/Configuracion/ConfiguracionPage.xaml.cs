using ControlEquiposElectronicos.Services;

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
        var rol = usuario?.Rol;
        bool esAdminOSuperior = rol == "Administrador" || rol == "OP";

        ContenidoConfig.IsVisible = esAdminOSuperior;
        AccesoRestringido.IsVisible = !esAdminOSuperior;

        if (esAdminOSuperior && usuario != null)
        {
            NombreLabel.Text = usuario.Nombre;
            RolLabel.Text = usuario.Rol;
            SeccionAuditoria.IsVisible = rol == "OP";
        }
    }

    private async void OnPermisosClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("PermisosRol");

    private async void OnRegistrarUsuarioClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("RegistroUsuario");

    private async void OnGestionarRolesUsuarioClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("RolesUsuario");

    private async void OnAuditoriaClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("Auditoria");
}
