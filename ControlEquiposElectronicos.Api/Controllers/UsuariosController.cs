using ControlEquiposElectronicos.Api.DTOs.Usuarios;
using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UsuarioDto>>> ObtenerTodos()
    {
        var usuarios = await _usuarioService.ObtenerTodosAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioDto>> ObtenerPorId(int id)
    {
        var usuario = await _usuarioService.ObtenerPorIdAsync(id);

        if (usuario == null)
            return NotFound(new { mensaje = "El usuario no existe." });

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Crear(CrearUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = usuario.UsuarioId },
                usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPatch("{id:int}/rol")]
    public async Task<ActionResult<UsuarioDto>> CambiarRol(int id, ActualizarRolUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioService.CambiarRolAsync(id, dto);

            if (usuario == null)
                return NotFound(new { mensaje = "El usuario no existe." });

            return Ok(usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}