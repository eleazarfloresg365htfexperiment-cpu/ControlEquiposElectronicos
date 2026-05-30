using ControlEquiposElectronicos.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ControlEquiposElectronicos;

public partial class AppShell : Shell
{
    private readonly SesionService _sesion;
    private bool _sidebarExpandido = true;
    private bool _animando = false;
    private Border? _btnActivo;

    public AppShell(SesionService sesion)
    {
        InitializeComponent();
        _sesion = sesion;

        Routing.RegisterRoute("PermisosRol", typeof(Views.Configuracion.PermisosRolPage));
        Routing.RegisterRoute("RegistroUsuario", typeof(Views.Login.RegistroUsuarioPage));
        Routing.RegisterRoute("RolesUsuario", typeof(Views.Configuracion.RolesUsuarioPage));
        Routing.RegisterRoute("PerfilUsuario", typeof(Views.Configuracion.PerfilUsuarioPage));
        Routing.RegisterRoute("Auditoria", typeof(Views.Configuracion.AuditoriaPage));
        Routing.RegisterRoute("RegistrarEquipoPage", typeof(Views.Equipos.RegistrarEquipoPage));
        Routing.RegisterRoute("DetalleEquipoPage", typeof(Views.Equipos.DetalleEquipoPage));
        Routing.RegisterRoute("ComputoPage", typeof(Views.Equipos.ComputoPage));
        Routing.RegisterRoute("AuditoriaPage", typeof(Views.Configuracion.AuditoriaPage));
        Routing.RegisterRoute("MantenimientoFormulario", typeof(Views.Mantenimientos.MantenimientoFormularioPage));
        Routing.RegisterRoute("ReporteFallaFormulario", typeof(Views.Reportes.ReporteFallaFormularioPage));

        MostrarUsuario();
        AplicarPermisos();
        ActualizarBotonActivo(BtnDashboard);
    }

    private async void OnDashboardTapped(object? s, TappedEventArgs e)
    { await GoToAsync("//Dashboard"); ActualizarBotonActivo(BtnDashboard); }

    private async void OnEquiposTapped(object? s, TappedEventArgs e)
    { await GoToAsync("//Equipos"); ActualizarBotonActivo(BtnEquipos); }

    private async void OnMantenimientosTapped(object? s, TappedEventArgs e)
    { await GoToAsync("//Mantenimientos"); ActualizarBotonActivo(BtnMantenimientos); }

    private async void OnChecklistTapped(object? s, TappedEventArgs e)
    { await GoToAsync("//Checklist"); ActualizarBotonActivo(BtnChecklist); }

    private async void OnReportesTapped(object? s, TappedEventArgs e)
    { await GoToAsync("//Reportes"); ActualizarBotonActivo(BtnReportes); }

    private async void OnConsultasTapped(object? s, TappedEventArgs e)
    { await GoToAsync("//Consultas"); ActualizarBotonActivo(BtnConsultas); }

    private async void OnUsuariosTapped(object? s, TappedEventArgs e)
    { await GoToAsync("//Usuarios"); ActualizarBotonActivo(BtnUsuarios); }

    private async void OnConfiguracionTapped(object? s, TappedEventArgs e)
    { await GoToAsync("//Configuracion"); ActualizarBotonActivo(BtnConfiguracion); }

    private void ActualizarBotonActivo(Border nuevo)
    {
        if (_btnActivo != null) _btnActivo.BackgroundColor = Colors.Transparent;
        nuevo.BackgroundColor = Color.FromArgb("#512BD4");
        _btnActivo = nuevo;
    }

