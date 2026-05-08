namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class DetalleRedDto
{
    public int Id { get; set; }

    public int EquipoId { get; set; }
    public string CodigoEquipo { get; set; } = string.Empty;
    public string NombreEquipo { get; set; } = string.Empty;

    public string? DireccionIP { get; set; }
    public string? MacAddress { get; set; }

    public int? CantidadPuertos { get; set; }
    public int? PuertosDanados { get; set; }

    public int? EquipoProveedorId { get; set; }
    public string? CodigoEquipoProveedor { get; set; }
    public string? NombreEquipoProveedor { get; set; }

    public string? ObservacionesRed { get; set; }
}
