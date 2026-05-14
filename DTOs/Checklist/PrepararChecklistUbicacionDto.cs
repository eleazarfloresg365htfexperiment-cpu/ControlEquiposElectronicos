namespace ControlEquiposElectronicos.DTOs.Checklist;

public class PrepararChecklistUbicacionDto
{
    public int UbicacionId { get; set; }
    public string Ubicacion { get; set; } = string.Empty;
    public int TotalEquipos { get; set; }

    public List<EquipoChecklistPreparadoDto> Equipos { get; set; } = new();
}
