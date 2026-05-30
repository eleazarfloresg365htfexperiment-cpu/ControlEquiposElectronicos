using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Configuracion;

public partial class ConfiguracionPage : ContentPage
{
    private readonly ConfiguracionViewModel _viewModel;

    public ConfiguracionPage(ConfiguracionViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Refrescar();
        ContenidoConfig.IsVisible = _viewModel.TieneAcceso;
        AccesoRestringido.IsVisible = !_viewModel.TieneAcceso;
        NombreLabel.Text = _viewModel.NombreUsuario;
        RolLabel.Text = _viewModel.RolUsuario;
        SeccionAuditoria.IsVisible = _viewModel.MostrarAuditoria;
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
