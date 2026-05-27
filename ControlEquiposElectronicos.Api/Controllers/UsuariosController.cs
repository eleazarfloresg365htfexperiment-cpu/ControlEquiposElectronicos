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

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Crear(CrearUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerTodos),
                new { id = usuario.UsuarioId },
                usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}