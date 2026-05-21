namespace ControlEquiposElectronicos.Views.Equipos;

public partial class SoporteAmbientalPage : ContentPage
{
    public SoporteAmbientalPage()
    {
        InitializeComponent();
    }

    private async void OnRegistrarAmbientador(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Ambientador",
            "Registrar Ambientador funcionando",
            "OK");
    }

    private async void OnRegistrarSoporte(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Soporte",
            "Registrar Soporte funcionando",
            "OK");
    }

    private async void OnEditarDetalle(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Detalle",
            "Editar detalle funcionando",
            "OK");
    }

    private async void OnReportarProblema(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Problema",
            "Reporte funcionando",
            "OK");
    }
}