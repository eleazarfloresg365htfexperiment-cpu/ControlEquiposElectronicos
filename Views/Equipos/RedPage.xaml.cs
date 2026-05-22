namespace ControlEquiposElectronicos.Views.Equipos;

public partial class RedPage : ContentPage
{
    public RedPage()
    {
        InitializeComponent();
    }

    private async void OnRegistrarRouter(object sender, EventArgs e)
    {
        await Application.Current.MainPage!.DisplayAlert(
            "Router",
            "Registrar Router funcionando",
            "OK");
    }

    private async void OnRegistrarSwitch(object sender, EventArgs e)
    {
        await Application.Current.MainPage!.DisplayAlert(
            "Switch",
            "Registrar Switch funcionando",
            "OK");
    }

    private async void OnRegistrarPuerto(object sender, EventArgs e)
    {
        await Application.Current.MainPage!.DisplayAlert(
            "Puerto",
            "Registrar Puerto funcionando",
            "OK");
    }

    private async void OnVerHistorial(object sender, EventArgs e)
    {
        await Application.Current.MainPage!.DisplayAlert(
            "Historial",
            "Historial funcionando",
            "OK");
    }

    private async void OnReportarFalla(object sender, EventArgs e)
    {
        await Application.Current.MainPage!.DisplayAlert(
            "Falla",
            "Reporte funcionando",
            "OK");
    }
}