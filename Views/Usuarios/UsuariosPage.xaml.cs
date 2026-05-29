using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Usuarios;

public partial class UsuariosPage : ContentPage
{
    private readonly UsuariosViewModel _viewModel;

    public UsuariosPage()
    {
        InitializeComponent();

        var usuarioApi = IPlatformApplication.Current!.Services
            .GetRequiredService<IUsuarioApiService>();

        _viewModel = new UsuariosViewModel(usuarioApi);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }
}
