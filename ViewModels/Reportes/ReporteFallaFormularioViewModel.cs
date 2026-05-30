using ControlEquiposElectronicos.DTOs.Catalogos;
using ControlEquiposElectronicos.DTOs.Reportes;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Reportes;

public class ReporteFallaFormularioViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IReporteFallaApiService _reporteFallaApiService;
    private readonly ICatalogoApiService _catalogoApiService;
    private readonly SesionService _sesionService;

    private List<CatalogoItemDto> _estadosReporte = new();

    private string _equipoIdTexto = string.Empty;
    public string EquipoIdTexto
    {
        get => _equipoIdTexto;
        set
        {
            _equipoIdTexto = value;
            OnPropertyChanged();
            if (int.TryParse(value, out var id))
                EquipoId = id;
        }
    }

    public int EquipoId { get; set; }

    private string _titulo = string.Empty;
    public string Titulo
    {
        get => _titulo;
        set { _titulo = value; OnPropertyChanged(); }
    }

    private string _descripcion = string.Empty;
    public string Descripcion
    {
        get => _descripcion;
        set { _descripcion = value; OnPropertyChanged(); }
    }

    private string _prioridad = "Media";
    public string Prioridad
    {
        get => _prioridad;
        set { _prioridad = value; OnPropertyChanged(); }
    }

    private string _estadoReporteNombre = "Abierto";
    public string EstadoReporteNombre
    {
        get => _estadoReporteNombre;
        set { _estadoReporteNombre = value; OnPropertyChanged(); }
    }

    public ICommand GuardarCommand { get; }
    public ICommand CancelarCommand { get; }

    public ReporteFallaFormularioViewModel(
        IReporteFallaApiService reporteFallaApiService,
        ICatalogoApiService catalogoApiService,
        SesionService sesionService)
    {
        Title = "Reporte de falla";
        _reporteFallaApiService = reporteFallaApiService;
        _catalogoApiService = catalogoApiService;
        _sesionService = sesionService;
        GuardarCommand = new Command(async () => await GuardarAsync());
        CancelarCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("equipoId", out var valor) &&
            int.TryParse(valor?.ToString(), out var id))
        {
            EquipoId = id;
            EquipoIdTexto = id.ToString();
        }
    }

    public async Task InicializarAsync()
    {
        try
        {
            _estadosReporte = await _catalogoApiService.ObtenerEstadosReporteAsync();
        }
        catch
        {
            _estadosReporte = new();
        }
    }

    private int ResolverEstadoReporteId()
    {
        var estado = _estadosReporte.FirstOrDefault(e =>
            e.Nombre.Equals(EstadoReporteNombre, StringComparison.OrdinalIgnoreCase));

        if (estado != null)
            return estado.Id;

        var abierto = _estadosReporte.FirstOrDefault(e =>
            e.Nombre.Contains("Abierto", StringComparison.OrdinalIgnoreCase));

        return abierto?.Id ?? 1;
    }

    private async Task GuardarAsync()
    {
        if (string.IsNullOrWhiteSpace(Titulo))
        {
            await Shell.Current.DisplayAlertAsync("Error", "El título es obligatorio.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Descripcion))
        {
            await Shell.Current.DisplayAlertAsync("Error", "La descripción es obligatoria.", "OK");
            return;
        }

        if (EquipoId <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Error", "Debe ingresar un ID de equipo válido.", "OK");
            return;
        }

        var usuarioId = _sesionService.UsuarioActual?.UsuarioId ?? 0;
        if (usuarioId <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Sesión",
                "No se pudo identificar al usuario. Cierra sesión e ingresa de nuevo.", "OK");
            return;
        }

        IsBusy = true;
        try
        {
            var dto = new CrearReporteFallaDto
            {
                EquipoId = EquipoId,
                UsuarioReportaId = usuarioId,
                EstadoReporteId = ResolverEstadoReporteId(),
                Titulo = Titulo.Trim(),
                Descripcion = Descripcion.Trim(),
                Prioridad = string.IsNullOrWhiteSpace(Prioridad) ? "Media" : Prioridad
            };

            var (exito, error) = await _reporteFallaApiService.CrearAsync(dto);

            if (exito)
            {
                await Shell.Current.DisplayAlertAsync("Éxito", "Reporte registrado correctamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Error", error ?? "No se pudo registrar el reporte.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
