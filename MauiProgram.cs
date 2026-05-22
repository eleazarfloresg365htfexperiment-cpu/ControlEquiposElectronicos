using ControlEquiposElectronicos.Services;
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
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<SesionService>();
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IApiService, ControlEquiposElectronicos.Services.ApiService>();
#endif

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

            return builder.Build();
        }
    }
}