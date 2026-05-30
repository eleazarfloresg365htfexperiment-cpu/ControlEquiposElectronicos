namespace ControlEquiposElectronicos.DTOs.Checklist;

public class EquipoChecklistPreparadoDto
{
    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public int TipoEquipoId { get; set; }
    public string TipoEquipo { get; set; } = string.Empty;

    public int? PlantillaChecklistId { get; set; }
    public string? PlantillaChecklist { get; set; }
    public bool TienePlantilla { get; set; }

    public List<ItemChecklistPreparadoDto> Items { get; set; } = new();

    /// <summary>Coincide con la regla de la API al iniciar checklist (plantilla + aspectos activos).</summary>
    public bool ListoParaIniciar => TienePlantilla && Items.Count > 0;

    public bool MostrarSinAspectos => TienePlantilla && !ListoParaIniciar;

    public bool MostrarSinPlantilla => !TienePlantilla;
}
