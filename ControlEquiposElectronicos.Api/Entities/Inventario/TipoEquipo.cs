namespace ControlEquiposElectronicos.Api.Entities.Inventario;

public class TipoEquipo
{
    public int Id { get; set; }

    public int CategoriaEquipoId { get; set; }
    public CategoriaEquipo CategoriaEquipo { get; set; } = null!;

    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;

    public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
}