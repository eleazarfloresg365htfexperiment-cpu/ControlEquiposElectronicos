using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class ComputoPage : ContentPage
{
    private readonly EquiposViewModel _viewModel;

    public ComputoPage(EquiposViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        BtnRegistrarPC.Clicked += async (s, e) =>
            await Shell.Current.GoToAsync("RegistrarEquipoPage");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarEquiposAsync();
    }

    private async void OnRegistrarPCTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("RegistrarEquipoPage");
    }

    private async void OnRegistrarPeifericoTapped(object sender, TappedEventArgs e)
    {
        string tipo = await DisplayActionSheet(
            "Tipo de periférico", "Cancelar", null,
            "Mouse", "Teclado", "Monitor", "Bocina", "Cañonera");

        if (tipo != null && tipo != "Cancelar")
            await Shell.Current.GoToAsync("RegistrarEquipoPage");
    }

    private async void OnAsignarPeifericoTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Asignar periférico",
            "Selecciona una PC de la lista para asignarle un periférico.", "OK");
    }

    private async void OnEditarPCTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is int id)
            await Shell.Current.GoToAsync($"DetalleEquipoPage?equipoId={id}");
    }

    private async void OnCambiarEstadoPCTapped(object sender, TappedEventArgs e)
    {
        string estado = await DisplayActionSheet(
            "Cambiar estado", "Cancelar", null,
            "Activo", "En mantenimiento", "Dañado", "Inactivo");

        if (estado != null && estado != "Cancelar")
            await DisplayAlert("Estado", $"Estado cambiado a {estado}", "OK");
    }

    private async void OnReclasificarPCTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Reclasificar", "Función de reclasificación próximamente.", "OK");
    }
}