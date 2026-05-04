using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Views.Login;

namespace ControlEquiposElectronicos;

public partial class App : Application
{
    private readonly SesionService _sesionService;

    public App(SesionService sesionService)
    {
        InitializeComponent();
        _sesionService = sesionService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new LoginPage(_sesionService));
    }
}