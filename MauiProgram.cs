using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels;
using ControlEquiposElectronicos.ViewModels.Checklist;
using ControlEquiposElectronicos.ViewModels.Consultas;
using ControlEquiposElectronicos.ViewModels.Mantenimientos;
using ControlEquiposElectronicos.ViewModels.Reportes;
using ControlEquiposElectronicos.Views.Checklist;
using ControlEquiposElectronicos.Views.Consultas;
using ControlEquiposElectronicos.Views.Equipos;
using ControlEquiposElectronicos.Views.Mantenimientos;
using ControlEquiposElectronicos.Views.Reportes;
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

        // Sesión y autenticación
        builder.Services.AddSingleton<SesionService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddTransient<AppShell>();

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
        builder.Services.AddSingleton<IExportService, ExportService>(); // ← fix Reportes

        // ViewModels y Pages — Equipos (suarlin)
        builder.Services.AddTransient<EquiposViewModel>();
        builder.Services.AddTransient<ControlEquiposElectronicos.ViewModels.Equipos.EquipoFormularioViewModel>();
        builder.Services.AddTransient<ControlEquiposElectronicos.ViewModels.Equipos.DetalleEquiposViewModel>();
        builder.Services.AddTransient<EquiposPage>();
        builder.Services.AddTransient<RegistrarEquipoPage>();
        builder.Services.AddTransient<DetalleEquipoPage>();
        builder.Services.AddTransient<ComputoPage>();
        builder.Services.AddTransient<AuditoriaPage>();

        // ViewModels y Pages — Checklist (danny)
        builder.Services.AddTransient<ChecklistViewModel>();
        builder.Services.AddTransient<NuevoChecklistViewModel>();
        builder.Services.AddTransient<RevisionEquipoChecklistViewModel>();
        builder.Services.AddTransient<PlantillasChecklistViewModel>();
        builder.Services.AddTransient<HistorialChecklistViewModel>();
        builder.Services.AddTransient<ChecklistPage>();
        builder.Services.AddTransient<NuevoChecklistPage>();
        builder.Services.AddTransient<RevisionEquipoChecklistPage>();
        builder.Services.AddTransient<HistorialChecklistPage>();
        builder.Services.AddTransient<PlantillasChecklistPage>();

        // ViewModels y Pages — Mantenimientos
        builder.Services.AddTransient<MantenimientosViewModel>();
        builder.Services.AddTransient<MantenimientosPage>();
        builder.Services.AddTransient<MantenimientoFormularioViewModel>();
        builder.Services.AddTransient<MantenimientoFormularioPage>();

        // ViewModels y Pages — Reportes
        builder.Services.AddTransient<ReportesViewModel>();
        builder.Services.AddTransient<ReportesPage>();
        builder.Services.AddTransient<ReporteFallaFormularioViewModel>();
        builder.Services.AddTransient<ReporteFallaFormularioPage>();

        // ViewModels y Pages — Consultas
        builder.Services.AddTransient<ConsultasViewModel>();
        builder.Services.AddTransient<ConsultasPage>();

        var app = builder.Build();

        // Rutas de subtabs de Equipos (Kevin)
        Routing.RegisterRoute("RedPage", typeof(ControlEquiposElectronicos.Views.Equipos.RedPage));
        Routing.RegisterRoute("ImpresionPage", typeof(ControlEquiposElectronicos.Views.Equipos.ImpresionPage));
        Routing.RegisterRoute("ElectricidadPage", typeof(ControlEquiposElectronicos.Views.Equipos.ElectricidadPage));
        Routing.RegisterRoute("SoporteAmbientalPage", typeof(ControlEquiposElectronicos.Views.Equipos.SoporteAmbientalPage));

        // Rutas del Checklist (danny)
        Routing.RegisterRoute(nameof(NuevoChecklistPage), typeof(NuevoChecklistPage));
        Routing.RegisterRoute(nameof(RevisionEquipoChecklistPage), typeof(RevisionEquipoChecklistPage));
        Routing.RegisterRoute(nameof(HistorialChecklistPage), typeof(HistorialChecklistPage));
        Routing.RegisterRoute(nameof(PlantillasChecklistPage), typeof(PlantillasChecklistPage));

        return app;
    }
}
