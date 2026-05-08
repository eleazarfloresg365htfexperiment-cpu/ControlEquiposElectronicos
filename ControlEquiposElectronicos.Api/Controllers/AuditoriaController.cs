using ControlEquiposElectronicos.Api.DTOs.Auditoria;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var historial = await _auditoriaService.ObtenerTodosAsync();
        return Ok(historial);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var historial = await _auditoriaService.ObtenerPorIdAsync(id);

        if (historial == null)
            return NotFound(new { mensaje = "Registro de auditoría no encontrado." });

        return Ok(historial);
    }

    [HttpGet("usuario/{usuarioId:int}")]
    public async Task<IActionResult> ObtenerPorUsuario(int usuarioId)
    {
        var historial = await _auditoriaService.ObtenerPorUsuarioAsync(usuarioId);
        return Ok(historial);
    }

    [HttpGet("modulo/{modulo}")]
    public async Task<IActionResult> ObtenerPorModulo(string modulo)
    {
        var historial = await _auditoriaService.ObtenerPorModuloAsync(modulo);
        return Ok(historial);
    }

    [HttpPost]
    public async Task<IActionResult> Registrar(CrearHistorialOperacionDto dto)
    {
        try
        {
            var historial = await _auditoriaService.RegistrarAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = historial.Id }, historial);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}