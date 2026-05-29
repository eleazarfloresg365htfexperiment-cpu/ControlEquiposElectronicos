namespace ControlEquiposElectronicos.DTOs.Equipos;

public class AsignarPerifericoDto
{
    public int EquipoPrincipalId { get; set; }
    public int PerifericoId { get; set; }
    public string Observaciones { get; set; } = string.Empty;
}