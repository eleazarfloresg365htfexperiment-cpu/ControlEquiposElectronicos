namespace ControlEquiposElectronicos.DTOs.Checklist;

public class PlantillaChecklistItemDto
{
    public int Id { get; set; }
    public int PlantillaChecklistId { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public int Orden { get; set; }
    public bool EsObligatorio { get; set; }
    public bool Activo { get; set; }
}
