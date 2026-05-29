using ControlEquiposElectronicos.Api.Entities.Auditoria;

namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class EquipoPeriferico
{
    public int Id { get; set; }

    public int EquipoPrincipalId { get; set; }
    public Equipo EquipoPrincipal { get; set; } = null!;

    public int PerifericoId { get; set; }
    public Equipo Periferico { get; set; } = null!;

    public DateTime FechaAsignacion { get; set; }

    public DateTime? FechaDesasignacion { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; } = true;
}