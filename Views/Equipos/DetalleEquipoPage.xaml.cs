using ControlEquiposElectronicos.ViewModels.Equipos;

namespace ControlEquiposElectronicos.Views.Equipos;

[QueryProperty(nameof(EquipoId), "equipoId")]
public partial class DetalleEquipoPage : ContentPage
{
    private readonly DetalleEquiposViewModel _viewModel;

    private int _equipoId;
    public int EquipoId
    {
        get => _equipoId;
        set
        {
            _equipoId = value;
            _ = _viewModel.CargarEquipoAsync(value);
        }
    }

    public DetalleEquipoPage(DetalleEquiposViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        BtnVolver.Clicked += async (s, e) => await Shell.Current.GoToAsync("..");

        BtnEditar.Clicked += async (s, e) =>
        {
            if (_viewModel.Equipo == null) return;
            await Shell.Current.GoToAsync($"RegistrarEquipoPage?equipoId={_viewModel.Equipo.Id}");
        };

        BtnCambiarEstado.Clicked += async (s, e) =>
        {
            string accion = await DisplayActionSheet(
                "Cambiar estado", "Cancelar", null,
                "Funcional",
                "No funcional",
                "En mantenimiento",
                "En bodega",
                "Dado de baja",
                "Operativo",
                "Administrativo",
                "Reasignado");

            if (accion != null && accion != "Cancelar")
            {
                await _viewModel.CambiarEstadoAsync(accion);
                await DisplayAlert("Éxito", $"Estado cambiado a {accion}", "OK");
            }
        };

        BtnReclasificar.Clicked += async (s, e) =>
        {
            await DisplayAlert("Reclasificar", "Selecciona la nueva clasificación del equipo.", "OK");
        };

        BtnDesactivar.Clicked += async (s, e) =>
        {
            if (_viewModel.Equipo == null) return;
            bool confirmar = await DisplayAlert("Desactivar",
                $"¿Estás segura de desactivar {_viewModel.Equipo.Nombre}?", "Sí", "No");
            if (confirmar)
            {
                var resultado = await _viewModel.DesactivarEquipoAsync();
                if (resultado)
                {
                    await DisplayAlert("Éxito", "Equipo desactivado correctamente.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo desactivar el equipo.", "OK");
                }
            }
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}