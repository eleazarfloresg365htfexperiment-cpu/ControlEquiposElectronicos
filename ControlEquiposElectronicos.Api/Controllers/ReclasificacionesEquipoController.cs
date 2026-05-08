using ControlEquiposElectronicos.Api.DTOs.Reclasificaciones;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReclasificacionesEquipoController : ControllerBase
{
    private readonly IReclasificacionEquipoService _reclasificacionEquipoService;

    public ReclasificacionesEquipoController(IReclasificacionEquipoService reclasificacionEquipoService)
    {
        _reclasificacionEquipoService = reclasificacionEquipoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodas()
    {
        var reclasificaciones = await _reclasificacionEquipoService.ObtenerTodasAsync();
        return Ok(reclasificaciones);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var reclasificacion = await _reclasificacionEquipoService.ObtenerPorIdAsync(id);

        if (reclasificacion == null)
            return NotFound(new { mensaje = "Reclasificación no encontrada." });

        return Ok(reclasificacion);
    }

    [HttpGet("equipo/{equipoId:int}")]
    public async Task<IActionResult> ObtenerPorEquipo(int equipoId)
    {
        var reclasificaciones = await _reclasificacionEquipoService.ObtenerPorEquipoAsync(equipoId);
        return Ok(reclasificaciones);
    }

    [HttpGet("equipo/{equipoId:int}/historial-estados")]
    public async Task<IActionResult> ObtenerHistorialEstadosPorEquipo(int equipoId)
    {
        var historial = await _reclasificacionEquipoService.ObtenerHistorialEstadosPorEquipoAsync(equipoId);
        return Ok(historial);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearReclasificacionEquipoDto dto)
    {
        try
        {
            var reclasificacion = await _reclasificacionEquipoService.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = reclasificacion.Id }, reclasificacion);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}