using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ControlEquiposElectronicos.DTOs.Equipos;

public class EquipoListadoDto : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int CategoriaEquipoId { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public int TipoEquipoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int EstadoEquipoId { get; set; }

    private string _estado = string.Empty;
    public string Estado
    {
        get => _estado;
        set
        {
            _estado = value;
            OnPropertyChanged();
        }
    }

    public int UbicacionId { get; set; }
    public string Ubicacion { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string NumeroSerie { get; set; } = string.Empty;
    public DateTime? FechaCompra { get; set; }
    public DateTime? FechaInstalacion { get; set; }
    public string? Observaciones { get; set; }
    public bool Activo { get; set; }
}