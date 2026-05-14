using ControlEquiposElectronicos.Api.DTOs.Checklist;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChecklistTecnicoController : ControllerBase
{
    private readonly IChecklistTecnicoService _checklistTecnicoService;

    public ChecklistTecnicoController(IChecklistTecnicoService checklistTecnicoService)
    {
        _checklistTecnicoService = checklistTecnicoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ChecklistTecnicoDto>>> ObtenerTodos()
    {
        var checklists = await _checklistTecnicoService.ObtenerTodosAsync();

        return Ok(checklists);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChecklistTecnicoDto>> ObtenerPorId(int id)
    {
        var checklist = await _checklistTecnicoService.ObtenerPorIdAsync(id);

        if (checklist == null)
            return NotFound(new { mensaje = "El checklist técnico no existe." });

        return Ok(checklist);
    }

    [HttpGet("ubicacion/{ubicacionId:int}")]
    public async Task<ActionResult<List<ChecklistTecnicoDto>>> ObtenerPorUbicacion(int ubicacionId)
    {
        var checklists = await _checklistTecnicoService.ObtenerPorUbicacionAsync(ubicacionId);

        return Ok(checklists);
    }

    [HttpGet("tecnico/{tecnicoId:int}")]
    public async Task<ActionResult<List<ChecklistTecnicoDto>>> ObtenerPorTecnico(int tecnicoId)
    {
        var checklists = await _checklistTecnicoService.ObtenerPorTecnicoAsync(tecnicoId);

        return Ok(checklists);
    }

    [HttpGet("equipo/{equipoId:int}")]
    public async Task<ActionResult<List<ChecklistTecnicoDto>>> ObtenerPorEquipo(int equipoId)
    {
        var checklists = await _checklistTecnicoService.ObtenerPorEquipoAsync(equipoId);

        return Ok(checklists);
    }

    [HttpGet("preparar/ubicacion/{ubicacionId:int}")]
    public async Task<ActionResult<PrepararChecklistUbicacionDto>> PrepararPorUbicacion(int ubicacionId)
    {
        try
        {
            var preparado = await _checklistTecnicoService.PrepararPorUbicacionAsync(ubicacionId);

            return Ok(preparado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("iniciar")]
    public async Task<ActionResult<ChecklistTecnicoDto>> Iniciar(IniciarChecklistTecnicoDto dto)
    {
        try
        {
            var checklist = await _checklistTecnicoService.IniciarAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = checklist.Id },
                checklist);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPatch("{id:int}/guardar-avance")]
    public async Task<IActionResult> GuardarAvance(int id, GuardarChecklistTecnicoDto dto)
    {
        try
        {
            var guardado = await _checklistTecnicoService.GuardarAvanceAsync(id, dto);

            if (!guardado)
                return NotFound(new { mensaje = "El checklist técnico no existe." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPatch("{id:int}/finalizar")]
    public async Task<IActionResult> Finalizar(int id, FinalizarChecklistTecnicoDto dto)
    {
        try
        {
            var finalizado = await _checklistTecnicoService.FinalizarAsync(id, dto);

            if (!finalizado)
                return NotFound(new { mensaje = "El checklist técnico no existe." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPatch("{id:int}/cancelar")]
    public async Task<IActionResult> Cancelar(int id)
    {
        try
        {
            var cancelado = await _checklistTecnicoService.CancelarAsync(id);

            if (!cancelado)
                return NotFound(new { mensaje = "El checklist técnico no existe." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}