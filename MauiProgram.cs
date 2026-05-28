using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels;
using ControlEquiposElectronicos.ViewModels.Consultas;
using ControlEquiposElectronicos.ViewModels.Mantenimientos;
using ControlEquiposElectronicos.ViewModels.Reportes;
using ControlEquiposElectronicos.Views.Consultas;
using ControlEquiposElectronicos.Views.Mantenimientos;
using ControlEquiposElectronicos.Views.Reportes;
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

            builder.Services.AddSingleton<IApiService, ApiService>();

            builder.Services.AddSingleton<IEquipoApiService, EquipoApiService>();
            builder.Services.AddSingleton<IChecklistApiService, ChecklistApiService>();
            builder.Services.AddSingleton<IMantenimientoApiService, MantenimientoApiService>();
            builder.Services.AddSingleton<IReporteFallaApiService, ReporteFallaApiService>();
            builder.Services.AddSingleton<IUsuarioApiService, UsuarioApiService>();
            builder.Services.AddSingleton<ICatalogoApiService, CatalogoApiService>();
            builder.Services.AddSingleton<IAuditoriaApiService, AuditoriaApiService>();
            builder.Services.AddSingleton<IExportService, ExportService>();

            builder.Services.AddTransient<MantenimientosViewModel>();
            builder.Services.AddTransient<MantenimientosPage>();
            builder.Services.AddTransient<MantenimientoFormularioViewModel>();
            builder.Services.AddTransient<MantenimientoFormularioPage>();

            builder.Services.AddTransient<ReportesViewModel>();
            builder.Services.AddTransient<ReportesPage>();
            builder.Services.AddTransient<ReporteFallaFormularioViewModel>();
            builder.Services.AddTransient<ReporteFallaFormularioPage>();

            builder.Services.AddTransient<ConsultasViewModel>();
            builder.Services.AddTransient<ConsultasPage>();

            return builder.Build();
        }
    }
}