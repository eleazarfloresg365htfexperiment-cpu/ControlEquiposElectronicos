using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Login;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
