namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class CategoriaEquipo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    // public ICollection<TipoEquipo> TiposEquipo { get; set; } = new List<TipoEquipo>();

    public ICollection<TipoEquipo> TiposEquipo { get; set; } = new List<TipoEquipo>();
    public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
}
