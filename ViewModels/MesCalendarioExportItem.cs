using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ControlEquiposElectronicos.ViewModels;

public class MesCalendarioExportItem : INotifyPropertyChanged
{
    private static readonly string[] NombresCortos =
        ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"];

    private bool _seleccionado;

    public MesCalendarioExportItem(int anio, int mes, int cantidadReportes, bool seleccionado = false)
    {
        Anio = anio;
        Mes = mes;
        CantidadReportes = cantidadReportes;
        NombreCorto = NombresCortos[mes - 1];
        _seleccionado = seleccionado;
    }

    public int Anio { get; }
    public int Mes { get; }
    public string NombreCorto { get; }
    public int CantidadReportes { get; }

    public string CantidadTexto => CantidadReportes > 0 ? $"{CantidadReportes} rep." : "—";

    public bool Seleccionado
    {
        get => _seleccionado;
        set
        {
            if (_seleccionado == value) return;
            _seleccionado = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ColorCelda));
            OnPropertyChanged(nameof(ColorTexto));
            OnPropertyChanged(nameof(ColorBorde));
        }
    }

    public string ColorCelda => Seleccionado ? "#4F8EF7" : (CantidadReportes > 0 ? "#EBF4FF" : "#FAFAFA");
    public string ColorTexto => Seleccionado ? "#FFFFFF" : "#1A1A2E";
    public string ColorBorde => Seleccionado ? "#4F8EF7" : (CantidadReportes > 0 ? "#93C5FD" : "#E0E0E0");

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
