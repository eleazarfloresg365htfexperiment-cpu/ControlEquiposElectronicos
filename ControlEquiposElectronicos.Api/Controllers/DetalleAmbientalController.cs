using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/equipos/{equipoId:int}/detalle-ambiental")]
public class DetalleAmbientalController : ControllerBase
{
    private readonly IDetalleAmbientalService _detalleAmbientalService;

    public DetalleAmbientalController(IDetalleAmbientalService detalleAmbientalService)
    {
        _detalleAmbientalService = detalleAmbientalService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPorEquipoId(int equipoId)
    {
        var detalle = await _detalleAmbientalService.ObtenerPorEquipoIdAsync(equipoId);

        if (detalle == null)
            return NotFound(new { mensaje = "El equipo no tiene detalle ambiental registrado." });

        return Ok(detalle);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(int equipoId, CrearDetalleAmbientalDto dto)
    {
        try
        {
            var detalle = await _detalleAmbientalService.CrearAsync(equipoId, dto);
            return CreatedAtAction(nameof(ObtenerPorEquipoId), new { equipoId }, detalle);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int equipoId, ActualizarDetalleAmbientalDto dto)
    {
        var actualizado = await _detalleAmbientalService.ActualizarAsync(equipoId, dto);

        if (!actualizado)
            return NotFound(new { mensaje = "No se encontró detalle ambiental para este equipo." });

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int equipoId)
    {
        var eliminado = await _detalleAmbientalService.EliminarAsync(equipoId);

        if (!eliminado)
            return NotFound(new { mensaje = "No se encontró detalle ambiental para este equipo." });

        return NoContent();
    }
}