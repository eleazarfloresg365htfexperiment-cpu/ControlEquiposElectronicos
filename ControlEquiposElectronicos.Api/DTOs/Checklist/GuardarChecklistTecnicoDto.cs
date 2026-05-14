using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Checklist;

public class GuardarChecklistTecnicoDto
{
    [MaxLength(1000)]
    public string? ObservacionesGenerales { get; set; }

    public List<GuardarChecklistEquipoDto> Equipos { get; set; } = new();
}