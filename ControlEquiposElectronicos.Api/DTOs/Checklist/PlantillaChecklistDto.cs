namespace ControlEquiposElectronicos.Api.DTOs.Checklist;

public class PlantillaChecklistDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public int? CategoriaEquipoId { get; set; }

    public string? CategoriaEquipo { get; set; }

    public int TipoEquipoId { get; set; }

    public string TipoEquipo { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public List<PlantillaChecklistItemDto> Items { get; set; } = new();
}
