using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Views.Equipos;
using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels.Checklist;
using ControlEquiposElectronicos.Views.Checklist;
using Microsoft.Extensions.Logging;

namespace ControlEquiposElectronicos
{
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
            builder.Services.AddSingleton<SesionService>();
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IApiService, ControlEquiposElectronicos.Services.ApiService>();
#endif
        builder.Logging.AddDebug();
#endif

        // Servicio base de la API
        builder.Services.AddSingleton<IApiService, ApiService>();

        // Sesión, autenticación y navegación
        builder.Services.AddSingleton<SesionService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddTransient<AppShell>();

        // Services por módulo
        builder.Services.AddSingleton<IEquipoApiService, EquipoApiService>();
        builder.Services.AddSingleton<IChecklistApiService, ChecklistApiService>();
        builder.Services.AddSingleton<IMantenimientoApiService, MantenimientoApiService>();
        builder.Services.AddSingleton<IReporteFallaApiService, ReporteFallaApiService>();
        builder.Services.AddSingleton<IUsuarioApiService, UsuarioApiService>();
        builder.Services.AddSingleton<ICatalogoApiService, CatalogoApiService>();
        builder.Services.AddSingleton<IAuditoriaApiService, AuditoriaApiService>();

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

            // Services por módulo
            builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IEquipoApiService, ControlEquiposElectronicos.Services.EquipoApiService>();
            builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IChecklistApiService, ControlEquiposElectronicos.Services.ChecklistApiService>();
            builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IMantenimientoApiService, ControlEquiposElectronicos.Services.MantenimientoApiService>();
            builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IReporteFallaApiService, ControlEquiposElectronicos.Services.ReporteFallaApiService>();
            builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IUsuarioApiService, ControlEquiposElectronicos.Services.UsuarioApiService>();
            builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.ICatalogoApiService, ControlEquiposElectronicos.Services.CatalogoApiService>();
            builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IAuditoriaApiService, ControlEquiposElectronicos.Services.AuditoriaApiService>();

            // ViewModels y Pages de Equipos
            builder.Services.AddTransient<ControlEquiposElectronicos.ViewModels.EquiposViewModel>();
            builder.Services.AddTransient<ControlEquiposElectronicos.ViewModels.Equipos.EquipoFormularioViewModel>();
            builder.Services.AddTransient<ControlEquiposElectronicos.ViewModels.Equipos.DetalleEquiposViewModel>();

            builder.Services.AddTransient<ControlEquiposElectronicos.Views.Equipos.EquiposPage>();
            builder.Services.AddTransient<ControlEquiposElectronicos.Views.Equipos.RegistrarEquipoPage>();
            builder.Services.AddTransient<ControlEquiposElectronicos.Views.Equipos.DetalleEquipoPage>();
            builder.Services.AddTransient<ControlEquiposElectronicos.Views.Equipos.ComputoPage>();
            builder.Services.AddTransient<AuditoriaPage>();

            return builder.Build();
        }
        var app = builder.Build();

        // Rutas del Checklist (de Danny)
        Routing.RegisterRoute(nameof(NuevoChecklistPage), typeof(NuevoChecklistPage));
        Routing.RegisterRoute(nameof(RevisionEquipoChecklistPage), typeof(RevisionEquipoChecklistPage));
        Routing.RegisterRoute(nameof(HistorialChecklistPage), typeof(HistorialChecklistPage));
        Routing.RegisterRoute(nameof(PlantillasChecklistPage), typeof(PlantillasChecklistPage));

        return app;
    }
}
}
