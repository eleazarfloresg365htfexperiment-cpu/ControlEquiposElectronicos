using ControlEquiposElectronicos.DTOs.Auditoria;
using ControlEquiposElectronicos.DTOs.Mantenimientos;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels;

public class MantenimientosViewModel : BaseViewModel
{
    private readonly IMantenimientoApiService _mantenimientoApiService;
    private readonly IAuditoriaApiService _auditoriaApiService;

    public ObservableCollection<MantenimientoListadoDto> Mantenimientos { get; set; } = new();
    public ObservableCollection<AuditoriaDto> Historial { get; set; } = new();

    private bool _mostrarHistorial = false;
    public bool MostrarHistorial
    {
        get => _mostrarHistorial;
        set { _mostrarHistorial = value; OnPropertyChanged(); }
    }

    private string _busqueda = string.Empty;
    public string Busqueda
    {
        get => _busqueda;
        set { _busqueda = value; OnPropertyChanged(); FiltrarMantenimientos(); }
    }

    private List<MantenimientoListadoDto> _todosLosMantenimientos = new();

    public ICommand EliminarCommand { get; }
    public ICommand RegistrarNuevoCommand { get; }
    public ICommand EditarCommand { get; }
    public ICommand FinalizarCommand { get; }
    public ICommand HistorialCommand { get; }
    public ICommand CerrarHistorialCommand { get; }

    public MantenimientosViewModel(IMantenimientoApiService mantenimientoApiService, IAuditoriaApiService auditoriaApiService)
    {
        Title = "Mantenimientos";
        _mantenimientoApiService = mantenimientoApiService;
        _auditoriaApiService = auditoriaApiService;

        EliminarCommand = new Command<MantenimientoListadoDto>(async (m) => await EliminarAsync(m));
        RegistrarNuevoCommand = new Command(async () => await Shell.Current.GoToAsync("MantenimientoFormulario"));
        EditarCommand = new Command<MantenimientoListadoDto>(async (m) => await EditarAsync(m));
        FinalizarCommand = new Command<MantenimientoListadoDto>(async (m) => await FinalizarAsync(m));
        HistorialCommand = new Command(async () => await CargarHistorialAsync());
        CerrarHistorialCommand = new Command(() => MostrarHistorial = false);
    }

    public async Task CargarMantenimientosAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var lista = await _mantenimientoApiService.ObtenerTodosAsync();
            _todosLosMantenimientos = lista;
            Mantenimientos.Clear();
            foreach (var m in lista)
                Mantenimientos.Add(m);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally { IsBusy = false; }
    }

    private async Task CargarHistorialAsync()
    {
        IsBusy = true;
        try
        {
            var lista = await _auditoriaApiService.ObtenerPorModuloAsync("Mantenimientos");
            Historial.Clear();
            foreach (var h in lista)
                Historial.Add(h);
            MostrarHistorial = true;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally { IsBusy = false; }
    }

    private void FiltrarMantenimientos()
    {
        var filtrados = string.IsNullOrWhiteSpace(_busqueda)
            ? _todosLosMantenimientos
            : _todosLosMantenimientos.Where(m =>
                m.CodigoEquipo.Contains(_busqueda, StringComparison.OrdinalIgnoreCase) ||
                m.NombreEquipo.Contains(_busqueda, StringComparison.OrdinalIgnoreCase) ||
                m.TipoMantenimiento.Contains(_busqueda, StringComparison.OrdinalIgnoreCase)).ToList();

        Mantenimientos.Clear();
        foreach (var m in filtrados)
            Mantenimientos.Add(m);
    }

    private async Task EliminarAsync(MantenimientoListadoDto mantenimiento)
    {
        bool confirmar = await Shell.Current.DisplayAlertAsync(
            "Confirmar eliminación",
            $"¿Deseas eliminar el mantenimiento del equipo {mantenimiento.CodigoEquipo}?",
            "Sí, eliminar", "Cancelar");

        if (!confirmar) return;

        var resultado = await _mantenimientoApiService.EliminarAsync(mantenimiento.Id);
        if (resultado)
        {
            Mantenimientos.Remove(mantenimiento);
            await Shell.Current.DisplayAlertAsync("Éxito", "Mantenimiento eliminado correctamente.", "OK");
        }
        else
            await Shell.Current.DisplayAlertAsync("Error", "No se pudo eliminar el mantenimiento.", "OK");
    }

    private async Task EditarAsync(MantenimientoListadoDto mantenimiento)
    {
        await Shell.Current.GoToAsync($"MantenimientoFormulario?id={mantenimiento.Id}");
    }

    private async Task FinalizarAsync(MantenimientoListadoDto mantenimiento)
    {
        bool confirmar = await Shell.Current.DisplayAlertAsync(
            "Finalizar mantenimiento",
            $"¿Deseas marcar como completado el mantenimiento del equipo {mantenimiento.CodigoEquipo}?",
            "Sí, finalizar", "Cancelar");

        if (!confirmar) return;

        var dto = new ActualizarMantenimientoDto
        {
            TipoMantenimientoId = mantenimiento.TipoMantenimientoId,
            TecnicoId = mantenimiento.TecnicoId,
            Descripcion = mantenimiento.Descripcion,
            EstadoMantenimiento = "Completado",
            Activo = true
        };

        var resultado = await _mantenimientoApiService.ActualizarAsync(mantenimiento.Id, dto);
        if (resultado)
        {
            await CargarMantenimientosAsync();
            await Shell.Current.DisplayAlertAsync("Éxito", "Mantenimiento finalizado correctamente.", "OK");
        }
        else
            await Shell.Current.DisplayAlertAsync("Error", "No se pudo finalizar el mantenimiento.", "OK");
    }
}