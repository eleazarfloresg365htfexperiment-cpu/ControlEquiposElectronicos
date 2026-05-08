namespace ControlEquiposElectronicos.Api.DTOs.Mantenimientos;

public class MantenimientoRepuestoDto
{
    public int Id { get; set; }

    public int MantenimientoId { get; set; }

    public string NombreRepuesto { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public int Cantidad { get; set; }

    public decimal? CostoUnitario { get; set; }

    public string? NumeroSerieAnterior { get; set; }
    public string? NumeroSerieNuevo { get; set; }

    public DateTime FechaRegistro { get; set; }
}