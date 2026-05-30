using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Configuracion;

public class PermisosRolViewModel : BaseViewModel
{
    private string _rolSeleccionado = "OP";
    public string RolSeleccionado
    {
        get => _rolSeleccionado;
        set
        {
            _rolSeleccionado = value;
            OnPropertyChanged();
            AplicarPlantillaRol();
        }
    }

    public IReadOnlyList<string> Roles { get; } = ["OP", "Administrador", "Técnico", "Consulta"];

    public bool SwDashboard { get; set; }
    public bool SwEquipos { get; set; }
    public bool SwMantenimientos { get; set; }
    public bool SwReportes { get; set; }
    public bool SwUsuarios { get; set; }
    public bool SwConfiguracion { get; set; }
    public bool SwChecklist { get; set; }
    public bool SwCrear { get; set; }
    public bool SwEditar { get; set; }
    public bool SwEliminar { get; set; }
    public bool SwGestionarPermisos { get; set; }

    public ICommand VolverCommand { get; }

    public PermisosRolViewModel()
    {
        Title = "Permisos por rol";
        VolverCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        AplicarPlantillaRol();
    }

    private void AplicarPlantillaRol()
    {
        switch (RolSeleccionado)
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
        SwDashboard = dashboard;
        SwEquipos = equipos;
        SwMantenimientos = mantenimientos;
        SwReportes = reportes;
        SwUsuarios = usuarios;
        SwConfiguracion = configuracion;
        SwChecklist = checklist;
        NotificarModulos();
    }

    private void EstablecerAcciones(bool crear, bool editar, bool eliminar, bool gestionarPermisos)
    {
        SwCrear = crear;
        SwEditar = editar;
        SwEliminar = eliminar;
        SwGestionarPermisos = gestionarPermisos;
        OnPropertyChanged(nameof(SwCrear));
        OnPropertyChanged(nameof(SwEditar));
        OnPropertyChanged(nameof(SwEliminar));
        OnPropertyChanged(nameof(SwGestionarPermisos));
    }

    private void NotificarModulos()
    {
        OnPropertyChanged(nameof(SwDashboard));
        OnPropertyChanged(nameof(SwEquipos));
        OnPropertyChanged(nameof(SwMantenimientos));
        OnPropertyChanged(nameof(SwReportes));
        OnPropertyChanged(nameof(SwUsuarios));
        OnPropertyChanged(nameof(SwConfiguracion));
        OnPropertyChanged(nameof(SwChecklist));
    }
}
