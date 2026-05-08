using ControlEquiposElectronicos.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControlEquiposElectronicos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogosController : ControllerBase
{
    private readonly ICatalogoService _catalogoService;

    public CatalogosController(ICatalogoService catalogoService)
    {
        _catalogoService = catalogoService;
    }

    [HttpGet("categorias-equipo")]
    public async Task<IActionResult> ObtenerCategoriasEquipo()
    {
        var categorias = await _catalogoService.ObtenerCategoriasEquipoAsync();
        return Ok(categorias);
    }

    [HttpGet("tipos-equipo")]
    public async Task<IActionResult> ObtenerTiposEquipo()
    {
        var tipos = await _catalogoService.ObtenerTiposEquipoAsync();
        return Ok(tipos);
    }

    [HttpGet("estados-equipo")]
    public async Task<IActionResult> ObtenerEstadosEquipo()
    {
        var estados = await _catalogoService.ObtenerEstadosEquipoAsync();
        return Ok(estados);
    }

    [HttpGet("ubicaciones")]
    public async Task<IActionResult> ObtenerUbicaciones()
    {
        var ubicaciones = await _catalogoService.ObtenerUbicacionesAsync();
        return Ok(ubicaciones);
    }

    [HttpGet("estados-reporte")]
    public async Task<IActionResult> ObtenerEstadosReporte()
    {
        var estados = await _catalogoService.ObtenerEstadosReporteAsync();
        return Ok(estados);
    }

    [HttpGet("tipos-mantenimiento")]
    public async Task<IActionResult> ObtenerTiposMantenimiento()
    {
        var tipos = await _catalogoService.ObtenerTiposMantenimientoAsync();
        return Ok(tipos);
    }
}