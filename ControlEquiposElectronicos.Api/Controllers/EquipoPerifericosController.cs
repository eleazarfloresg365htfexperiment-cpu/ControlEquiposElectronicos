using ControlEquiposElectronicos.Api.DTOs.EquipoPerifericos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipoPerifericosController : ControllerBase
{
    private readonly IEquipoPerifericoService _equipoPerifericoService;

    public EquipoPerifericosController(IEquipoPerifericoService equipoPerifericoService)
    {
        _equipoPerifericoService = equipoPerifericoService;
    }

    [HttpGet("pc/{equipoPrincipalId:int}")]
    public async Task<ActionResult<List<EquipoPerifericoDto>>> ObtenerPorEquipoPrincipal(int equipoPrincipalId)
    {
        var perifericos = await _equipoPerifericoService.ObtenerPorEquipoPrincipalAsync(equipoPrincipalId);

        return Ok(perifericos);
    }

    [HttpGet("periferico/{perifericoId:int}")]
    public async Task<ActionResult<List<EquipoPerifericoDto>>> ObtenerPorPeriferico(int perifericoId)
    {
        var asignaciones = await _equipoPerifericoService.ObtenerPorPerifericoAsync(perifericoId);

        return Ok(asignaciones);
    }

    [HttpPost("asignar")]
    public async Task<ActionResult<EquipoPerifericoDto>> Asignar(AsignarPerifericoDto dto)
    {
        try
        {
            var asignacion = await _equipoPerifericoService.AsignarAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorEquipoPrincipal),
                new { equipoPrincipalId = asignacion.EquipoPrincipalId },
                asignacion);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPatch("{id:int}/quitar")]
    public async Task<IActionResult> QuitarAsignacion(int id)
    {
        var resultado = await _equipoPerifericoService.QuitarAsignacionAsync(id);

        if (!resultado)
            return NotFound(new { mensaje = "La asignación no existe o ya fue desactivada." });

        return NoContent();
    }
}