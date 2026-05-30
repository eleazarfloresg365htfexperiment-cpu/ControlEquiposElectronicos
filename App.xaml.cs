using ControlEquiposElectronicos.Views.Login;

namespace ControlEquiposElectronicos;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var loginPage = IPlatformApplication.Current!.Services.GetRequiredService<LoginPage>();
        return new Window(loginPage);
    }
}
