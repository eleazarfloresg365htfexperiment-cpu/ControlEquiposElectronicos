using ControlEquiposElectronicos.Api.Entities.Inventario;

namespace ControlEquiposElectronicos.Api.Entities.Mantenimientos;

public class MantenimientoRepuesto
{
    public int Id { get; set; }

    public int MantenimientoId { get; set; }
    public Mantenimiento Mantenimiento { get; set; } = null!;

    public string NombreRepuesto { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public int Cantidad { get; set; } = 1;
    public decimal? CostoUnitario { get; set; }

    public string? NumeroSerieAnterior { get; set; }
    public string? NumeroSerieNuevo { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}