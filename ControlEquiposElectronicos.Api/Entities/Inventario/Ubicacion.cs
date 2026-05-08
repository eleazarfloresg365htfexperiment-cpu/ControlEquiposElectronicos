namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class Ubicacion
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string TipoUbicacion { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
}