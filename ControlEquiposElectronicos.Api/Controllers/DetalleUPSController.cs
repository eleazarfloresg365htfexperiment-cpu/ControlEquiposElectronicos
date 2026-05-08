using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/equipos/{equipoId:int}/detalle-ups")]
public class DetalleUPSController : ControllerBase
{
    private readonly IDetalleUPSService _detalleUPSService;

    public DetalleUPSController(IDetalleUPSService detalleUPSService)
    {
        _detalleUPSService = detalleUPSService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPorEquipoId(int equipoId)
    {
        var detalle = await _detalleUPSService.ObtenerPorEquipoIdAsync(equipoId);

        if (detalle == null)
            return NotFound(new { mensaje = "El equipo no tiene detalle de UPS registrado." });

        return Ok(detalle);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(int equipoId, CrearDetalleUPSDto dto)
    {
        try
        {
            var detalle = await _detalleUPSService.CrearAsync(equipoId, dto);
            return CreatedAtAction(nameof(ObtenerPorEquipoId), new { equipoId }, detalle);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int equipoId, ActualizarDetalleUPSDto dto)
    {
        var actualizado = await _detalleUPSService.ActualizarAsync(equipoId, dto);

        if (!actualizado)
            return NotFound(new { mensaje = "No se encontró detalle de UPS para este equipo." });

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int equipoId)
    {
        var eliminado = await _detalleUPSService.EliminarAsync(equipoId);

        if (!eliminado)
            return NotFound(new { mensaje = "No se encontró detalle de UPS para este equipo." });

        return NoContent();
    }
}