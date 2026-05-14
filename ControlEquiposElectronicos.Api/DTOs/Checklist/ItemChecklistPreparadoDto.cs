namespace ControlEquiposElectronicos.Api.DTOs.Checklist;

public class ItemChecklistPreparadoDto
{
    public int PlantillaChecklistItemId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public int Orden { get; set; }

    public bool EsObligatorio { get; set; }
}