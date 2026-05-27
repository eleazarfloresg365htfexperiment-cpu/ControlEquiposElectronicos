using ControlEquiposElectronicos.DTOs.Mantenimientos;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Mantenimientos;

[QueryProperty(nameof(MantenimientoId), "id")]
public class MantenimientoFormularioViewModel : BaseViewModel
{
    private readonly IMantenimientoApiService _mantenimientoApiService;

    private int _mantenimientoId;
    public int MantenimientoId
    {
        get => _mantenimientoId;
        set
        {
            _mantenimientoId = value;
            OnPropertyChanged();
            if (value > 0)
                Task.Run(async () => await CargarParaEditarAsync(value));
        }
    }

    public bool EsEdicion => MantenimientoId > 0;
    public int EquipoId { get; set; }

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

    public int TipoMantenimientoId { get; set; } = 1;
    public int TecnicoId { get; set; } = 1;

    private string _descripcion = string.Empty;
    public string Descripcion
    {
        get => _descripcion;
        set { _descripcion = value; OnPropertyChanged(); }
    }

    private string _diagnostico = string.Empty;
    public string Diagnostico
    {
        get => _diagnostico;
        set { _diagnostico = value; OnPropertyChanged(); }
    }

    private string _estadoMantenimiento = "Pendiente";
    public string EstadoMantenimiento
    {
        get => _estadoMantenimiento;
        set { _estadoMantenimiento = value; OnPropertyChanged(); }
    }

    private string _costoEstimado = string.Empty;
    public string CostoEstimado
    {
        get => _costoEstimado;
        set { _costoEstimado = value; OnPropertyChanged(); }
    }

    public ICommand GuardarCommand { get; }
    public ICommand CancelarCommand { get; }

    public MantenimientoFormularioViewModel(IMantenimientoApiService mantenimientoApiService)
    {
        _mantenimientoApiService = mantenimientoApiService;
        Title = "Registrar mantenimiento";
        GuardarCommand = new Command(async () => await GuardarAsync());
        CancelarCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    private async Task CargarParaEditarAsync(int id)
    {
        Title = "Editar mantenimiento";
        var todos = await _mantenimientoApiService.ObtenerTodosAsync();
        var m = todos.FirstOrDefault(x => x.Id == id);
        if (m == null) return;

        EquipoId = m.EquipoId;
        EquipoIdTexto = m.EquipoId.ToString();
        TipoMantenimientoId = m.TipoMantenimientoId;
        TecnicoId = m.TecnicoId;
        Descripcion = m.Descripcion;
        Diagnostico = m.Diagnostico ?? string.Empty;
        EstadoMantenimiento = m.EstadoMantenimiento;
        CostoEstimado = m.CostoEstimado?.ToString() ?? string.Empty;
    }

    private async Task GuardarAsync()
    {
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

        IsBusy = true;
        try
        {
            decimal? costo = null;
            if (!string.IsNullOrWhiteSpace(CostoEstimado) && decimal.TryParse(CostoEstimado, out var costoDecimal))
                costo = costoDecimal;

            bool resultado;

            if (EsEdicion)
            {
                var dto = new ActualizarMantenimientoDto
                {
                    TipoMantenimientoId = TipoMantenimientoId,
                    TecnicoId = TecnicoId,
                    Descripcion = Descripcion,
                    Diagnostico = string.IsNullOrWhiteSpace(Diagnostico) ? null : Diagnostico,
                    CostoEstimado = costo,
                    EstadoMantenimiento = EstadoMantenimiento,
                    Activo = true
                };
                resultado = await _mantenimientoApiService.ActualizarAsync(MantenimientoId, dto);
            }
            else
            {
                var dto = new CrearMantenimientoDto
                {
                    EquipoId = EquipoId,
                    TipoMantenimientoId = TipoMantenimientoId,
                    TecnicoId = TecnicoId,
                    Descripcion = Descripcion,
                    Diagnostico = string.IsNullOrWhiteSpace(Diagnostico) ? null : Diagnostico,
                    CostoEstimado = costo,
                    EstadoMantenimiento = EstadoMantenimiento
                };
                resultado = await _mantenimientoApiService.CrearAsync(dto);
            }

            if (resultado)
            {
                await Shell.Current.DisplayAlertAsync("Éxito", EsEdicion ? "Mantenimiento actualizado correctamente." : "Mantenimiento registrado correctamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Error", "No se pudo guardar el mantenimiento.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally { IsBusy = false; }
    }
}