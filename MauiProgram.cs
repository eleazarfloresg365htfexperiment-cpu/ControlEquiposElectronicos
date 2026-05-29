using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels.Checklist;
using ControlEquiposElectronicos.Views.Checklist;
using ControlEquiposElectronicos.Views.Equipos;
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
        builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IAuthService, ControlEquiposElectronicos.Services.AuthService>();
        builder.Services.AddTransient<AppShell>();

        // Servicio base de la API
        builder.Services.AddSingleton<IApiService, ApiService>();

        // Services por módulo
        builder.Services.AddSingleton<IEquipoApiService, EquipoApiService>();
        builder.Services.AddSingleton<IChecklistApiService, ChecklistApiService>();
        builder.Services.AddSingleton<IMantenimientoApiService, MantenimientoApiService>();
        builder.Services.AddSingleton<IReporteFallaApiService, ReporteFallaApiService>();
        builder.Services.AddSingleton<IUsuarioApiService, UsuarioApiService>();
        builder.Services.AddSingleton<ICatalogoApiService, CatalogoApiService>();
        builder.Services.AddSingleton<IAuditoriaApiService, AuditoriaApiService>();

        // ViewModels y Pages de Equipos
        builder.Services.AddTransient<ControlEquiposElectronicos.ViewModels.EquiposViewModel>();
        builder.Services.AddTransient<ControlEquiposElectronicos.ViewModels.Equipos.EquipoFormularioViewModel>();
        builder.Services.AddTransient<ControlEquiposElectronicos.ViewModels.Equipos.DetalleEquiposViewModel>();
        builder.Services.AddTransient<EquiposPage>();
        builder.Services.AddTransient<RegistrarEquipoPage>();
        builder.Services.AddTransient<DetalleEquipoPage>();
        builder.Services.AddTransient<ComputoPage>();
        builder.Services.AddTransient<AuditoriaPage>();

        // ViewModels del Checklist (de Danny)
        builder.Services.AddTransient<ChecklistViewModel>();
        builder.Services.AddTransient<NuevoChecklistViewModel>();
        builder.Services.AddTransient<RevisionEquipoChecklistViewModel>();
        builder.Services.AddTransient<PlantillasChecklistViewModel>();
        builder.Services.AddTransient<HistorialChecklistViewModel>();

        // Pages del Checklist (de Danny)
        builder.Services.AddTransient<ChecklistPage>();
        builder.Services.AddTransient<NuevoChecklistPage>();
        builder.Services.AddTransient<RevisionEquipoChecklistPage>();
        builder.Services.AddTransient<HistorialChecklistPage>();
        builder.Services.AddTransient<PlantillasChecklistPage>();

        var app = builder.Build();

        // Rutas del Checklist (de Danny)
        Routing.RegisterRoute(nameof(NuevoChecklistPage), typeof(NuevoChecklistPage));
        Routing.RegisterRoute(nameof(RevisionEquipoChecklistPage), typeof(RevisionEquipoChecklistPage));
        Routing.RegisterRoute(nameof(HistorialChecklistPage), typeof(HistorialChecklistPage));
        Routing.RegisterRoute(nameof(PlantillasChecklistPage), typeof(PlantillasChecklistPage));

        return app;
    }
}