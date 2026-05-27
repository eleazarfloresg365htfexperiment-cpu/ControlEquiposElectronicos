using ControlEquiposElectronicos.ViewModels.Equipos;

namespace ControlEquiposElectronicos.Views.Equipos;

[QueryProperty(nameof(EquipoId), "equipoId")]
public partial class RegistrarEquipoPage : ContentPage
{
    private readonly EquipoFormularioViewModel _viewModel;
    private int _equipoId;

    public int EquipoId
    {
        get => _equipoId;
        set
        {
            _equipoId = value;
            if (value > 0)
            {
                Title = "Editar equipo";
                _ = CargarDatosEdicion(value);
            }
        }
    }

    public RegistrarEquipoPage(EquipoFormularioViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;

        BtnGuardar.Clicked += async (s, e) =>
        {
            bool resultado;
            if (_equipoId > 0)
                resultado = await _viewModel.ActualizarEquipoAsync(_equipoId);
            else
                resultado = await _viewModel.GuardarEquiposAsync();

            if (resultado)
            {
                await DisplayAlert("Éxito",
                    _equipoId > 0 ? "Equipo actualizado correctamente" : "Equipo registrado correctamente",
                    "OK");
                await Shell.Current.GoToAsync("..");
            }
        };

        BtnLimpiar.Clicked += (s, e) => _viewModel.LimpiarFormulario();
        BtnCancelar.Clicked += async (s, e) => await Shell.Current.GoToAsync("..");
    }

    private async Task CargarDatosEdicion(int id)
    {
        await _viewModel.CargarCatalogosAsync();
        var equipo = await _viewModel.ObtenerEquipoPorIdAsync(id);
        if (equipo != null)
            _viewModel.CargarDatosParaEditar(equipo);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_equipoId == 0)
            await _viewModel.CargarCatalogosAsync();
    }
}