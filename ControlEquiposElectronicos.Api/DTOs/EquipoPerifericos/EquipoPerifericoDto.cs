namespace ControlEquiposElectronicos.Api.DTOs.EquipoPerifericos;

public class EquipoPerifericoDto
{
    public int Id { get; set; }

    public int EquipoPrincipalId { get; set; }
    public string CodigoEquipoPrincipal { get; set; } = string.Empty;
    public string NombreEquipoPrincipal { get; set; } = string.Empty;

    public int PerifericoId { get; set; }
    public string CodigoPeriferico { get; set; } = string.Empty;
    public string NombrePeriferico { get; set; } = string.Empty;

    public DateTime FechaAsignacion { get; set; }

    public DateTime? FechaDesasignacion { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; }
}