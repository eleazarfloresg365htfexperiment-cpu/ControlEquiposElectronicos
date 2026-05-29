using Microsoft.EntityFrameworkCore;
using ControlEquiposElectronicos.Api.Entities.Seguridad;
using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Entities.Reportes;
using ControlEquiposElectronicos.Api.Entities.Mantenimientos;
using ControlEquiposElectronicos.Api.Entities.Auditoria;
using ControlEquiposElectronicos.Api.Entities.Checklist;

namespace ControlEquiposElectronicos.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Checklist técnico
    public DbSet<PlantillaChecklist> PlantillasChecklist => Set<PlantillaChecklist>();
    public DbSet<PlantillaChecklistItem> PlantillaChecklistItems => Set<PlantillaChecklistItem>();
    public DbSet<ChecklistTecnico> ChecklistTecnicos => Set<ChecklistTecnico>();
    public DbSet<ChecklistTecnicoEquipo> ChecklistTecnicoEquipos => Set<ChecklistTecnicoEquipo>();
    public DbSet<ChecklistTecnicoDetalle> ChecklistTecnicoDetalles => Set<ChecklistTecnicoDetalle>();

    // SEGURIDAD
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<RolPermiso> RolPermisos => Set<RolPermiso>();

    // INVENTARIO
    public DbSet<Ubicacion> Ubicaciones => Set<Ubicacion>();
    public DbSet<CategoriaEquipo> CategoriasEquipo => Set<CategoriaEquipo>();
    public DbSet<TipoEquipo> TiposEquipo => Set<TipoEquipo>();
    public DbSet<EstadoEquipo> EstadosEquipo => Set<EstadoEquipo>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<EquipoPeriferico> EquipoPerifericos => Set<EquipoPeriferico>();

    // DETALLES
    public DbSet<DetalleComputadora> DetallesComputadora => Set<DetalleComputadora>();
    public DbSet<DetalleImpresora> DetallesImpresora => Set<DetalleImpresora>();
    public DbSet<DetalleRed> DetallesRed => Set<DetalleRed>();
    public DbSet<DetalleUPS> DetallesUPS => Set<DetalleUPS>();
    public DbSet<DetalleAmbiental> DetallesAmbiental => Set<DetalleAmbiental>();

    // RED
    public DbSet<SwitchPuerto> SwitchPuertos => Set<SwitchPuerto>();

    // REPORTES
    public DbSet<ReporteFalla> ReportesFalla => Set<ReporteFalla>();
    public DbSet<EstadoReporte> EstadosReporte => Set<EstadoReporte>();
    public DbSet<ReclasificacionEquipo> ReclasificacionesEquipo => Set<ReclasificacionEquipo>();

    // MANTENIMIENTOS
    public DbSet<Mantenimiento> Mantenimientos => Set<Mantenimiento>();
    public DbSet<TipoMantenimiento> TiposMantenimiento => Set<TipoMantenimiento>();
    public DbSet<MantenimientoRepuesto> MantenimientoRepuestos => Set<MantenimientoRepuesto>();

    // AUDITORÍA
    public DbSet<HistorialOperacion> HistorialOperaciones => Set<HistorialOperacion>();
    public DbSet<HistorialEstadoEquipo> HistorialEstadosEquipo => Set<HistorialEstadoEquipo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // SEGURIDAD
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.NombreUsuario)
            .IsUnique();

        modelBuilder.Entity<Rol>()
            .HasIndex(r => r.NombreRol)
            .IsUnique();

        modelBuilder.Entity<Permiso>()
            .HasIndex(p => new { p.Modulo, p.Accion })
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.RolId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Rol)
            .WithMany(r => r.RolPermisos)
            .HasForeignKey(rp => rp.RolId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Permiso)
            .WithMany(p => p.RolPermisos)
            .HasForeignKey(rp => rp.PermisoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RolPermiso>()
            .HasIndex(rp => new { rp.RolId, rp.PermisoId })
            .IsUnique();

        // EQUIPOS
        modelBuilder.Entity<Equipo>()
            .HasOne(e => e.CategoriaEquipo)
            .WithMany(c => c.Equipos)
            .HasForeignKey(e => e.CategoriaEquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Equipo>()
            .HasOne(e => e.TipoEquipo)
            .WithMany(t => t.Equipos)
            .HasForeignKey(e => e.TipoEquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Equipo>()
            .HasOne(e => e.EstadoEquipo)
            .WithMany(e => e.Equipos)
            .HasForeignKey(e => e.EstadoEquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Equipo>()
            .HasOne(e => e.Ubicacion)
            .WithMany(u => u.Equipos)
            .HasForeignKey(e => e.UbicacionId)
            .OnDelete(DeleteBehavior.Restrict);

        // PERIFERICOS

        modelBuilder.Entity<EquipoPeriferico>(entity =>
        {
            entity.ToTable("EquipoPerifericos");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Observaciones)
                .HasMaxLength(300);

            entity.Property(e => e.FechaAsignacion)
                .IsRequired();

            entity.Property(e => e.Activo)
                .HasDefaultValue(true);

            entity.HasOne(e => e.EquipoPrincipal)
                .WithMany()
                .HasForeignKey(e => e.EquipoPrincipalId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Periferico)
                .WithMany()
                .HasForeignKey(e => e.PerifericoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.EquipoPrincipalId, e.PerifericoId, e.Activo })
                .HasDatabaseName("IX_EquipoPerifericos_EquipoPrincipal_Periferico_Activo");
        });

        // DETALLE RED
        modelBuilder.Entity<DetalleRed>()
            .HasOne(d => d.Equipo)
            .WithOne(e => e.DetalleRed)
            .HasForeignKey<DetalleRed>(d => d.EquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetalleRed>()
            .HasOne(d => d.EquipoProveedor)
            .WithMany()
            .HasForeignKey(d => d.EquipoProveedorId)
            .OnDelete(DeleteBehavior.Restrict);

        // SWITCH PUERTOS
        modelBuilder.Entity<SwitchPuerto>()
            .HasOne(p => p.SwitchEquipo)
            .WithMany()
            .HasForeignKey(p => p.SwitchEquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SwitchPuerto>()
            .HasOne(p => p.EquipoConectado)
            .WithMany()
            .HasForeignKey(p => p.EquipoConectadoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SwitchPuerto>()
            .HasOne(p => p.UbicacionDestino)
            .WithMany()
            .HasForeignKey(p => p.UbicacionDestinoId)
            .OnDelete(DeleteBehavior.Restrict);

        // REPORTES
        modelBuilder.Entity<ReporteFalla>()
            .HasOne(r => r.Equipo)
            .WithMany()
            .HasForeignKey(r => r.EquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReporteFalla>()
            .HasOne(r => r.UsuarioReporta)
            .WithMany(u => u.ReportesRealizados)
            .HasForeignKey(r => r.UsuarioReportaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReporteFalla>()
            .HasOne(r => r.EstadoReporte)
            .WithMany(e => e.ReportesFalla)
            .HasForeignKey(r => r.EstadoReporteId)
            .OnDelete(DeleteBehavior.Restrict);

        // RECLASIFICACIONES
        modelBuilder.Entity<ReclasificacionEquipo>()
            .HasOne(r => r.Equipo)
            .WithMany()
            .HasForeignKey(r => r.EquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReclasificacionEquipo>()
            .HasOne(r => r.Usuario)
            .WithMany(u => u.ReclasificacionesRealizadas)
            .HasForeignKey(r => r.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReclasificacionEquipo>()
            .HasOne(r => r.EstadoAnterior)
            .WithMany()
            .HasForeignKey(r => r.EstadoAnteriorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReclasificacionEquipo>()
            .HasOne(r => r.EstadoNuevo)
            .WithMany()
            .HasForeignKey(r => r.EstadoNuevoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReclasificacionEquipo>()
            .HasOne(r => r.UbicacionAnterior)
            .WithMany()
            .HasForeignKey(r => r.UbicacionAnteriorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReclasificacionEquipo>()
            .HasOne(r => r.UbicacionNueva)
            .WithMany()
            .HasForeignKey(r => r.UbicacionNuevaId)
            .OnDelete(DeleteBehavior.Restrict);

        // MANTENIMIENTOS
        modelBuilder.Entity<Mantenimiento>()
            .HasOne(m => m.Equipo)
            .WithMany()
            .HasForeignKey(m => m.EquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Mantenimiento>()
            .HasOne(m => m.ReporteFalla)
            .WithMany()
            .HasForeignKey(m => m.ReporteFallaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Mantenimiento>()
            .HasOne(m => m.TipoMantenimiento)
            .WithMany(t => t.Mantenimientos)
            .HasForeignKey(m => m.TipoMantenimientoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Mantenimiento>()
            .HasOne(m => m.Tecnico)
            .WithMany(u => u.MantenimientosRealizados)
            .HasForeignKey(m => m.TecnicoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MantenimientoRepuesto>()
            .HasOne(r => r.Mantenimiento)
            .WithMany(m => m.Repuestos)
            .HasForeignKey(r => r.MantenimientoId)
            .OnDelete(DeleteBehavior.Restrict);

        // AUDITORÍA
        modelBuilder.Entity<HistorialOperacion>()
            .HasOne(h => h.Usuario)
            .WithMany(u => u.HistorialOperaciones)
            .HasForeignKey(h => h.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HistorialEstadoEquipo>()
            .HasOne(h => h.Equipo)
            .WithMany()
            .HasForeignKey(h => h.EquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HistorialEstadoEquipo>()
            .HasOne(h => h.EstadoAnterior)
            .WithMany()
            .HasForeignKey(h => h.EstadoAnteriorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HistorialEstadoEquipo>()
            .HasOne(h => h.EstadoNuevo)
            .WithMany()
            .HasForeignKey(h => h.EstadoNuevoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HistorialEstadoEquipo>()
            .HasOne(h => h.Usuario)
            .WithMany(u => u.HistorialEstadosEquipo)
            .HasForeignKey(h => h.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        // ===============================
        // Checklist técnico
        // ===============================

        modelBuilder.Entity<PlantillaChecklist>(entity =>
        {
            entity.ToTable("PlantillasChecklist");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(500);

            entity.Property(e => e.Activo)
                .HasDefaultValue(true);

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.CategoriaEquipo)
                .WithMany()
                .HasForeignKey(e => e.CategoriaEquipoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TipoEquipo)
                .WithMany()
                .HasForeignKey(e => e.TipoEquipoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PlantillaChecklistItem>(entity =>
        {
            entity.ToTable("PlantillaChecklistItems");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(500);

            entity.Property(e => e.Orden)
                .IsRequired();

            entity.Property(e => e.EsObligatorio)
                .HasDefaultValue(true);

            entity.Property(e => e.Activo)
                .HasDefaultValue(true);

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.PlantillaChecklist)
                .WithMany(e => e.Items)
                .HasForeignKey(e => e.PlantillaChecklistId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ChecklistTecnico>(entity =>
        {
            entity.ToTable("ChecklistTecnicos");

            entity.Property(e => e.EstadoChecklist)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.ObservacionesGenerales)
                .HasMaxLength(1000);

            entity.Property(e => e.Activo)
                .HasDefaultValue(true);

            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Ubicacion)
                .WithMany()
                .HasForeignKey(e => e.UbicacionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Tecnico)
                .WithMany()
                .HasForeignKey(e => e.TecnicoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ChecklistTecnicoEquipo>(entity =>
        {
            entity.ToTable("ChecklistTecnicoEquipos");

            entity.Property(e => e.ResultadoGeneral)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(e => e.ObservacionesEquipo)
                .HasMaxLength(1000);

            entity.HasOne(e => e.ChecklistTecnico)
                .WithMany(e => e.EquiposRevisados)
                .HasForeignKey(e => e.ChecklistTecnicoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Equipo)
                .WithMany()
                .HasForeignKey(e => e.EquipoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PlantillaChecklist)
                .WithMany(e => e.ChecklistTecnicoEquipos)
                .HasForeignKey(e => e.PlantillaChecklistId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ChecklistTecnicoDetalle>(entity =>
        {
            entity.ToTable("ChecklistTecnicoDetalles");

            entity.Property(e => e.EstadoRevision)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Observacion)
                .HasMaxLength(1000);

            entity.HasOne(e => e.ChecklistTecnicoEquipo)
                .WithMany(e => e.Detalles)
                .HasForeignKey(e => e.ChecklistTecnicoEquipoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.PlantillaChecklistItem)
                .WithMany(e => e.DetallesChecklist)
                .HasForeignKey(e => e.PlantillaChecklistItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // DECIMALES
        modelBuilder.Entity<Mantenimiento>()
            .Property(m => m.CostoEstimado)
            .HasPrecision(18, 2);

        modelBuilder.Entity<MantenimientoRepuesto>()
            .Property(r => r.CostoUnitario)
            .HasPrecision(18, 2);
    }
}