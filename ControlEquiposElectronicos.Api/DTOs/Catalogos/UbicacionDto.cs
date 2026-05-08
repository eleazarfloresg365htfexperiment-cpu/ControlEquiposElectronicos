namespace ControlEquiposElectronicos.Api.DTOs.Catalogos;

public class UbicacionDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string TipoUbicacion { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
}