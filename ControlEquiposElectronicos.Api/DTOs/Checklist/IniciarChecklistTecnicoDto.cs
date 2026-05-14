using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.Checklist;

public class IniciarChecklistTecnicoDto
{
    [Required]
    public int UbicacionId { get; set; }

    [Required]
    public int TecnicoId { get; set; }

    [MaxLength(1000)]
    public string? ObservacionesGenerales { get; set; }
}