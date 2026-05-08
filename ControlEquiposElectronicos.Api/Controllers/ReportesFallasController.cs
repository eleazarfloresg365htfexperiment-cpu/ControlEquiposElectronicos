using ControlEquiposElectronicos.Api.DTOs.ReportesFallas;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesFallaController : ControllerBase
{
    private readonly IReporteFallaService _reporteFallaService;

    public ReportesFallaController(IReporteFallaService reporteFallaService)
    {
        _reporteFallaService = reporteFallaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var reportes = await _reporteFallaService.ObtenerTodosAsync();
        return Ok(reportes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var reporte = await _reporteFallaService.ObtenerPorIdAsync(id);

        if (reporte == null)
            return NotFound(new { mensaje = "Reporte de falla no encontrado." });

        return Ok(reporte);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearReporteFallaDto dto)
    {
        try
        {
            var reporte = await _reporteFallaService.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = reporte.Id }, reporte);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, ActualizarReporteFallaDto dto)
    {
        var actualizado = await _reporteFallaService.ActualizarAsync(id, dto);

        if (!actualizado)
            return NotFound(new { mensaje = "Reporte de falla no encontrado." });

        return NoContent();
    }

    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, CambiarEstadoReporteFallaDto dto)
    {
        try
        {
            var actualizado = await _reporteFallaService.CambiarEstadoAsync(id, dto);

            if (!actualizado)
                return NotFound(new { mensaje = "Reporte de falla no encontrado." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
