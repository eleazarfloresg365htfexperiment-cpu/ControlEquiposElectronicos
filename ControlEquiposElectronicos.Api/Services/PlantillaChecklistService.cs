using ControlEquiposElectronicos.Api.Data;
using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.DTOs.Checklist;
using ControlEquiposElectronicos.Api.Entities.Checklist;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControlEquiposElectronicos.Api.Services;

public PlantillaChecklistService : IPlantillaChecklistService
{
    private readonly AppDbContext _context;
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUsuarioActualService _usuarioActualService;

    public PlantillaChecklistService(
        AppDbContext context,
        IAuditoriaService auditoriaService,
        IUsuarioActualService usuarioActualService)
    {
        _context = context;
        _auditoriaService = auditoriaService;
        _usuarioActualService = usuarioActualService;
    }

    public async Task<List<PlantillaChecklistDto>> ObtenerTodasAsync()
    {
        return await _context.PlantillasChecklist
            .Include(p => p.CategoriaEquipo)
            .Include(p => p.TipoEquipo)
            .Include(p => p.Items)
            .OrderBy(p => p.TipoEquipo.Nombre)
            .ThenBy(p => p.Nombre)
            .Select(p => new PlantillaChecklistDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,

                CategoriaEquipoId = p.CategoriaEquipoId,
                CategoriaEquipo = p.CategoriaEquipo != null ? p.CategoriaEquipo.Nombre : null,

                TipoEquipoId = p.TipoEquipoId,
                TipoEquipo = p.TipoEquipo.Nombre,

                Activo = p.Activo,
                FechaCreacion = p.FechaCreacion,
                FechaActualizacion = p.FechaActualizacion,

                Items = p.Items
                    .OrderBy(i => i.Orden)
                    .Select(i => new PlantillaChecklistItemDto
                    {
                        Id = i.Id,
                        PlantillaChecklistId = i.PlantillaChecklistId,
                        Nombre = i.Nombre,
                        Descripcion = i.Descripcion,
                        Orden = i.Orden,
                        EsObligatorio = i.EsObligatorio,
                        Activo = i.Activo,
                        FechaCreacion = i.FechaCreacion,
                        FechaActualizacion = i.FechaActualizacion
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<PlantillaChecklistDto?> ObtenerPorIdAsync(int id)
    {
        return await _context.PlantillasChecklist
            .Include(p => p.CategoriaEquipo)
            .Include(p => p.TipoEquipo)
            .Include(p => p.Items)
            .Where(p => p.Id == id)
            .Select(p => new PlantillaChecklistDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,

                CategoriaEquipoId = p.CategoriaEquipoId,
                CategoriaEquipo = p.CategoriaEquipo != null ? p.CategoriaEquipo.Nombre : null,

                TipoEquipoId = p.TipoEquipoId,
                TipoEquipo = p.TipoEquipo.Nombre,

                Activo = p.Activo,
                FechaCreacion = p.FechaCreacion,
                FechaActualizacion = p.FechaActualizacion,

                Items = p.Items
                    .OrderBy(i => i.Orden)
                    .Select(i => new PlantillaChecklistItemDto
                    {
                        Id = i.Id,
                        PlantillaChecklistId = i.PlantillaChecklistId,
                        Nombre = i.Nombre,
                        Descripcion = i.Descripcion,
                        Orden = i.Orden,
                        EsObligatorio = i.EsObligatorio,
                        Activo = i.Activo,
                        FechaCreacion = i.FechaCreacion,
                        FechaActualizacion = i.FechaActualizacion
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PlantillaChecklistDto?> ObtenerPorTipoEquipoAsync(int tipoEquipoId)
    {
        return await _context.PlantillasChecklist
            .Include(p => p.CategoriaEquipo)
            .Include(p => p.TipoEquipo)
            .Include(p => p.Items)
            .Where(p => p.TipoEquipoId == tipoEquipoId && p.Activo)
            .OrderByDescending(p => p.FechaCreacion)
            .Select(p => new PlantillaChecklistDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,

                CategoriaEquipoId = p.CategoriaEquipoId,
                CategoriaEquipo = p.CategoriaEquipo != null ? p.CategoriaEquipo.Nombre : null,

                TipoEquipoId = p.TipoEquipoId,
                TipoEquipo = p.TipoEquipo.Nombre,

                Activo = p.Activo,
                FechaCreacion = p.FechaCreacion,
                FechaActualizacion = p.FechaActualizacion,

                Items = p.Items
                    .Where(i => i.Activo)
                    .OrderBy(i => i.Orden)
                    .Select(i => new PlantillaChecklistItemDto
                    {
                        Id = i.Id,
                        PlantillaChecklistId = i.PlantillaChecklistId,
                        Nombre = i.Nombre,
                        Descripcion = i.Descripcion,
                        Orden = i.Orden,
                        EsObligatorio = i.EsObligatorio,
                        Activo = i.Activo,
                        FechaCreacion = i.FechaCreacion,
                        FechaActualizacion = i.FechaActualizacion
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PlantillaChecklistDto> CrearAsync(CrearPlantillaChecklistDto dto)
    {
        await ValidarRelacionesAsync(dto.CategoriaEquipoId, dto.TipoEquipoId);

        var nombre = dto.Nombre.Trim();

        var existeNombre = await _context.PlantillasChecklist
            .AnyAsync(p => p.Nombre == nombre);

        if (existeNombre)
            throw new InvalidOperationException("Ya existe una plantilla de checklist con ese nombre.");

        var existePlantillaActivaParaTipo = await _context.PlantillasChecklist
            .AnyAsync(p => p.TipoEquipoId == dto.TipoEquipoId && p.Activo);

        if (existePlantillaActivaParaTipo)
            throw new InvalidOperationException("Ya existe una plantilla activa para este tipo de equipo.");

        var plantilla = new PlantillaChecklist
        {
            Nombre = nombre,
            Descripcion = dto.Descripcion?.Trim(),
            CategoriaEquipoId = dto.CategoriaEquipoId,
            TipoEquipoId = dto.TipoEquipoId,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        _context.PlantillasChecklist.Add(plantilla);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Creación de plantilla de checklist",
            Modulo = "Checklist",
            TablaAfectada = "PlantillasChecklist",
            RegistroId = plantilla.Id,
            Descripcion =
                $"Se creó la plantilla de checklist '{plantilla.Nombre}' para el TipoEquipoId {plantilla.TipoEquipoId}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        var creada = await ObtenerPorIdAsync(plantilla.Id);

        return creada!;
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarPlantillaChecklistDto dto)
    {
        var plantilla = await _context.PlantillasChecklist
            .FirstOrDefaultAsync(p => p.Id == id);

        if (plantilla == null)
            return false;

        await ValidarRelacionesAsync(dto.CategoriaEquipoId, dto.TipoEquipoId);

        var nombre = dto.Nombre.Trim();

        var existeNombre = await _context.PlantillasChecklist
            .AnyAsync(p => p.Nombre == nombre && p.Id != id);

        if (existeNombre)
            throw new InvalidOperationException("Ya existe otra plantilla de checklist con ese nombre.");

        if (dto.Activo)
        {
            var existePlantillaActivaParaTipo = await _context.PlantillasChecklist
                .AnyAsync(p => p.TipoEquipoId == dto.TipoEquipoId && p.Activo && p.Id != id);

            if (existePlantillaActivaParaTipo)
                throw new InvalidOperationException("Ya existe otra plantilla activa para este tipo de equipo.");
        }

        var nombreAnterior = plantilla.Nombre;
        var tipoAnteriorId = plantilla.TipoEquipoId;

        plantilla.Nombre = nombre;
        plantilla.Descripcion = dto.Descripcion?.Trim();
        plantilla.CategoriaEquipoId = dto.CategoriaEquipoId;
        plantilla.TipoEquipoId = dto.TipoEquipoId;
        plantilla.Activo = dto.Activo;
        plantilla.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Actualización de plantilla de checklist",
            Modulo = "Checklist",
            TablaAfectada = "PlantillasChecklist",
            RegistroId = plantilla.Id,
            Descripcion =
                $"Se actualizó la plantilla de checklist Id {plantilla.Id}. " +
                $"Nombre anterior: {nombreAnterior}. Nombre actual: {plantilla.Nombre}. " +
                $"TipoEquipoId anterior: {tipoAnteriorId}. TipoEquipoId actual: {plantilla.TipoEquipoId}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> DesactivarAsync(int id)
    {
        var plantilla = await _context.PlantillasChecklist
            .FirstOrDefaultAsync(p => p.Id == id);

        if (plantilla == null)
            return false;

        plantilla.Activo = false;
        plantilla.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Desactivación de plantilla de checklist",
            Modulo = "Checklist",
            TablaAfectada = "PlantillasChecklist",
            RegistroId = plantilla.Id,
            Descripcion =
                $"Se desactivó la plantilla de checklist '{plantilla.Nombre}'.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<List<PlantillaChecklistItemDto>> ObtenerItemsAsync(int plantillaId)
    {
        var plantillaExiste = await _context.PlantillasChecklist
            .AnyAsync(p => p.Id == plantillaId);

        if (!plantillaExiste)
            throw new InvalidOperationException("La plantilla de checklist no existe.");

        return await _context.PlantillaChecklistItems
            .Where(i => i.PlantillaChecklistId == plantillaId)
            .OrderBy(i => i.Orden)
            .Select(i => new PlantillaChecklistItemDto
            {
                Id = i.Id,
                PlantillaChecklistId = i.PlantillaChecklistId,
                Nombre = i.Nombre,
                Descripcion = i.Descripcion,
                Orden = i.Orden,
                EsObligatorio = i.EsObligatorio,
                Activo = i.Activo,
                FechaCreacion = i.FechaCreacion,
                FechaActualizacion = i.FechaActualizacion
            })
            .ToListAsync();
    }

    public async Task<PlantillaChecklistItemDto> AgregarItemAsync(int plantillaId, CrearPlantillaChecklistItemDto dto)
    {
        var plantilla = await _context.PlantillasChecklist
            .FirstOrDefaultAsync(p => p.Id == plantillaId && p.Activo);

        if (plantilla == null)
            throw new InvalidOperationException("La plantilla de checklist no existe o no está activa.");

        var nombre = dto.Nombre.Trim();

        var itemDuplicado = await _context.PlantillaChecklistItems
            .AnyAsync(i =>
                i.PlantillaChecklistId == plantillaId &&
                i.Nombre == nombre &&
                i.Activo);

        if (itemDuplicado)
            throw new InvalidOperationException("Ya existe un aspecto activo con ese nombre en esta plantilla.");

        var orden = dto.Orden;

        if (orden <= 0)
        {
            var ultimoOrden = await _context.PlantillaChecklistItems
                .Where(i => i.PlantillaChecklistId == plantillaId)
                .Select(i => (int?)i.Orden)
                .MaxAsync() ?? 0;

            orden = ultimoOrden + 1;
        }

        var item = new PlantillaChecklistItem
        {
            PlantillaChecklistId = plantillaId,
            Nombre = nombre,
            Descripcion = dto.Descripcion?.Trim(),
            Orden = orden,
            EsObligatorio = dto.EsObligatorio,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        _context.PlantillaChecklistItems.Add(item);
        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Creación de aspecto de checklist",
            Modulo = "Checklist",
            TablaAfectada = "PlantillaChecklistItems",
            RegistroId = item.Id,
            Descripcion =
                $"Se agregó el aspecto '{item.Nombre}' a la plantilla '{plantilla.Nombre}'. " +
                $"Orden: {item.Orden}. Obligatorio: {item.EsObligatorio}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return new PlantillaChecklistItemDto
        {
            Id = item.Id,
            PlantillaChecklistId = item.PlantillaChecklistId,
            Nombre = item.Nombre,
            Descripcion = item.Descripcion,
            Orden = item.Orden,
            EsObligatorio = item.EsObligatorio,
            Activo = item.Activo,
            FechaCreacion = item.FechaCreacion,
            FechaActualizacion = item.FechaActualizacion
        };
    }

    public async Task<bool> ActualizarItemAsync(int itemId, ActualizarPlantillaChecklistItemDto dto)
    {
        var item = await _context.PlantillaChecklistItems
            .Include(i => i.PlantillaChecklist)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item == null)
            return false;

        var nombre = dto.Nombre.Trim();

        var itemDuplicado = await _context.PlantillaChecklistItems
            .AnyAsync(i =>
                i.PlantillaChecklistId == item.PlantillaChecklistId &&
                i.Nombre == nombre &&
                i.Activo &&
                i.Id != itemId);

        if (itemDuplicado)
            throw new InvalidOperationException("Ya existe otro aspecto activo con ese nombre en esta plantilla.");

        if (dto.Orden <= 0)
            throw new InvalidOperationException("El orden del aspecto debe ser mayor que cero.");

        var nombreAnterior = item.Nombre;
        var ordenAnterior = item.Orden;

        item.Nombre = nombre;
        item.Descripcion = dto.Descripcion?.Trim();
        item.Orden = dto.Orden;
        item.EsObligatorio = dto.EsObligatorio;
        item.Activo = dto.Activo;
        item.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Actualización de aspecto de checklist",
            Modulo = "Checklist",
            TablaAfectada = "PlantillaChecklistItems",
            RegistroId = item.Id,
            Descripcion =
                $"Se actualizó el aspecto Id {item.Id} de la plantilla '{item.PlantillaChecklist.Nombre}'. " +
                $"Nombre anterior: {nombreAnterior}. Nombre actual: {item.Nombre}. " +
                $"Orden anterior: {ordenAnterior}. Orden actual: {item.Orden}.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    public async Task<bool> DesactivarItemAsync(int itemId)
    {
        var item = await _context.PlantillaChecklistItems
            .Include(i => i.PlantillaChecklist)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item == null)
            return false;

        item.Activo = false;
        item.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _auditoriaService.RegistrarAsync(new CrearHistorialOperacionDto
        {
            UsuarioId = _usuarioActualService.ObtenerUsuarioId(),
            Accion = "Desactivación de aspecto de checklist",
            Modulo = "Checklist",
            TablaAfectada = "PlantillaChecklistItems",
            RegistroId = item.Id,
            Descripcion =
                $"Se desactivó el aspecto '{item.Nombre}' de la plantilla '{item.PlantillaChecklist.Nombre}'.",
            DireccionIP = _usuarioActualService.ObtenerDireccionIP()
        });

        return true;
    }

    private async Task ValidarRelacionesAsync(int? categoriaEquipoId, int tipoEquipoId)
    {
        if (categoriaEquipoId.HasValue)
        {
            var categoriaExiste = await _context.CategoriasEquipo
                .AnyAsync(c => c.Id == categoriaEquipoId.Value);

            if (!categoriaExiste)
                throw new InvalidOperationException("La categoría de equipo no existe.");
        }

        var tipoExiste = await _context.TiposEquipo
            .AnyAsync(t => t.Id == tipoEquipoId);

        if (!tipoExiste)
            throw new InvalidOperationException("El tipo de equipo no existe.");

        if (categoriaEquipoId.HasValue)
        {
            var tipoPerteneceCategoria = await _context.TiposEquipo
                .AnyAsync(t =>
                    t.Id == tipoEquipoId &&
                    t.CategoriaEquipoId == categoriaEquipoId.Value);

            if (!tipoPerteneceCategoria)
                throw new InvalidOperationException("El tipo de equipo no pertenece a la categoría indicada.");
        }
    }
}