using ControlEquiposElectronicos.Api.DTOs.Checklist;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlantillasChecklistController : ControllerBase
{
    private readonly IPlantillaChecklistService _plantillaChecklistService;

    public PlantillasChecklistController(IPlantillaChecklistService plantillaChecklistService)
    {
        _plantillaChecklistService = plantillaChecklistService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PlantillaChecklistDto>>> ObtenerTodas()
    {
        var plantillas = await _plantillaChecklistService.ObtenerTodasAsync();

        return Ok(plantillas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlantillaChecklistDto>> ObtenerPorId(int id)
    {
        var plantilla = await _plantillaChecklistService.ObtenerPorIdAsync(id);

        if (plantilla == null)
            return NotFound(new { mensaje = "La plantilla de checklist no existe." });

        return Ok(plantilla);
    }

    [HttpGet("tipo-equipo/{tipoEquipoId:int}")]
    public async Task<ActionResult<PlantillaChecklistDto>> ObtenerPorTipoEquipo(int tipoEquipoId)
    {
        var plantilla = await _plantillaChecklistService.ObtenerPorTipoEquipoAsync(tipoEquipoId);

        if (plantilla == null)
            return NotFound(new { mensaje = "No existe una plantilla activa para este tipo de equipo." });

        return Ok(plantilla);
    }

    [HttpPost]
    public async Task<ActionResult<PlantillaChecklistDto>> Crear(CrearPlantillaChecklistDto dto)
    {
        try
        {
            var plantilla = await _plantillaChecklistService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = plantilla.Id },
                plantilla);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, ActualizarPlantillaChecklistDto dto)
    {
        try
        {
            var actualizado = await _plantillaChecklistService.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound(new { mensaje = "La plantilla de checklist no existe." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desactivar(int id)
    {
        var desactivado = await _plantillaChecklistService.DesactivarAsync(id);

        if (!desactivado)
            return NotFound(new { mensaje = "La plantilla de checklist no existe." });

        return NoContent();
    }

    [HttpGet("{plantillaId:int}/items")]
    public async Task<ActionResult<List<PlantillaChecklistItemDto>>> ObtenerItems(int plantillaId)
    {
        try
        {
            var items = await _plantillaChecklistService.ObtenerItemsAsync(plantillaId);

            return Ok(items);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    [HttpPost("{plantillaId:int}/items")]
    public async Task<ActionResult<PlantillaChecklistItemDto>> AgregarItem(
        int plantillaId,
        CrearPlantillaChecklistItemDto dto)
    {
        try
        {
            var item = await _plantillaChecklistService.AgregarItemAsync(plantillaId, dto);

            return CreatedAtAction(
                nameof(ObtenerItems),
                new { plantillaId },
                item);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("items/{itemId:int}")]
    public async Task<IActionResult> ActualizarItem(
        int itemId,
        ActualizarPlantillaChecklistItemDto dto)
    {
        try
        {
            var actualizado = await _plantillaChecklistService.ActualizarItemAsync(itemId, dto);

            if (!actualizado)
                return NotFound(new { mensaje = "El aspecto de checklist no existe." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("items/{itemId:int}")]
    public async Task<IActionResult> DesactivarItem(int itemId)
    {
        var desactivado = await _plantillaChecklistService.DesactivarItemAsync(itemId);

        if (!desactivado)
            return NotFound(new { mensaje = "El aspecto de checklist no existe." });

        return NoContent();
    }
}