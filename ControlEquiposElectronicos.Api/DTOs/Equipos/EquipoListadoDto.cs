namespace ControlEquiposElectronicos.Api.DTOs.Equipos;

public class EquipoListadoDto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }

    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? NumeroSerie { get; set; }

    public bool Activo { get; set; }
}