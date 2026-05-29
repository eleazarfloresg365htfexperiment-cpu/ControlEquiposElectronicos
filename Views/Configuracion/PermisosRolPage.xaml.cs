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

        switch (rol)
        {
            case "OP":
                EstablecerModulos(true, true, true, true, true, true, true);
                EstablecerAcciones(true, true, true, true);
                break;
            case "Administrador":
                EstablecerModulos(true, true, true, true, true, true, true);
                EstablecerAcciones(true, true, true, false);
                break;
            case "Técnico":
                EstablecerModulos(true, true, true, true, false, false, true);
                EstablecerAcciones(true, true, false, false);
                break;
            case "Consulta":
                EstablecerModulos(true, true, true, true, false, false, true);
                EstablecerAcciones(false, false, false, false);
                break;
        }
    }

    private void EstablecerModulos(bool dashboard, bool equipos, bool mantenimientos,
        bool reportes, bool usuarios, bool configuracion, bool checklist)
    {
        SwDashboard.IsToggled = dashboard;
        SwEquipos.IsToggled = equipos;
        SwMantenimientos.IsToggled = mantenimientos;
        SwReportes.IsToggled = reportes;
        SwUsuarios.IsToggled = usuarios;
        SwConfiguracion.IsToggled = configuracion;
        SwChecklist.IsToggled = checklist;
    }

    private void EstablecerAcciones(bool crear, bool editar, bool eliminar, bool gestionarPermisos)
    {
        SwCrear.IsToggled = crear;
        SwEditar.IsToggled = editar;
        SwEliminar.IsToggled = eliminar;
        SwGestionarPermisos.IsToggled = gestionarPermisos;
    }

    // Volver con Shell — compatible con Clicked y TapGestureRecognizer
    private async void OnVolverClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}
