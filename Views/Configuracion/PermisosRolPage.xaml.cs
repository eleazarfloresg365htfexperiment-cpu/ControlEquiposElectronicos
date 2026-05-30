using ControlEquiposElectronicos.ViewModels.Configuracion;

namespace ControlEquiposElectronicos.Views.Configuracion;

public partial class PermisosRolPage : ContentPage
{
    private readonly PermisosRolViewModel _viewModel;

    public PermisosRolPage(PermisosRolViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        RolPicker.SelectedIndex = 0;
        SincronizarToggles();
    }

    private void OnRolCambiado(object? sender, EventArgs e)
    {
        if (RolPicker.SelectedItem is string rol)
        {
            _viewModel.RolSeleccionado = rol;
            SincronizarToggles();
        }
    }

    private void SincronizarToggles()
    {
        SwDashboard.IsToggled = _viewModel.SwDashboard;
        SwEquipos.IsToggled = _viewModel.SwEquipos;
        SwMantenimientos.IsToggled = _viewModel.SwMantenimientos;
        SwReportes.IsToggled = _viewModel.SwReportes;
        SwUsuarios.IsToggled = _viewModel.SwUsuarios;
        SwConfiguracion.IsToggled = _viewModel.SwConfiguracion;
        SwChecklist.IsToggled = _viewModel.SwChecklist;
        SwCrear.IsToggled = _viewModel.SwCrear;
        SwEditar.IsToggled = _viewModel.SwEditar;
        SwEliminar.IsToggled = _viewModel.SwEliminar;
        SwGestionarPermisos.IsToggled = _viewModel.SwGestionarPermisos;
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}
