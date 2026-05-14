using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Checklist;

public class FinalizarChecklistTecnicoDto
{
    [MaxLength(1000)]
    public string? ObservacionesGenerales { get; set; }

    public bool CrearReportesFallaAutomaticos { get; set; } = false;
}