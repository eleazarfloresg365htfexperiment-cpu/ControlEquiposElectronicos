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
#endif


            // Services base de consumo API
            builder.Services.AddSingleton<
        ControlEquiposElectronicos.Services.Interfaces.IEquipoApiService,
        ControlEquiposElectronicos.Services.EquipoApiService>();

            // Services por módulo
            builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IEquipoApiService, ControlEquiposElectronicos.Services.EquipoApiService>();
        builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IChecklistApiService, ControlEquiposElectronicos.Services.ChecklistApiService>();
        builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IMantenimientoApiService, ControlEquiposElectronicos.Services.MantenimientoApiService>();
        builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IReporteFallaApiService, ControlEquiposElectronicos.Services.ReporteFallaApiService>();
        builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IUsuarioApiService, ControlEquiposElectronicos.Services.UsuarioApiService>();
        builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.ICatalogoApiService, ControlEquiposElectronicos.Services.CatalogoApiService>();
        builder.Services.AddSingleton<ControlEquiposElectronicos.Services.Interfaces.IAuditoriaApiService, ControlEquiposElectronicos.Services.AuditoriaApiService>();
        return builder.Build();
        }
    }
}

