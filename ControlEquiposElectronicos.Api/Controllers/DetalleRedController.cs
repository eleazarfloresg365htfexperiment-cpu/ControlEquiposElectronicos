using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/equipos/{equipoId:int}/detalle-red")]
public class DetalleRedController : ControllerBase
{
    private readonly IDetalleRedService _detalleRedService;

    public DetalleRedController(IDetalleRedService detalleRedService)
    {
        _detalleRedService = detalleRedService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPorEquipoId(int equipoId)
    {
        var detalle = await _detalleRedService.ObtenerPorEquipoIdAsync(equipoId);

        if (detalle == null)
            return NotFound(new { mensaje = "El equipo no tiene detalle de red registrado." });

        return Ok(detalle);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(int equipoId, CrearDetalleRedDto dto)
    {
        try
        {
            var detalle = await _detalleRedService.CrearAsync(equipoId, dto);
            return CreatedAtAction(nameof(ObtenerPorEquipoId), new { equipoId }, detalle);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int equipoId, ActualizarDetalleRedDto dto)
    {
        try
        {
            var actualizado = await _detalleRedService.ActualizarAsync(equipoId, dto);

            if (!actualizado)
                return NotFound(new { mensaje = "No se encontró detalle de red para este equipo." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int equipoId)
    {
        var eliminado = await _detalleRedService.EliminarAsync(equipoId);

        if (!eliminado)
            return NotFound(new { mensaje = "No se encontró detalle de red para este equipo." });

        return NoContent();
    }
}