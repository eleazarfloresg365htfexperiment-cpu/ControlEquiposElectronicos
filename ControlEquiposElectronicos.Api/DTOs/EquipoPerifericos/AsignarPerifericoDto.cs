using System.ComponentModel.DataAnnotations;

namespace ControlEquiposElectronicos.Api.DTOs.EquipoPerifericos;

public class AsignarPerifericoDto
{
    [Required]
    public int EquipoPrincipalId { get; set; }

    [Required]
    public int PerifericoId { get; set; }

    [MaxLength(300)]
    public string? Observaciones { get; set; }
}