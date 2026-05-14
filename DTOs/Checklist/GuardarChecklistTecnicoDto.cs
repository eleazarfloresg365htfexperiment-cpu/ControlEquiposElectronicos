namespace ControlEquiposElectronicos.DTOs.Checklist;

public class GuardarChecklistTecnicoDto
{
    public string? ObservacionesGenerales { get; set; }
    public List<GuardarChecklistEquipoDto> Equipos { get; set; } = new();
}
