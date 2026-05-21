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
        await Application.Current.MainPage!.DisplayAlert(
            "Impresora",
            "Registrar Impresora funcionando",
            "OK");
    }

    private async void OnRegistrarCartucho(object sender, EventArgs e)
    {
        await Application.Current.MainPage!.DisplayAlert(
            "Cartucho",
            "Registrar Cartucho funcionando",
            "OK");
    }

    private async void OnEditarDetalle(object sender, EventArgs e)
    {
        await Application.Current.MainPage!.DisplayAlert(
            "Detalle",
            "Editar detalle funcionando",
            "OK");
    }

    private async void OnVerHistorial(object sender, EventArgs e)
    {
        await Application.Current.MainPage!.DisplayAlert(
            "Historial",
            "Historial funcionando",
            "OK");
    }
}