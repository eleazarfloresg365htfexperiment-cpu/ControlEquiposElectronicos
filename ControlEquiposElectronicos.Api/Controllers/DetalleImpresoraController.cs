using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/equipos/{equipoId:int}/detalle-impresora")]
public class DetalleImpresoraController : ControllerBase
{
    private readonly IDetalleImpresoraService _detalleImpresoraService;

    public DetalleImpresoraController(IDetalleImpresoraService detalleImpresoraService)
    {
        _detalleImpresoraService = detalleImpresoraService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPorEquipoId(int equipoId)
    {
        var detalle = await _detalleImpresoraService.ObtenerPorEquipoIdAsync(equipoId);

        if (detalle == null)
            return NotFound(new { mensaje = "El equipo no tiene detalle de impresora registrado." });

        return Ok(detalle);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(int equipoId, CrearDetalleImpresoraDto dto)
    {
        try
        {
            var detalle = await _detalleImpresoraService.CrearAsync(equipoId, dto);
            return CreatedAtAction(nameof(ObtenerPorEquipoId), new { equipoId }, detalle);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int equipoId, ActualizarDetalleImpresoraDto dto)
    {
        var actualizado = await _detalleImpresoraService.ActualizarAsync(equipoId, dto);

        if (!actualizado)
            return NotFound(new { mensaje = "No se encontró detalle de impresora para este equipo." });

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int equipoId)
    {
        var eliminado = await _detalleImpresoraService.EliminarAsync(equipoId);

        if (!eliminado)
            return NotFound(new { mensaje = "No se encontró detalle de impresora para este equipo." });

        return NoContent();
    }
}