namespace ControlEquiposElectronicos.Views.Equipos;

public partial class ElectricidadPage : ContentPage
{
    public ElectricidadPage()
    {
        InitializeComponent();
    }

    private async void OnRegistrarUPS(object sender, EventArgs e)
    {
        await DisplayAlert(
            "UPS",
            "Registrar UPS funcionando",
            "OK");
    }

    private async void OnRegistrarBateria(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Batería",
            "Registrar Batería funcionando",
            "OK");
    }

    private async void OnRegistrarAire(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Aire Acondicionado",
            "Registrar Aire Acondicionado funcionando",
            "OK");
    }

    private async void OnRegistrarBTU(object sender, EventArgs e)
    {
        await DisplayAlert(
            "BTU",
            "Registrar BTU funcionando",
            "OK");
    }

    private async void OnVerServicios(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Servicios",
            "Ver Servicios funcionando",
            "OK");
    }
}