    private async void OnToggleSidebar(object? s, TappedEventArgs e)
    {
        if (_animando) return;
        _animando = true;

        _sidebarExpandido = !_sidebarExpandido;

        if (_sidebarExpandido)
        {
            FlyoutWidth = 240;
            LabelPrincipal.Text = "PRINCIPAL";
            LabelGestion.Text = "GESTIÓN";
            LabelSistema.Text = "SISTEMA";
            LogoCPC.IsVisible = true;
            NombreEmpresaStack.IsVisible = true;
            NombreUsuarioStack.IsVisible = true;
            SetOpacity(0);
            MostrarTextos(true);

            await Task.WhenAll(
                NombreEmpresaStack.FadeTo(1, 200, Easing.CubicOut),
                NombreUsuarioStack.FadeTo(1, 200, Easing.CubicOut),
                LblDashboard.FadeTo(1, 200, Easing.CubicOut),
                LblEquipos.FadeTo(1, 200, Easing.CubicOut),
                LblMantenimientos.FadeTo(1, 200, Easing.CubicOut),
                LblChecklist.FadeTo(1, 200, Easing.CubicOut),
                LblReportes.FadeTo(1, 200, Easing.CubicOut),
                LblConsultas.FadeTo(1, 200, Easing.CubicOut),
                LblUsuarios.FadeTo(1, 200, Easing.CubicOut),
                LblConfiguracion.FadeTo(1, 200, Easing.CubicOut),
                FlechaMenuLabel.FadeTo(1, 200, Easing.CubicOut)
            );
        }
        else
        {
            await Task.WhenAll(
                NombreEmpresaStack.FadeTo(0, 150, Easing.CubicIn),
                NombreUsuarioStack.FadeTo(0, 150, Easing.CubicIn),
                LblDashboard.FadeTo(0, 150, Easing.CubicIn),
                LblEquipos.FadeTo(0, 150, Easing.CubicIn),
                LblMantenimientos.FadeTo(0, 150, Easing.CubicIn),
                LblChecklist.FadeTo(0, 150, Easing.CubicIn),
                LblReportes.FadeTo(0, 150, Easing.CubicIn),
                LblConsultas.FadeTo(0, 150, Easing.CubicIn),
                LblUsuarios.FadeTo(0, 150, Easing.CubicIn),
                LblConfiguracion.FadeTo(0, 150, Easing.CubicIn),
                FlechaMenuLabel.FadeTo(0, 150, Easing.CubicIn)
            );

            MostrarTextos(false);
            NombreEmpresaStack.IsVisible = false;
            NombreUsuarioStack.IsVisible = false;
            LogoCPC.IsVisible = false;
            LabelPrincipal.Text = "P";
            LabelGestion.Text = "G";
            LabelSistema.Text = "S";
            FlyoutWidth = 54;
        }

        _animando = false;
    }

    private void SetOpacity(double v)
    {
        NombreEmpresaStack.Opacity = NombreUsuarioStack.Opacity = v;
        LblDashboard.Opacity = LblEquipos.Opacity = LblMantenimientos.Opacity = v;
        LblChecklist.Opacity = LblReportes.Opacity = LblConsultas.Opacity = LblUsuarios.Opacity = v;
        LblConfiguracion.Opacity = FlechaMenuLabel.Opacity = v;
    }

    private void MostrarTextos(bool visible)
    {
        LblDashboard.IsVisible = LblEquipos.IsVisible = LblMantenimientos.IsVisible = visible;
        LblChecklist.IsVisible = LblReportes.IsVisible = LblConsultas.IsVisible = LblUsuarios.IsVisible = visible;
        LblConfiguracion.IsVisible = FlechaMenuLabel.IsVisible = visible;
    }

    private void OnMenuUsuarioTapped(object? s, TappedEventArgs e)
    {
        MenuUsuarioDesplegable.IsVisible = !MenuUsuarioDesplegable.IsVisible;
        FlechaMenuLabel.Text = MenuUsuarioDesplegable.IsVisible ? "\u25B4" : "\u25BE";
    }

    private async void OnMiPerfilTapped(object? s, TappedEventArgs e)
    {
        MenuUsuarioDesplegable.IsVisible = false;
        FlechaMenuLabel.Text = "\u25BE";
        await Shell.Current.GoToAsync("PerfilUsuario");
    }

    private async void OnCerrarSesionTapped(object? s, TappedEventArgs e)
    {
        MenuUsuarioDesplegable.IsVisible = false;
        FlechaMenuLabel.Text = "\u25BE";
        bool ok = await DisplayAlert("Cerrar sesión",
            "¿Seguro que deseas cerrar sesión?", "Sí", "Cancelar");
        if (!ok) return;
        _sesion.CerrarSesion();
        Application.Current!.Windows[0].Page =
            IPlatformApplication.Current!.Services.GetRequiredService<Views.Login.LoginPage>();
    }

    private void MostrarUsuario()
    {
        var u = _sesion.UsuarioActual;
        if (u == null) return;
        NombreUsuarioLabel.Text = u.Nombre;
        RolUsuarioLabel.Text = u.Rol;
        InicialesLabel.Text = Iniciales(u.Nombre);
    }

    private void AplicarPermisos()
    {
        BtnEquipos.IsVisible = _sesion.TienePermiso("Equipos.Ver");
        BtnMantenimientos.IsVisible = _sesion.TienePermiso("Mantenimientos.Ver");
        BtnChecklist.IsVisible = _sesion.TienePermiso("Checklist.Ver");
        BtnReportes.IsVisible = _sesion.TienePermiso("Reportes.Ver");
        BtnConsultas.IsVisible = _sesion.TienePermiso("Equipos.Ver")
            || _sesion.TienePermiso("Reportes.Ver")
            || _sesion.TienePermiso("Mantenimientos.Ver");
        BtnUsuarios.IsVisible = _sesion.TienePermiso("Usuarios.Ver");
        BtnConfiguracion.IsVisible = _sesion.TienePermiso("Configuracion.Ver");
    }

    private static string Iniciales(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return "??";
        var p = nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return p.Length == 1 ? p[0][..Math.Min(2, p[0].Length)].ToUpper()
                             : $"{p[0][0]}{p[1][0]}".ToUpper();
    }
}
