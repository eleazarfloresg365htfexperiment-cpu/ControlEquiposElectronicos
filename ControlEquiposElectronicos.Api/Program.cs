using Microsoft.EntityFrameworkCore;
using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.Services;
using ControlEquiposElectronicos.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AzureSqlConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }));

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Servicios
builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<IDetalleComputadoraService, DetalleComputadoraService>();
builder.Services.AddScoped<IDetalleImpresoraService, DetalleImpresoraService>();
builder.Services.AddScoped<IDetalleRedService, DetalleRedService>();
builder.Services.AddScoped<IDetalleUPSService, DetalleUPSService>();
builder.Services.AddScoped<IDetalleAmbientalService, DetalleAmbientalService>();
builder.Services.AddScoped<ISwitchPuertoService, SwitchPuertoService>();
builder.Services.AddScoped<IReporteFallaService, ReporteFallaService>();
builder.Services.AddScoped<IMantenimientoService, MantenimientoService>();
builder.Services.AddScoped<IReclasificacionEquipoService, ReclasificacionEquipoService>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioActualService, UsuarioActualService>();

var app = builder.Build();

// Ejecutar seed inicial
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();