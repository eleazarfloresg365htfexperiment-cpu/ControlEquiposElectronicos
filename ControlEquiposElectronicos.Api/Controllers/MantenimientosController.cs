using ControlEquiposElectronicos.Api.DTOs.Mantenimientos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MantenimientosController : ControllerBase
{
    private readonly IMantenimientoService _mantenimientoService;

    public MantenimientosController(IMantenimientoService mantenimientoService)
    {
        _mantenimientoService = mantenimientoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var mantenimientos = await _mantenimientoService.ObtenerTodosAsync();
        return Ok(mantenimientos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var mantenimiento = await _mantenimientoService.ObtenerPorIdAsync(id);

        if (mantenimiento == null)
            return NotFound(new { mensaje = "Mantenimiento no encontrado." });

        return Ok(mantenimiento);
    }

    [HttpGet("equipo/{equipoId:int}")]
    public async Task<IActionResult> ObtenerPorEquipo(int equipoId)
    {
        var mantenimientos = await _mantenimientoService.ObtenerPorEquipoAsync(equipoId);
        return Ok(mantenimientos);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearMantenimientoDto dto)
    {
        try
        {
            var mantenimiento = await _mantenimientoService.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = mantenimiento.Id }, mantenimiento);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, ActualizarMantenimientoDto dto)
    {
        try
        {
            var actualizado = await _mantenimientoService.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound(new { mensaje = "Mantenimiento no encontrado." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPatch("{id:int}/finalizar")]
    public async Task<IActionResult> Finalizar(int id, FinalizarMantenimientoDto dto)
    {
        var finalizado = await _mantenimientoService.FinalizarAsync(id, dto);

        if (!finalizado)
            return NotFound(new { mensaje = "Mantenimiento no encontrado." });

        return NoContent();
    }

    [HttpPost("{mantenimientoId:int}/repuestos")]
    public async Task<IActionResult> AgregarRepuesto(int mantenimientoId, CrearMantenimientoRepuestoDto dto)
    {
        try
        {
            var repuesto = await _mantenimientoService.AgregarRepuestoAsync(mantenimientoId, dto);
            return Created(string.Empty, repuesto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("repuestos/{repuestoId:int}")]
    public async Task<IActionResult> EliminarRepuesto(int repuestoId)
    {
        var eliminado = await _mantenimientoService.EliminarRepuestoAsync(repuestoId);

        if (!eliminado)
            return NotFound(new { mensaje = "Repuesto no encontrado." });

        return NoContent();
    }
}
