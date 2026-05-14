using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Checklist;
using ControlEquiposElectronicos.Api.Entities.Checklist;
using ControlEquiposElectronicos.Api.Entities.Reportes;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public class ChecklistTecnicoService : IChecklistTecnicoService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    private static readonly string[] EstadosChecklistValidos =
    {
        "En proceso",
        "Finalizado",
        "Cancelado"
    };

    private static readonly string[] ResultadosEquipoValidos =
    {
        "Pendiente",
        "Revisado correctamente",
        "Revisado con observaciones",
        "Revisado con problemas",
        "No revisado"
    };

    private static readonly string[] EstadosRevisionValidos =
    {
        "Correcto",
        "Con problema",
        "No aplica",
        "No revisado"
    };

    public ChecklistTecnicoService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<List<ChecklistTecnicoDto>> ObtenerTodosAsync()
    {
        var checklists = await _context.ChecklistTecnicos
            .Include(c => c.Ubicacion)
            .Include(c => c.Tecnico)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Equipo)
                    .ThenInclude(e => e.TipoEquipo)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.PlantillaChecklist)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Detalles)
                    .ThenInclude(d => d.PlantillaChecklistItem)
            .OrderByDescending(c => c.FechaInicio)
            .AsNoTracking()
            .ToListAsync();

        return checklists.Select(MapChecklistTecnicoDto).ToList();
    }

    public async Task<ChecklistTecnicoDto?> ObtenerPorIdAsync(int id)
    {
        var checklist = await _context.ChecklistTecnicos
            .Include(c => c.Ubicacion)
            .Include(c => c.Tecnico)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Equipo)
                    .ThenInclude(e => e.TipoEquipo)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.PlantillaChecklist)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Detalles)
                    .ThenInclude(d => d.PlantillaChecklistItem)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return checklist == null ? null : MapChecklistTecnicoDto(checklist);
    }

    public async Task<List<ChecklistTecnicoDto>> ObtenerPorUbicacionAsync(int ubicacionId)
    {
        var checklists = await _context.ChecklistTecnicos
            .Include(c => c.Ubicacion)
            .Include(c => c.Tecnico)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Equipo)
                    .ThenInclude(e => e.TipoEquipo)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.PlantillaChecklist)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Detalles)
                    .ThenInclude(d => d.PlantillaChecklistItem)
            .Where(c => c.UbicacionId == ubicacionId)
            .OrderByDescending(c => c.FechaInicio)
            .AsNoTracking()
            .ToListAsync();

        return checklists.Select(MapChecklistTecnicoDto).ToList();
    }

    public async Task<List<ChecklistTecnicoDto>> ObtenerPorTecnicoAsync(int tecnicoId)
    {
        var checklists = await _context.ChecklistTecnicos
            .Include(c => c.Ubicacion)
            .Include(c => c.Tecnico)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Equipo)
                    .ThenInclude(e => e.TipoEquipo)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.PlantillaChecklist)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Detalles)
                    .ThenInclude(d => d.PlantillaChecklistItem)
            .Where(c => c.TecnicoId == tecnicoId)
            .OrderByDescending(c => c.FechaInicio)
            .AsNoTracking()
            .ToListAsync();

        return checklists.Select(MapChecklistTecnicoDto).ToList();
    }

    public async Task<List<ChecklistTecnicoDto>> ObtenerPorEquipoAsync(int equipoId)
    {
        var checklists = await _context.ChecklistTecnicos
            .Include(c => c.Ubicacion)
            .Include(c => c.Tecnico)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Equipo)
                    .ThenInclude(e => e.TipoEquipo)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.PlantillaChecklist)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Detalles)
                    .ThenInclude(d => d.PlantillaChecklistItem)
            .Where(c => c.EquiposRevisados.Any(e => e.EquipoId == equipoId))
            .OrderByDescending(c => c.FechaInicio)
            .AsNoTracking()
            .ToListAsync();

        return checklists.Select(MapChecklistTecnicoDto).ToList();
    }

    public async Task<PrepararChecklistUbicacionDto> PrepararPorUbicacionAsync(int ubicacionId)
    {
        var ubicacion = await _context.Ubicaciones
            .FirstOrDefaultAsync(u => u.Id == ubicacionId && u.Activo);

        if (ubicacion == null)
            throw new InvalidOperationException("La ubicación no existe o no está activa.");

        var equipos = await _context.Equipos
            .Include(e => e.TipoEquipo)
            .Where(e => e.UbicacionId == ubicacionId && e.Activo)
            .OrderBy(e => e.Codigo)
            .AsNoTracking()
            .ToListAsync();

        var tiposEquipoIds = equipos
            .Select(e => e.TipoEquipoId)
            .Distinct()
            .ToList();

        var plantillas = await _context.PlantillasChecklist
            .Include(p => p.Items)
            .Where(p => p.Activo && tiposEquipoIds.Contains(p.TipoEquipoId))
            .AsNoTracking()
            .ToListAsync();

        var resultado = new PrepararChecklistUbicacionDto
        {
            UbicacionId = ubicacion.Id,
            Ubicacion = ubicacion.Nombre,
            TotalEquipos = equipos.Count,
            Equipos = equipos.Select(e =>
            {
                var plantilla = plantillas
                    .Where(p => p.TipoEquipoId == e.TipoEquipoId)
                    .OrderByDescending(p => p.FechaCreacion)
                    .FirstOrDefault();

                return new EquipoChecklistPreparadoDto
                {
                    EquipoId = e.Id,
                    CodigoEquipo = e.Codigo,
                    NombreEquipo = e.Nombre,
                    TipoEquipoId = e.TipoEquipoId,
                    TipoEquipo = e.TipoEquipo.Nombre,

                    PlantillaChecklistId = plantilla?.Id,
                    PlantillaChecklist = plantilla?.Nombre,
                    TienePlantilla = plantilla != null,

                    Items = plantilla == null
                        ? new List<ItemChecklistPreparadoDto>()
                        : plantilla.Items
                            .Where(i => i.Activo)
                            .OrderBy(i => i.Orden)
                            .Select(i => new ItemChecklistPreparadoDto
                            {
                                PlantillaChecklistItemId = i.Id,
                                Nombre = i.Nombre,
                                Descripcion = i.Descripcion,
                                Orden = i.Orden,
                                EsObligatorio = i.EsObligatorio
                            })
                            .ToList()
                };
            }).ToList()
        };

        return resultado;
    }

    public async Task<ChecklistTecnicoDto> IniciarAsync(IniciarChecklistTecnicoDto dto)
    {
        var ubicacion = await _context.Ubicaciones
            .FirstOrDefaultAsync(u => u.Id == dto.UbicacionId && u.Activo);

        if (ubicacion == null)
            throw new InvalidOperationException("La ubicación no existe o no está activa.");

        var tecnico = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.UsuarioId == dto.TecnicoId && u.Activo);

        if (tecnico == null)
            throw new InvalidOperationException("El técnico no existe o no está activo.");

        var equipos = await _context.Equipos
            .Include(e => e.TipoEquipo)
            .Where(e => e.UbicacionId == dto.UbicacionId && e.Activo)
            .OrderBy(e => e.Codigo)
            .ToListAsync();

        if (!equipos.Any())
            throw new InvalidOperationException("No hay equipos activos registrados en esta ubicación.");

        var tiposEquipoIds = equipos
            .Select(e => e.TipoEquipoId)
            .Distinct()
            .ToList();

        var plantillas = await _context.PlantillasChecklist
            .Include(p => p.Items.Where(i => i.Activo))
            .Where(p => p.Activo && tiposEquipoIds.Contains(p.TipoEquipoId))
            .ToListAsync();

        var equiposConPlantilla = equipos
            .Select(e => new
            {
                Equipo = e,
                Plantilla = plantillas
                    .Where(p => p.TipoEquipoId == e.TipoEquipoId)
                    .OrderByDescending(p => p.FechaCreacion)
                    .FirstOrDefault()
            })
            .Where(x => x.Plantilla != null && x.Plantilla.Items.Any(i => i.Activo))
            .ToList();

        if (!equiposConPlantilla.Any())
        {
            throw new InvalidOperationException(
                "No hay equipos con plantilla activa de checklist en esta ubicación. " +
                "Debe crear plantillas para los tipos de equipo antes de iniciar el checklist.");
        }

        var fechaOperacion = DateTime.UtcNow;

        var checklist = new ChecklistTecnico
        {
            UbicacionId = dto.UbicacionId,
            TecnicoId = dto.TecnicoId,
            FechaInicio = fechaOperacion,
            EstadoChecklist = "En proceso",
            ObservacionesGenerales = dto.ObservacionesGenerales?.Trim(),
            Activo = true,
            FechaCreacion = fechaOperacion
        };

        foreach (var item in equiposConPlantilla)
        {
            var plantilla = item.Plantilla!;

            var checklistEquipo = new ChecklistTecnicoEquipo
            {
                ChecklistTecnico = checklist,
                EquipoId = item.Equipo.Id,
                PlantillaChecklistId = plantilla.Id,
                ResultadoGeneral = "Pendiente",
                ObservacionesEquipo = null,
                FechaRevision = null
            };

            foreach (var aspecto in plantilla.Items
                .Where(i => i.Activo)
                .OrderBy(i => i.Orden))
            {
                checklistEquipo.Detalles.Add(new ChecklistTecnicoDetalle
                {
                    PlantillaChecklistItemId = aspecto.Id,
                    EstadoRevision = "No revisado",
                    Observacion = null
                });
            }

            checklist.EquiposRevisados.Add(checklistEquipo);
        }

        _context.ChecklistTecnicos.Add(checklist);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = dto.TecnicoId,
            Accion = "Inicio de checklist técnico",
            Modulo = "Checklist",
            TablaAfectada = "ChecklistTecnicos",
            RegistroId = checklist.Id,
            Descripcion =
                $"Se inició un checklist técnico en la ubicación '{ubicacion.Nombre}'. " +
                $"Equipos incluidos: {checklist.EquiposRevisados.Count}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creado = await ObtenerPorIdAsync(checklist.Id);

        return creado!;
    }

    public async Task<bool> GuardarAvanceAsync(int id, GuardarChecklistTecnicoDto dto)
    {
        var checklist = await _context.ChecklistTecnicos
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Detalles)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (checklist == null)
            return false;

        if (checklist.EstadoChecklist == "Finalizado")
            throw new InvalidOperationException("No se puede modificar un checklist finalizado.");

        if (checklist.EstadoChecklist == "Cancelado")
            throw new InvalidOperationException("No se puede modificar un checklist cancelado.");

        checklist.ObservacionesGenerales = dto.ObservacionesGenerales?.Trim();
        checklist.FechaActualizacion = DateTime.UtcNow;

        foreach (var equipoDto in dto.Equipos)
        {
            ValidarResultadoEquipo(equipoDto.ResultadoGeneral);

            var checklistEquipo = checklist.EquiposRevisados
                .FirstOrDefault(e => e.Id == equipoDto.ChecklistTecnicoEquipoId);

            if (checklistEquipo == null)
                throw new InvalidOperationException($"El equipo de checklist Id {equipoDto.ChecklistTecnicoEquipoId} no pertenece a este checklist.");

            checklistEquipo.ResultadoGeneral = equipoDto.ResultadoGeneral.Trim();
            checklistEquipo.ObservacionesEquipo = equipoDto.ObservacionesEquipo?.Trim();
            checklistEquipo.FechaRevision = DateTime.UtcNow;

            foreach (var detalleDto in equipoDto.Detalles)
            {
                ValidarEstadoRevision(detalleDto.EstadoRevision);

                var detalle = checklistEquipo.Detalles
                    .FirstOrDefault(d => d.Id == detalleDto.ChecklistTecnicoDetalleId);

                if (detalle == null)
                    throw new InvalidOperationException($"El detalle de checklist Id {detalleDto.ChecklistTecnicoDetalleId} no pertenece al equipo indicado.");

                detalle.EstadoRevision = detalleDto.EstadoRevision.Trim();
                detalle.Observacion = detalleDto.Observacion?.Trim();
            }
        }

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = checklist.TecnicoId,
            Accion = "Guardado de avance de checklist técnico",
            Modulo = "Checklist",
            TablaAfectada = "ChecklistTecnicos",
            RegistroId = checklist.Id,
            Descripcion =
                $"Se guardó avance del checklist técnico Id {checklist.Id}. " +
                $"Equipos actualizados: {dto.Equipos.Count}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> FinalizarAsync(int id, FinalizarChecklistTecnicoDto dto)
    {
        var checklist = await _context.ChecklistTecnicos
            .Include(c => c.Ubicacion)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Equipo)
            .Include(c => c.EquiposRevisados)
                .ThenInclude(e => e.Detalles)
                    .ThenInclude(d => d.PlantillaChecklistItem)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (checklist == null)
            return false;

        if (checklist.EstadoChecklist == "Finalizado")
            throw new InvalidOperationException("Este checklist ya está finalizado.");

        if (checklist.EstadoChecklist == "Cancelado")
            throw new InvalidOperationException("No se puede finalizar un checklist cancelado.");

        checklist.EstadoChecklist = "Finalizado";
        checklist.FechaFinalizacion = DateTime.UtcNow;
        checklist.FechaActualizacion = DateTime.UtcNow;
        checklist.ObservacionesGenerales = dto.ObservacionesGenerales?.Trim() ?? checklist.ObservacionesGenerales;

        var reportesCreados = 0;

        if (dto.CrearReportesFallaAutomaticos)
        {
            reportesCreados = await CrearReportesFallaDesdeProblemasAsync(checklist);
        }

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = checklist.TecnicoId,
            Accion = "Finalización de checklist técnico",
            Modulo = "Checklist",
            TablaAfectada = "ChecklistTecnicos",
            RegistroId = checklist.Id,
            Descripcion =
                $"Se finalizó el checklist técnico Id {checklist.Id} de la ubicación '{checklist.Ubicacion.Nombre}'. " +
                $"Reportes de falla creados automáticamente: {reportesCreados}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> CancelarAsync(int id)
    {
        var checklist = await _context.ChecklistTecnicos
            .Include(c => c.Ubicacion)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (checklist == null)
            return false;

        if (checklist.EstadoChecklist == "Finalizado")
            throw new InvalidOperationException("No se puede cancelar un checklist finalizado.");

        checklist.EstadoChecklist = "Cancelado";
        checklist.Activo = false;
        checklist.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = checklist.TecnicoId,
            Accion = "Cancelación de checklist técnico",
            Modulo = "Checklist",
            TablaAfectada = "ChecklistTecnicos",
            RegistroId = checklist.Id,
            Descripcion =
                $"Se canceló el checklist técnico Id {checklist.Id} de la ubicación '{checklist.Ubicacion.Nombre}'.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    private ChecklistTecnicoDto MapChecklistTecnicoDto(ChecklistTecnico checklist)
    {
        return new ChecklistTecnicoDto
        {
            Id = checklist.Id,
            UbicacionId = checklist.UbicacionId,
            Ubicacion = checklist.Ubicacion.Nombre,

            TecnicoId = checklist.TecnicoId,
            Tecnico = checklist.Tecnico.Nickname,

            FechaInicio = checklist.FechaInicio,
            FechaFinalizacion = checklist.FechaFinalizacion,

            EstadoChecklist = checklist.EstadoChecklist,
            ObservacionesGenerales = checklist.ObservacionesGenerales,

            Activo = checklist.Activo,
            FechaCreacion = checklist.FechaCreacion,
            FechaActualizacion = checklist.FechaActualizacion,

            EquiposRevisados = checklist.EquiposRevisados
                .OrderBy(e => e.Equipo.Codigo)
                .Select(e => new ChecklistTecnicoEquipoDto
                {
                    Id = e.Id,
                    ChecklistTecnicoId = e.ChecklistTecnicoId,

                    EquipoId = e.EquipoId,
                    CodigoEquipo = e.Equipo.Codigo,
                    NombreEquipo = e.Equipo.Nombre,

                    TipoEquipoId = e.Equipo.TipoEquipoId,
                    TipoEquipo = e.Equipo.TipoEquipo.Nombre,

                    PlantillaChecklistId = e.PlantillaChecklistId,
                    PlantillaChecklist = e.PlantillaChecklist.Nombre,

                    ResultadoGeneral = e.ResultadoGeneral,
                    ObservacionesEquipo = e.ObservacionesEquipo,
                    FechaRevision = e.FechaRevision,

                    Detalles = e.Detalles
                        .OrderBy(d => d.PlantillaChecklistItem.Orden)
                        .Select(d => new ChecklistTecnicoDetalleDto
                        {
                            Id = d.Id,
                            ChecklistTecnicoEquipoId = d.ChecklistTecnicoEquipoId,

                            PlantillaChecklistItemId = d.PlantillaChecklistItemId,
                            Item = d.PlantillaChecklistItem.Nombre,
                            DescripcionItem = d.PlantillaChecklistItem.Descripcion,
                            Orden = d.PlantillaChecklistItem.Orden,
                            EsObligatorio = d.PlantillaChecklistItem.EsObligatorio,

                            EstadoRevision = d.EstadoRevision,
                            Observacion = d.Observacion
                        })
                        .ToList()
                })
                .ToList()
        };
    }

    private static void ValidarResultadoEquipo(string resultado)
    {
        if (string.IsNullOrWhiteSpace(resultado) ||
            !ResultadosEquipoValidos.Contains(resultado.Trim()))
        {
            throw new InvalidOperationException(
                "Resultado general inválido. Valores permitidos: " +
                string.Join(", ", ResultadosEquipoValidos));
        }
    }

    private static void ValidarEstadoRevision(string estado)
    {
        if (string.IsNullOrWhiteSpace(estado) ||
            !EstadosRevisionValidos.Contains(estado.Trim()))
        {
            throw new InvalidOperationException(
                "Estado de revisión inválido. Valores permitidos: " +
                string.Join(", ", EstadosRevisionValidos));
        }
    }

    private async Task<int> CrearReportesFallaDesdeProblemasAsync(ChecklistTecnico checklist)
    {
        var estadoReporte = await _context.EstadosReporte
            .Where(e => e.Activo)
            .OrderBy(e =>
                e.Nombre == "Abierto" ? 0 :
                e.Nombre == "Pendiente" ? 1 :
                e.Nombre == "Nuevo" ? 2 : 3)
            .ThenBy(e => e.Id)
            .FirstOrDefaultAsync();

        if (estadoReporte == null)
            throw new InvalidOperationException("No existe un estado de reporte activo para crear reportes de falla.");

        var detallesConProblema = checklist.EquiposRevisados
            .SelectMany(e => e.Detalles
                .Where(d => d.EstadoRevision == "Con problema")
                .Select(d => new
                {
                    ChecklistEquipo = e,
                    Detalle = d
                }))
            .ToList();

        var reportesCreados = 0;

        foreach (var grupoEquipo in detallesConProblema.GroupBy(x => x.ChecklistEquipo.EquipoId))
        {
            var checklistEquipo = grupoEquipo.First().ChecklistEquipo;
            var equipo = checklistEquipo.Equipo;

            var problemas = grupoEquipo
                .Select(x =>
                    $"- {x.Detalle.PlantillaChecklistItem.Nombre}: " +
                    $"{(string.IsNullOrWhiteSpace(x.Detalle.Observacion) ? "Sin observación específica." : x.Detalle.Observacion)}")
                .ToList();

            var descripcion =
                $"Problemas detectados durante el checklist técnico Id {checklist.Id} " +
                $"realizado en la ubicación '{checklist.Ubicacion.Nombre}'.\n\n" +
                string.Join("\n", problemas);

            var reporte = new ReporteFalla
            {
                EquipoId = equipo.Id,
                UsuarioReportaId = checklist.TecnicoId,
                EstadoReporteId = estadoReporte.Id,
                Titulo = $"Problemas detectados en checklist - {equipo.Codigo}",
                Descripcion = descripcion,
                Prioridad = "Media",
                FechaReporte = DateTime.UtcNow,
                FechaCierre = null,
                ObservacionesCierre = null
            };

            _context.ReportesFalla.Add(reporte);
            reportesCreados++;
        }

        return reportesCreados;
    }
}