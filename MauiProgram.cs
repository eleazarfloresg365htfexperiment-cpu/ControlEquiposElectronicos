using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels;
using ControlEquiposElectronicos.ViewModels.Checklist;
using ControlEquiposElectronicos.ViewModels.Configuracion;
using ControlEquiposElectronicos.ViewModels.Consultas;
using ControlEquiposElectronicos.ViewModels.Equipos;
using ControlEquiposElectronicos.ViewModels.Mantenimientos;
using ControlEquiposElectronicos.ViewModels.Reportes;
using ControlEquiposElectronicos.Views.Checklist;
using ControlEquiposElectronicos.Views.Configuracion;
using ControlEquiposElectronicos.Views.Consultas;
using ControlEquiposElectronicos.Views.Dashboard;
using ControlEquiposElectronicos.Views.Equipos;
using ControlEquiposElectronicos.Views.Login;
using ControlEquiposElectronicos.Views.Mantenimientos;
using ControlEquiposElectronicos.Views.Reportes;
using ControlEquiposElectronicos.Views.Usuarios;
using Microsoft.Extensions.Logging;

namespace ControlEquiposElectronicos;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-solid-900.ttf", "FontAwesome");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Sesión, autenticación y permisos
        builder.Services.AddSingleton<SesionService>();
        builder.Services.AddSingleton<IPermisoService, PermisoService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();

        // Servicio base de la API
        builder.Services.AddSingleton<IApiService, ApiService>();

        // Servicios por módulo
        builder.Services.AddSingleton<IEquipoApiService, EquipoApiService>();
        builder.Services.AddSingleton<IChecklistApiService, ChecklistApiService>();
        builder.Services.AddSingleton<IMantenimientoApiService, MantenimientoApiService>();
        builder.Services.AddSingleton<IReporteFallaApiService, ReporteFallaApiService>();
        builder.Services.AddSingleton<IUsuarioApiService, UsuarioApiService>();
        builder.Services.AddSingleton<ICatalogoApiService, CatalogoApiService>();
        builder.Services.AddSingleton<IAuditoriaApiService, AuditoriaApiService>();
        builder.Services.AddSingleton<IExportService, ExportService>();

        // ViewModels — Carlos
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<ConfiguracionViewModel>();
        builder.Services.AddTransient<PermisosRolViewModel>();
        builder.Services.AddTransient<UsuariosViewModel>();

        // ViewModels — Equipos (Suarlin + Kevin)
        builder.Services.AddTransient<EquiposViewModel>();
        builder.Services.AddTransient<EquipoFormularioViewModel>();
        builder.Services.AddTransient<DetalleEquiposViewModel>();
        builder.Services.AddTransient<RedViewModel>();
        builder.Services.AddTransient<ImpresionViewModel>();
        builder.Services.AddTransient<ElectricidadViewModel>();
        builder.Services.AddTransient<SoporteAmbientalViewModel>();

        // ViewModels — Checklist (Danny)
        builder.Services.AddTransient<ChecklistViewModel>();
        builder.Services.AddTransient<NuevoChecklistViewModel>();
        builder.Services.AddTransient<RevisionEquipoChecklistViewModel>();
        builder.Services.AddTransient<PlantillasChecklistViewModel>();
        builder.Services.AddTransient<HistorialChecklistViewModel>();

        // ViewModels — Mantenimientos y reportes (Pablo)
        builder.Services.AddTransient<MantenimientosViewModel>();
        builder.Services.AddTransient<MantenimientoFormularioViewModel>();
        builder.Services.AddTransient<ReportesViewModel>();
        builder.Services.AddTransient<ReporteFallaFormularioViewModel>();
        builder.Services.AddTransient<ConsultasViewModel>();

        // Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<ConfiguracionPage>();
        builder.Services.AddTransient<PermisosRolPage>();
        builder.Services.AddTransient<UsuariosPage>();
        builder.Services.AddTransient<EquiposPage>();
        builder.Services.AddTransient<RegistrarEquipoPage>();
        builder.Services.AddTransient<DetalleEquipoPage>();
        builder.Services.AddTransient<ComputoPage>();
        builder.Services.AddTransient<RedPage>();
        builder.Services.AddTransient<ImpresionPage>();
        builder.Services.AddTransient<ElectricidadPage>();
        builder.Services.AddTransient<SoporteAmbientalPage>();
        builder.Services.AddTransient<ChecklistPage>();
        builder.Services.AddTransient<NuevoChecklistPage>();
        builder.Services.AddTransient<RevisionEquipoChecklistPage>();
        builder.Services.AddTransient<HistorialChecklistPage>();
        builder.Services.AddTransient<PlantillasChecklistPage>();
        builder.Services.AddTransient<MantenimientosPage>();
        builder.Services.AddTransient<MantenimientoFormularioPage>();
        builder.Services.AddTransient<ReportesPage>();
        builder.Services.AddTransient<ReporteFallaFormularioPage>();
        builder.Services.AddTransient<ConsultasPage>();

        var app = builder.Build();

        Routing.RegisterRoute("RedPage", typeof(RedPage));
        Routing.RegisterRoute("ImpresionPage", typeof(ImpresionPage));
        Routing.RegisterRoute("ElectricidadPage", typeof(ElectricidadPage));
        Routing.RegisterRoute("SoporteAmbientalPage", typeof(SoporteAmbientalPage));
        Routing.RegisterRoute("RegistrarEquipoPage", typeof(RegistrarEquipoPage));
        Routing.RegisterRoute("DetalleEquipoPage", typeof(DetalleEquipoPage));
        Routing.RegisterRoute("ComputoPage", typeof(ComputoPage));
        Routing.RegisterRoute("ReporteFallaFormulario", typeof(ReporteFallaFormularioPage));
        Routing.RegisterRoute("ConsultasPage", typeof(ConsultasPage));

        Routing.RegisterRoute(nameof(NuevoChecklistPage), typeof(NuevoChecklistPage));
        Routing.RegisterRoute(nameof(RevisionEquipoChecklistPage), typeof(RevisionEquipoChecklistPage));
        Routing.RegisterRoute(nameof(HistorialChecklistPage), typeof(HistorialChecklistPage));
        Routing.RegisterRoute(nameof(PlantillasChecklistPage), typeof(PlantillasChecklistPage));

        return app;
    }
}
