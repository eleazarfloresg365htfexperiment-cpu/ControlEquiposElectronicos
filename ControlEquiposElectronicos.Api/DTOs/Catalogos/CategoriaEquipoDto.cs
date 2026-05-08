namespace ControlEquiposElectronicos.Api.DTOs.Catalogos;

public class CategoriaEquipoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}