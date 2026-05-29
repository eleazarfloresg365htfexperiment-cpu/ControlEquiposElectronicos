namespace ControlEquiposElectronicos.DTOs.Checklist;

public class CrearPlantillaChecklistItemDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int Orden { get; set; }
    public bool EsObligatorio { get; set; } = true;
}
