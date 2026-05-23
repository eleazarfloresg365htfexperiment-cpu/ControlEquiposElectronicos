namespace ControlEquiposElectronicos.Views.Configuracion;

public partial class PermisosRolPage : ContentPage
{
    public PermisosRolPage()
    {
        InitializeComponent();
        RolPicker.SelectedIndex = 0;
    }

    private void OnRolCambiado(object? sender, EventArgs e)
    {
        var rol = RolPicker.SelectedItem?.ToString();

        if (rol == "Administrador")
        {
            EstablecerModulos(true, true, true, true, true);
            EstablecerAcciones(true, true, true);
        }
        else if (rol == "Técnico")
        {
            EstablecerModulos(true, true, true, true, false);
            EstablecerAcciones(true, true, false);
        }
    }

    private void EstablecerModulos(bool dashboard, bool equipos, bool mantenimientos, bool reportes, bool usuarios)
    {
        SwDashboard.IsToggled = dashboard;
        SwEquipos.IsToggled = equipos;
        SwMantenimientos.IsToggled = mantenimientos;
        SwReportes.IsToggled = reportes;
        SwUsuarios.IsToggled = usuarios;
    }

    private void EstablecerAcciones(bool crear, bool editar, bool eliminar)
    {
        SwCrear.IsToggled = crear;
        SwEditar.IsToggled = editar;
        SwEliminar.IsToggled = eliminar;
    }

    private async void OnGuardarClicked(object? sender, EventArgs e)
    {
        var rol = RolPicker.SelectedItem?.ToString() ?? "rol";

        await DisplayAlert("Permisos guardados",
            $"Los permisos del rol \"{rol}\" se guardaron localmente. " +
            "La sincronización con el servidor se conectará cuando la API esté disponible.",
            "Entendido");
    }
}