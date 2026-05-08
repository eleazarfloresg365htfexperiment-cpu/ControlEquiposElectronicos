using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/equipos/{equipoId:int}/detalle-computadora")]
public class DetalleComputadoraController : ControllerBase
{
    private readonly IDetalleComputadoraService _detalleComputadoraService;

    public DetalleComputadoraController(IDetalleComputadoraService detalleComputadoraService)
    {
        _detalleComputadoraService = detalleComputadoraService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPorEquipoId(int equipoId)
    {
        var detalle = await _detalleComputadoraService.ObtenerPorEquipoIdAsync(equipoId);

        if (detalle == null)
            return NotFound(new { mensaje = "El equipo no tiene detalle de computadora registrado." });

        return Ok(detalle);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(int equipoId, CrearDetalleComputadoraDto dto)
    {
        try
        {
            var detalle = await _detalleComputadoraService.CrearAsync(equipoId, dto);
            return CreatedAtAction(nameof(ObtenerPorEquipoId), new { equipoId }, detalle);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int equipoId, ActualizarDetalleComputadoraDto dto)
    {
        var actualizado = await _detalleComputadoraService.ActualizarAsync(equipoId, dto);

        if (!actualizado)
            return NotFound(new { mensaje = "No se encontró detalle de computadora para este equipo." });

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int equipoId)
    {
        var eliminado = await _detalleComputadoraService.EliminarAsync(equipoId);

        if (!eliminado)
            return NotFound(new { mensaje = "No se encontró detalle de computadora para este equipo." });

        return NoContent();
    }
}