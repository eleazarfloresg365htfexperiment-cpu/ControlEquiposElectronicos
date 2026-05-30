using ControlEquiposElectronicos.DTOs.Catalogos;
using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.DTOs.Reportes;
using ControlEquiposElectronicos.Services;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public abstract class EquipoSubmoduloViewModelBase : BaseViewModel
{
    private readonly IEquipoApiService _equipoApiService;
    private readonly IReporteFallaApiService? _reporteFallaApiService;
    private readonly ICatalogoApiService? _catalogoApiService;
    private readonly SesionService? _sesionService;

    protected EquipoSubmoduloViewModelBase(
        IEquipoApiService equipoApiService,
        IReporteFallaApiService? reporteFallaApiService = null,
        SesionService? sesionService = null,
        ICatalogoApiService? catalogoApiService = null)
    {
        _equipoApiService = equipoApiService;
        _reporteFallaApiService = reporteFallaApiService;
        _sesionService = sesionService;
        _catalogoApiService = catalogoApiService;

        ActualizarCommand = new Command(async () => await CargarAsync());
        BuscarCommand = new Command(async () => await CargarAsync());
        VerDetalleCommand = new Command<EquipoListadoDto>(async e => await VerDetalleAsync(e));
        ReportarFallaCommand = new Command<EquipoListadoDto>(async e => await ReportarFallaAsync(e));
    }

    protected abstract IReadOnlyList<string> TiposEquipo { get; }

    public ObservableCollection<EquipoListadoDto> Equipos { get; } = new();

    private string _busquedaTexto = string.Empty;
    public string BusquedaTexto
    {
        get => _busquedaTexto;
        set { _busquedaTexto = value; OnPropertyChanged(); }
    }

    public bool SinEquipos => Equipos.Count == 0 && !IsBusy;

    public ICommand ActualizarCommand { get; }
    public ICommand BuscarCommand { get; }
    public ICommand VerDetalleCommand { get; }
    public ICommand ReportarFallaCommand { get; }

    protected ICommand CrearRegistrarCommand(string tipoEquipo) =>
        new Command(async () => await Shell.Current.GoToAsync($"RegistrarEquipoPage?tipo={Uri.EscapeDataString(tipoEquipo)}"));

    public async Task CargarAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var todos = await _equipoApiService.ObtenerTodosAsync();
            var filtrados = todos
                .Where(e => TiposEquipo.Contains(e.Tipo, StringComparer.OrdinalIgnoreCase))
                .AsEnumerable();

            if (!string.IsNullOrWhiteSpace(BusquedaTexto))
            {
                filtrados = filtrados.Where(e =>
                    e.Nombre.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                    e.Codigo.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                    e.Ubicacion.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase) ||
                    e.Tipo.Contains(BusquedaTexto, StringComparison.OrdinalIgnoreCase));
            }

            Equipos.Clear();
            foreach (var equipo in filtrados)
                Equipos.Add(equipo);

            ActualizarContadores();
            OnPropertyChanged(nameof(SinEquipos));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudieron cargar los equipos.\n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(SinEquipos));
        }
    }

    protected virtual void ActualizarContadores() { }

    protected void NotificarContadores()
    {
        ActualizarContadores();
        OnPropertyChanged(nameof(SinEquipos));
    }

    protected static async Task VerDetalleAsync(EquipoListadoDto? equipo)
    {
        if (equipo == null) return;
        await Shell.Current.GoToAsync($"DetalleEquipoPage?equipoId={equipo.Id}");
    }

    private async Task ReportarFallaAsync(EquipoListadoDto? equipo)
    {
        if (equipo == null) return;

        if (_reporteFallaApiService == null || _sesionService?.UsuarioActual == null)
        {
            await Shell.Current.GoToAsync($"ReporteFallaFormulario?equipoId={equipo.Id}");
            return;
        }

        var usuarioId = _sesionService.UsuarioActual.UsuarioId;
        if (usuarioId <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Sesión", "No se pudo identificar al usuario actual.", "OK");
            return;
        }

        var descripcion = await Shell.Current.DisplayPromptAsync(
            "Reportar falla",
            $"Describe la falla de {equipo.Nombre}:",
            "Enviar",
            "Cancelar",
            maxLength: 500);

        if (string.IsNullOrWhiteSpace(descripcion)) return;

        IsBusy = true;
        try
        {
            var estadoId = await ObtenerEstadoAbiertoIdAsync();

            var dto = new CrearReporteFallaDto
            {
                EquipoId = equipo.Id,
                UsuarioReportaId = usuarioId,
                EstadoReporteId = estadoId,
                Titulo = $"Falla en {equipo.Codigo}",
                Descripcion = descripcion,
                Prioridad = "Media"
            };

            var (exito, error) = await _reporteFallaApiService.CrearAsync(dto);
            if (exito)
                await Shell.Current.DisplayAlertAsync("Reporte", "Falla registrada correctamente.", "OK");
            else
                await Shell.Current.DisplayAlertAsync("Error", error ?? "No se pudo registrar la falla.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task<int> ObtenerEstadoAbiertoIdAsync()
    {
        if (_catalogoApiService == null)
            return 1;

        try
        {
            var estados = await _catalogoApiService.ObtenerEstadosReporteAsync();
            var abierto = estados.FirstOrDefault(e =>
                e.Nombre.Contains("Abierto", StringComparison.OrdinalIgnoreCase));
            return abierto?.Id ?? 1;
        }
        catch
        {
            return 1;
        }
    }
}
