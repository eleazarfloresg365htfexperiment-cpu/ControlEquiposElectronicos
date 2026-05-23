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

    private async void OnCerrarSesionClicked(object? sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Cerrar sesión",
            "¿Seguro que deseas cerrar sesión?", "Sí", "Cancelar");

        if (!confirmar)
            return;

        _sesion.CerrarSesion();
        Application.Current!.Windows[0].Page = new LoginPage(_sesion);
    }
}