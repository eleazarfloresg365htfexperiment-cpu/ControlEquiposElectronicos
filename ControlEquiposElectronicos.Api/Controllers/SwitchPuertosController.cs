using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/switches/{switchEquipoId:int}/puertos")]
public class SwitchPuertosController : ControllerBase
{
    private readonly ISwitchPuertoService _switchPuertoService;

    public SwitchPuertosController(ISwitchPuertoService switchPuertoService)
    {
        _switchPuertoService = switchPuertoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPorSwitch(int switchEquipoId)
    {
        var puertos = await _switchPuertoService.ObtenerPorSwitchAsync(switchEquipoId);
        return Ok(puertos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var puerto = await _switchPuertoService.ObtenerPorIdAsync(id);

        if (puerto == null)
            return NotFound(new { mensaje = "Puerto no encontrado." });

        return Ok(puerto);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(int switchEquipoId, CrearSwitchPuertoDto dto)
    {
        try
        {
            var puerto = await _switchPuertoService.CrearAsync(switchEquipoId, dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { switchEquipoId, id = puerto.Id }, puerto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, ActualizarSwitchPuertoDto dto)
    {
        try
        {
            var actualizado = await _switchPuertoService.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound(new { mensaje = "Puerto no encontrado." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _switchPuertoService.EliminarAsync(id);

        if (!eliminado)
            return NotFound(new { mensaje = "Puerto no encontrado." });

        return NoContent();
    }
}