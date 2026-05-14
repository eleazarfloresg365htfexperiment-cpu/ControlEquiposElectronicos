using ControlEquiposElectronicos.Api.Entities.Inventario;

namespace ControlEquiposElectronicos.Api.Entities.Checklist;

public class PlantillaChecklist
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public int? CategoriaEquipoId { get; set; }

    public CategoriaEquipo? CategoriaEquipo { get; set; }

    public int TipoEquipoId { get; set; }

    public TipoEquipo TipoEquipo { get; set; } = null!;

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaActualizacion { get; set; }

    public ICollection<PlantillaChecklistItem> Items { get; set; } = new List<PlantillaChecklistItem>();

    public ICollection<ChecklistTecnicoEquipo> ChecklistTecnicoEquipos { get; set; } = new List<ChecklistTecnicoEquipo>();
}