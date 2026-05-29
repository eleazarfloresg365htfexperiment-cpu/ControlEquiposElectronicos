namespace ControlEquiposElectronicos.DTOs.Checklist;

public class CrearPlantillaChecklistDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int? CategoriaEquipoId { get; set; }
    public int TipoEquipoId { get; set; }
}
