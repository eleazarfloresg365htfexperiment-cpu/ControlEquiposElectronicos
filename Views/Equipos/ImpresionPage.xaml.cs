namespace ControlEquiposElectronicos.Views.Equipos;

using Microsoft.Maui.Controls;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class ImpresionPage : ContentPage
{
    public ImpresionPage()
    {
        InitializeComponent();
    }

    private async void OnRegistrarImpresora(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Impresora",
            "Registrar Impresora funcionando",
            "OK");
    }

    private async void OnRegistrarCartucho(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Cartucho",
            "Registrar Cartucho funcionando",
            "OK");
    }

    private async void OnEditarDetalle(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Detalle",
            "Editar detalle funcionando",
            "OK");
    }

    private async void OnVerHistorial(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Historial",
            "Historial funcionando",
            "OK");
    }
}