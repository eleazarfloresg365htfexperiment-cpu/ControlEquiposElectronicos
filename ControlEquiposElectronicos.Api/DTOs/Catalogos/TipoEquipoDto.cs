namespace ControlEquiposElectronicos.Api.DTOs.Catalogos;

public class TipoEquipoDto
{
    public int Id { get; set; }
    public int CategoriaEquipoId { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
}