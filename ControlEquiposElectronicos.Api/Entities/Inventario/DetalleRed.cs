namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class DetalleRed
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public string? DireccionIP { get; set; }
    public string? MacAddress { get; set; }

    public int? CantidadPuertos { get; set; }
    public int? PuertosDanados { get; set; }

    public int? EquipoProveedorId { get; set; }
    public Equipo? EquipoProveedor { get; set; }

    public string? ObservacionesRed { get; set; }
}
