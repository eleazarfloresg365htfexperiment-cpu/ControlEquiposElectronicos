using ControlEquiposElectronicos.Api.Entities.Inventario;
using ControlEquiposElectronicos.Api.Entities.Mantenimientos;
using ControlEquiposElectronicos.Api.Entities.Reportes;
using ControlEquiposElectronicos.Api.Entities.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedRolesAsync(context);
        await SeedPermisosAsync(context);
        await SeedRolPermisosAsync(context);

        await SeedCategoriasEquipoAsync(context);
        await SeedEstadosEquipoAsync(context);
        await SeedUbicacionesAsync(context);
        await SeedTiposEquipoAsync(context);

        await SeedEstadosReporteAsync(context);
        await SeedTiposMantenimientoAsync(context);
    }

    private static async Task SeedRolesAsync(AppDbContext context)
    {
        if (await context.Roles.AnyAsync())
            return;

        var roles = new List<Rol>
        {
            new Rol
            {
                NombreRol = "Consulta",
                Descripcion = "Usuario con acceso únicamente de consulta.",
                EsSistema = true,
                Activo = true
            },
            new Rol
            {
                NombreRol = "Tecnico",
                Descripcion = "Usuario encargado de reportes, diagnósticos y mantenimientos.",
                EsSistema = true,
                Activo = true
            },
            new Rol
            {
                NombreRol = "Administrador",
                Descripcion = "Usuario con permisos administrativos generales.",
                EsSistema = true,
                Activo = true
            },
            new Rol
            {
                NombreRol = "OP",
                Descripcion = "Operador superior del sistema. Puede gestionar configuración y permisos.",
                EsSistema = true,
                Activo = true
            }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPermisosAsync(AppDbContext context)
    {
        var permisos = new List<Permiso>
    {
        // Dashboard
        new Permiso { Modulo = "Dashboard", Accion = "Ver", Descripcion = "Ver panel principal." },

        // Equipos
        new Permiso { Modulo = "Equipos", Accion = "Ver", Descripcion = "Ver equipos registrados." },
        new Permiso { Modulo = "Equipos", Accion = "Crear", Descripcion = "Registrar nuevos equipos." },
        new Permiso { Modulo = "Equipos", Accion = "Editar", Descripcion = "Editar información de equipos." },
        new Permiso { Modulo = "Equipos", Accion = "Eliminar", Descripcion = "Eliminar o desactivar equipos." },
        new Permiso { Modulo = "Equipos", Accion = "Reclasificar", Descripcion = "Reclasificar equipos." },
        new Permiso { Modulo = "Equipos", Accion = "VerHistorial", Descripcion = "Ver historial de equipos." },

        // Reportes
        new Permiso { Modulo = "Reportes", Accion = "Ver", Descripcion = "Ver reportes de fallas." },
        new Permiso { Modulo = "Reportes", Accion = "Crear", Descripcion = "Crear reportes de fallas." },
        new Permiso { Modulo = "Reportes", Accion = "Editar", Descripcion = "Editar reportes de fallas." },
        new Permiso { Modulo = "Reportes", Accion = "Cerrar", Descripcion = "Cerrar reportes de fallas." },

        // Mantenimientos
        new Permiso { Modulo = "Mantenimientos", Accion = "Ver", Descripcion = "Ver mantenimientos." },
        new Permiso { Modulo = "Mantenimientos", Accion = "Crear", Descripcion = "Registrar mantenimientos." },
        new Permiso { Modulo = "Mantenimientos", Accion = "Editar", Descripcion = "Editar mantenimientos." },
        new Permiso { Modulo = "Mantenimientos", Accion = "Finalizar", Descripcion = "Finalizar mantenimientos." },

        // Usuarios
        new Permiso { Modulo = "Usuarios", Accion = "Ver", Descripcion = "Ver usuarios." },
        new Permiso { Modulo = "Usuarios", Accion = "Crear", Descripcion = "Crear usuarios." },
        new Permiso { Modulo = "Usuarios", Accion = "Editar", Descripcion = "Editar usuarios." },
        new Permiso { Modulo = "Usuarios", Accion = "Desactivar", Descripcion = "Desactivar usuarios." },

        // Configuración
        new Permiso { Modulo = "Configuracion", Accion = "Ver", Descripcion = "Ver configuración del sistema." },
        new Permiso { Modulo = "Configuracion", Accion = "Editar", Descripcion = "Editar configuración del sistema." },

        // Permisos
        new Permiso { Modulo = "Permisos", Accion = "Ver", Descripcion = "Ver permisos del sistema." },
        new Permiso { Modulo = "Permisos", Accion = "Gestionar", Descripcion = "Gestionar permisos por rol." },

        // Checklist técnico
        new Permiso { Modulo = "Checklist", Accion = "Ver", Descripcion = "Ver checklist técnicos." },
        new Permiso { Modulo = "Checklist", Accion = "Crear", Descripcion = "Iniciar checklist técnicos." },
        new Permiso { Modulo = "Checklist", Accion = "Editar", Descripcion = "Guardar avances de checklist técnicos." },
        new Permiso { Modulo = "Checklist", Accion = "Finalizar", Descripcion = "Finalizar checklist técnicos." },
        new Permiso { Modulo = "Checklist", Accion = "Cancelar", Descripcion = "Cancelar checklist técnicos." },
        new Permiso { Modulo = "Checklist", Accion = "CrearReporte", Descripcion = "Crear reportes de falla desde checklist." },

        // Plantillas de checklist
        new Permiso { Modulo = "ChecklistPlantillas", Accion = "Ver", Descripcion = "Ver plantillas de checklist." },
        new Permiso { Modulo = "ChecklistPlantillas", Accion = "Crear", Descripcion = "Crear plantillas de checklist." },
        new Permiso { Modulo = "ChecklistPlantillas", Accion = "Editar", Descripcion = "Editar plantillas de checklist." },
        new Permiso { Modulo = "ChecklistPlantillas", Accion = "Eliminar", Descripcion = "Desactivar plantillas de checklist." },

        // Aspectos/items de checklist
        new Permiso { Modulo = "ChecklistItems", Accion = "Crear", Descripcion = "Agregar aspectos a plantillas de checklist." },
        new Permiso { Modulo = "ChecklistItems", Accion = "Editar", Descripcion = "Editar aspectos de plantillas de checklist." },
        new Permiso { Modulo = "ChecklistItems", Accion = "Eliminar", Descripcion = "Desactivar aspectos de plantillas de checklist." }
    };

        foreach (var permiso in permisos)
        {
            var existe = await context.Permisos
                .AnyAsync(p => p.Modulo == permiso.Modulo && p.Accion == permiso.Accion);

            if (!existe)
            {
                context.Permisos.Add(permiso);
            }
        }

        await context.SaveChangesAsync();
    }
    private static async Task SeedRolPermisosAsync(AppDbContext context)
    {
        var roles = await context.Roles.ToListAsync();
        var permisos = await context.Permisos.ToListAsync();

        var consulta = roles.First(r => r.NombreRol == "Consulta");
        var tecnico = roles.First(r => r.NombreRol == "Tecnico");
        var administrador = roles.First(r => r.NombreRol == "Administrador");
        var op = roles.First(r => r.NombreRol == "OP");

        async Task AsignarSiNoExisteAsync(Rol rol, string modulo, string accion)
        {
            var permiso = permisos.First(p => p.Modulo == modulo && p.Accion == accion);

            var existe = await context.RolPermisos
                .AnyAsync(rp => rp.RolId == rol.RolId && rp.PermisoId == permiso.Id);

            if (!existe)
            {
                context.RolPermisos.Add(new RolPermiso
                {
                    RolId = rol.RolId,
                    PermisoId = permiso.Id
                });
            }
        }

        // Consulta
        await AsignarSiNoExisteAsync(consulta, "Dashboard", "Ver");
        await AsignarSiNoExisteAsync(consulta, "Equipos", "Ver");
        await AsignarSiNoExisteAsync(consulta, "Reportes", "Ver");
        await AsignarSiNoExisteAsync(consulta, "Mantenimientos", "Ver");
        await AsignarSiNoExisteAsync(consulta, "Checklist", "Ver");

        // Técnico
        await AsignarSiNoExisteAsync(tecnico, "Dashboard", "Ver");

        await AsignarSiNoExisteAsync(tecnico, "Equipos", "Ver");
        await AsignarSiNoExisteAsync(tecnico, "Equipos", "Editar");
        await AsignarSiNoExisteAsync(tecnico, "Equipos", "Reclasificar");
        await AsignarSiNoExisteAsync(tecnico, "Equipos", "VerHistorial");

        await AsignarSiNoExisteAsync(tecnico, "Reportes", "Ver");
        await AsignarSiNoExisteAsync(tecnico, "Reportes", "Crear");
        await AsignarSiNoExisteAsync(tecnico, "Reportes", "Editar");
        await AsignarSiNoExisteAsync(tecnico, "Reportes", "Cerrar");

        await AsignarSiNoExisteAsync(tecnico, "Mantenimientos", "Ver");
        await AsignarSiNoExisteAsync(tecnico, "Mantenimientos", "Crear");
        await AsignarSiNoExisteAsync(tecnico, "Mantenimientos", "Editar");
        await AsignarSiNoExisteAsync(tecnico, "Mantenimientos", "Finalizar");

        await AsignarSiNoExisteAsync(tecnico, "Checklist", "Ver");
        await AsignarSiNoExisteAsync(tecnico, "Checklist", "Crear");
        await AsignarSiNoExisteAsync(tecnico, "Checklist", "Editar");
        await AsignarSiNoExisteAsync(tecnico, "Checklist", "Finalizar");
        await AsignarSiNoExisteAsync(tecnico, "Checklist", "CrearReporte");

        // Administrador
        foreach (var permiso in permisos.Where(p =>
                     !(p.Modulo == "Permisos" && p.Accion == "Gestionar") &&
                     !(p.Modulo == "Configuracion" && p.Accion == "Editar") &&
                     !(p.Modulo == "ChecklistPlantillas" && p.Accion is "Crear" or "Editar" or "Eliminar") &&
                     !(p.Modulo == "ChecklistItems" && p.Accion is "Crear" or "Editar" or "Eliminar")))
        {
            var existe = await context.RolPermisos
                .AnyAsync(rp => rp.RolId == administrador.RolId && rp.PermisoId == permiso.Id);

            if (!existe)
            {
                context.RolPermisos.Add(new RolPermiso
                {
                    RolId = administrador.RolId,
                    PermisoId = permiso.Id
                });
            }
        }

        // OP: todos los permisos
        foreach (var permiso in permisos)
        {
            var existe = await context.RolPermisos
                .AnyAsync(rp => rp.RolId == op.RolId && rp.PermisoId == permiso.Id);

            if (!existe)
            {
                context.RolPermisos.Add(new RolPermiso
                {
                    RolId = op.RolId,
                    PermisoId = permiso.Id
                });
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedCategoriasEquipoAsync(AppDbContext context)
    {
        if (await context.CategoriasEquipo.AnyAsync())
            return;

        var categorias = new List<CategoriaEquipo>
    {
        new CategoriaEquipo
        {
            Nombre = "Cómputo",
            Descripcion = "Computadoras y periféricos."
        },
        new CategoriaEquipo
        {
            Nombre = "Impresión",
            Descripcion = "Impresoras y equipos de impresión."
        },
        new CategoriaEquipo
        {
            Nombre = "Red",
            Descripcion = "Equipos de red y conectividad."
        },
        new CategoriaEquipo
        {
            Nombre = "Electricidad",
            Descripcion = "UPS, reguladores y protección eléctrica."
        },
        new CategoriaEquipo
        {
            Nombre = "Soporte ambiental",
            Descripcion = "Aire acondicionado, ventiladores y ambientadores."
        }
    };

        await context.CategoriasEquipo.AddRangeAsync(categorias);
        await context.SaveChangesAsync();
    }
    private static async Task SeedEstadosEquipoAsync(AppDbContext context)
    {
        if (await context.EstadosEquipo.AnyAsync())
            return;

        var estados = new List<EstadoEquipo>
        {
            new EstadoEquipo
            {
                Nombre = "Funcional",
                Descripcion = "Equipo disponible y funcional.",
                Activo = true
            },
            new EstadoEquipo
            {
                Nombre = "No funcional",
                Descripcion = "Equipo dañado o inutilizable.",
                Activo = true
            },
            new EstadoEquipo
            {
                Nombre = "En mantenimiento",
                Descripcion = "Equipo actualmente en mantenimiento.",
                Activo = true
            },
            new EstadoEquipo
            {
                Nombre = "En bodega",
                Descripcion = "Equipo almacenado.",
                Activo = true
            },
            new EstadoEquipo
            {
                Nombre = "Dado de baja",
                Descripcion = "Equipo retirado del inventario activo.",
                Activo = true
            },
            new EstadoEquipo
            {
                Nombre = "Operativo",
                Descripcion = "Equipo usado por instructores o técnicos.",
                Activo = true
            },
            new EstadoEquipo
            {
                Nombre = "Administrativo",
                Descripcion = "Equipo usado por personal administrativo.",
                Activo = true
            },
            new EstadoEquipo
            {
                Nombre = "Reasignado",
                Descripcion = "Equipo cambiado de ubicación o función.",
                Activo = true
            }
        };

        await context.EstadosEquipo.AddRangeAsync(estados);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUbicacionesAsync(AppDbContext context)
    {
        if (await context.Ubicaciones.AnyAsync())
            return;

        var ubicaciones = new List<Ubicacion>
        {
            new Ubicacion
            {
                Nombre = "Salón 1",
                TipoUbicacion = "Salón",
                Descripcion = "Salón de clases 1.",
                Activo = true
            },
            new Ubicacion
            {
                Nombre = "Salón 2",
                TipoUbicacion = "Salón",
                Descripcion = "Salón de clases 2.",
                Activo = true
            },
            new Ubicacion
            {
                Nombre = "Salón 3",
                TipoUbicacion = "Salón",
                Descripcion = "Salón de clases 3.",
                Activo = true
            },
            new Ubicacion
            {
                Nombre = "Salón 4",
                TipoUbicacion = "Salón",
                Descripcion = "Salón de clases 4.",
                Activo = true
            },
            new Ubicacion
            {
                Nombre = "Salón 5",
                TipoUbicacion = "Salón",
                Descripcion = "Salón de clases 5.",
                Activo = true
            },
            new Ubicacion
            {
                Nombre = "Redes 1",
                TipoUbicacion = "Laboratorio",
                Descripcion = "Laboratorio de redes 1.",
                Activo = true
            },
            new Ubicacion
            {
                Nombre = "Redes 2",
                TipoUbicacion = "Laboratorio",
                Descripcion = "Laboratorio de redes 2.",
                Activo = true
            },
            new Ubicacion
            {
                Nombre = "Oficina",
                TipoUbicacion = "Oficina",
                Descripcion = "Área de oficina.",
                Activo = true
            },
            new Ubicacion
            {
                Nombre = "Bodega",
                TipoUbicacion = "Bodega",
                Descripcion = "Área de almacenamiento.",
                Activo = true
            },
            new Ubicacion
            {
                Nombre = "Recepción",
                TipoUbicacion = "Administración",
                Descripcion = "Área de recepción.",
                Activo = true
            },
            new Ubicacion
            {
                Nombre = "Dirección",
                TipoUbicacion = "Administración",
                Descripcion = "Área de dirección.",
                Activo = true
            }
        };

        await context.Ubicaciones.AddRangeAsync(ubicaciones);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTiposEquipoAsync(AppDbContext context)
    {
        if (await context.TiposEquipo.AnyAsync())
            return;

        var categorias = await context.CategoriasEquipo.ToListAsync();

        var computo = categorias.First(c => c.Nombre == "Cómputo");
        var impresion = categorias.First(c => c.Nombre == "Impresión");
        var red = categorias.First(c => c.Nombre == "Red");
        var electricidad = categorias.First(c => c.Nombre == "Electricidad");
        var ambiental = categorias.First(c => c.Nombre == "Soporte ambiental");

        var tipos = new List<TipoEquipo>
        {
            // Cómputo
            new TipoEquipo { CategoriaEquipoId = computo.Id, Nombre = "PC", Descripcion = "Computadora de escritorio.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = computo.Id, Nombre = "Laptop", Descripcion = "Computadora portátil.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = computo.Id, Nombre = "Monitor", Descripcion = "Pantalla o monitor.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = computo.Id, Nombre = "Teclado", Descripcion = "Teclado de computadora.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = computo.Id, Nombre = "Mouse", Descripcion = "Mouse o ratón.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = computo.Id, Nombre = "Bocinas", Descripcion = "Bocinas o parlantes.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = computo.Id, Nombre = "Cámara web", Descripcion = "Cámara para videollamadas.", Activo = true },

            // Impresión
            new TipoEquipo { CategoriaEquipoId = impresion.Id, Nombre = "Impresora", Descripcion = "Equipo de impresión.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = impresion.Id, Nombre = "Multifuncional", Descripcion = "Impresora multifuncional.", Activo = true },

            // Red
            new TipoEquipo { CategoriaEquipoId = red.Id, Nombre = "Router", Descripcion = "Equipo router.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = red.Id, Nombre = "Switch", Descripcion = "Equipo switch.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = red.Id, Nombre = "Repetidor", Descripcion = "Repetidor de señal.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = red.Id, Nombre = "Access Point", Descripcion = "Punto de acceso inalámbrico.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = red.Id, Nombre = "Cableado", Descripcion = "Cableado de red.", Activo = true },

            // Electricidad
            new TipoEquipo { CategoriaEquipoId = electricidad.Id, Nombre = "UPS", Descripcion = "Sistema de alimentación ininterrumpida.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = electricidad.Id, Nombre = "Regulador", Descripcion = "Regulador de voltaje.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = electricidad.Id, Nombre = "Supresor", Descripcion = "Supresor de picos.", Activo = true },

            // Soporte ambiental
            new TipoEquipo { CategoriaEquipoId = ambiental.Id, Nombre = "Aire acondicionado", Descripcion = "Equipo de aire acondicionado.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = ambiental.Id, Nombre = "Ambientador", Descripcion = "Equipo ambientador.", Activo = true },
            new TipoEquipo { CategoriaEquipoId = ambiental.Id, Nombre = "Ventilador", Descripcion = "Ventilador.", Activo = true }
        };

        await context.TiposEquipo.AddRangeAsync(tipos);
        await context.SaveChangesAsync();
    }

    private static async Task SeedEstadosReporteAsync(AppDbContext context)
    {
        if (await context.EstadosReporte.AnyAsync())
            return;

        var estados = new List<EstadoReporte>
        {
            new EstadoReporte
            {
                Nombre = "Abierto",
                Descripcion = "Reporte recién creado.",
                Activo = true
            },
            new EstadoReporte
            {
                Nombre = "Asignado",
                Descripcion = "Reporte asignado a un técnico.",
                Activo = true
            },
            new EstadoReporte
            {
                Nombre = "En revisión",
                Descripcion = "Reporte en proceso de revisión.",
                Activo = true
            },
            new EstadoReporte
            {
                Nombre = "Resuelto",
                Descripcion = "Reporte resuelto técnicamente.",
                Activo = true
            },
            new EstadoReporte
            {
                Nombre = "Cerrado",
                Descripcion = "Reporte cerrado.",
                Activo = true
            },
            new EstadoReporte
            {
                Nombre = "Cancelado",
                Descripcion = "Reporte cancelado.",
                Activo = true
            }
        };

        await context.EstadosReporte.AddRangeAsync(estados);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTiposMantenimientoAsync(AppDbContext context)
    {
        if (await context.TiposMantenimiento.AnyAsync())
            return;

        var tipos = new List<TipoMantenimiento>
        {
            new TipoMantenimiento
            {
                Nombre = "Preventivo",
                Descripcion = "Mantenimiento preventivo programado.",
                Activo = true
            },
            new TipoMantenimiento
            {
                Nombre = "Correctivo",
                Descripcion = "Mantenimiento para corregir fallas.",
                Activo = true
            },
            new TipoMantenimiento
            {
                Nombre = "Limpieza",
                Descripcion = "Limpieza física o interna del equipo.",
                Activo = true
            },
            new TipoMantenimiento
            {
                Nombre = "Actualización",
                Descripcion = "Actualización de software, sistema o configuración.",
                Activo = true
            },
            new TipoMantenimiento
            {
                Nombre = "Revisión",
                Descripcion = "Revisión general del estado del equipo.",
                Activo = true
            },
            new TipoMantenimiento
            {
                Nombre = "Cambio de componente",
                Descripcion = "Cambio de piezas o componentes.",
                Activo = true
            },
            new TipoMantenimiento
            {
                Nombre = "Instalación",
                Descripcion = "Instalación de equipo, sistema o componente.",
                Activo = true
            },
            new TipoMantenimiento
            {
                Nombre = "Configuración",
                Descripcion = "Configuración técnica del equipo.",
                Activo = true
            }
        };

        await context.TiposMantenimiento.AddRangeAsync(tipos);
        await context.SaveChangesAsync();
    }
}