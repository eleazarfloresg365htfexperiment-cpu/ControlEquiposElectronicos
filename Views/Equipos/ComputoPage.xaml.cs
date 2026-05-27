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
        string opcion = await DisplayActionSheet(
            "Asignar periférico", "Cancelar", null,
            "Asignar periférico existente",
            "Registrar y asignar nuevo");

        if (opcion == null || opcion == "Cancelar") return;

        if (opcion == "Asignar periférico existente")
        {
            var perifericos = _viewModel.Equipos
                .Where(e => e.Tipo != "PC")
                .ToList();

            if (!perifericos.Any())
            {
                await DisplayAlert("Sin periféricos",
                    "No hay periféricos disponibles en el inventario.", "OK");
                return;
            }

            var opciones = perifericos.Select(p => $"{p.Codigo} - {p.Nombre}").ToArray();

            string seleccion = await DisplayActionSheet(
                "Selecciona un periférico", "Cancelar", null, opciones);

            if (seleccion == null || seleccion == "Cancelar") return;

            string pc = await DisplayActionSheet(
                "Selecciona la PC", "Cancelar", null,
                _viewModel.Equipos
                    .Where(e => e.Tipo == "PC")
                    .Select(p => $"{p.Codigo} - {p.Nombre}")
                    .ToArray());

            if (pc == null || pc == "Cancelar") return;

            await DisplayAlert("Asignado",
                $"Periférico {seleccion.Split('-')[0].Trim()} asignado a {pc.Split('-')[0].Trim()} correctamente.\n(Se conectará con la API cuando esté disponible)",
                "OK");
        }
        else if (opcion == "Registrar y asignar nuevo")
        {
            string tipo = await DisplayActionSheet(
                "Tipo de periférico", "Cancelar", null,
                "Mouse", "Teclado", "Monitor", "Bocina", "Cañonera");

            if (tipo != null && tipo != "Cancelar")
                await Shell.Current.GoToAsync("RegistrarEquipoPage");
        }
    }

    private async void OnEditarPCTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is int id)
            await Shell.Current.GoToAsync($"DetalleEquipoPage?equipoId={id}");
    }

    private async void OnCambiarEstadoPCTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not int id) return;

        string estado = await DisplayActionSheet(
            "Cambiar estado", "Cancelar", null,
            "Funcional",
            "No funcional",
            "En mantenimiento",
            "En bodega",
            "Dado de baja",
            "Operativo",
            "Administrativo",
            "Reasignado");

        if (estado == null || estado == "Cancelar") return;

        var equipo = _viewModel.Equipos.FirstOrDefault(eq => eq.Id == id);
        if (equipo == null) return;

        _viewModel.ActualizarEstadoVisual(id, estado);
        await DisplayAlert("Estado", $"Estado cambiado a {estado}", "OK");
        await _viewModel.CargarEquiposAsync();
    }

    private async void OnReclasificarPCTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Reclasificar", "Función de reclasificación próximamente.", "OK");
    }
}