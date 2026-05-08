using ControlEquiposElectronicos.Api.DTOs.Equipos;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquiposController : ControllerBase
{
    private readonly IEquipoService _equipoService;

    public EquiposController(IEquipoService equipoService)
    {
        _equipoService = equipoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var equipos = await _equipoService.ObtenerTodosAsync();
        return Ok(equipos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var equipo = await _equipoService.ObtenerPorIdAsync(id);

        if (equipo == null)
            return NotFound(new { mensaje = "Equipo no encontrado." });

        return Ok(equipo);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearEquipoDto dto)
    {
        try
        {
            var equipo = await _equipoService.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = equipo.Id }, equipo);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, ActualizarEquipoDto dto)
    {
        try
        {
            var actualizado = await _equipoService.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound(new { mensaje = "Equipo no encontrado." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desactivar(int id)
    {
        var desactivado = await _equipoService.DesactivarAsync(id);

        if (!desactivado)
            return NotFound(new { mensaje = "Equipo no encontrado." });

        return NoContent();
    }
}