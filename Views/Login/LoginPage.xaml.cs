using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Login;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void UsuarioEntry_Completed(object sender, EventArgs e)
    {
        ContrasenaEntry.Focus();
    }
}