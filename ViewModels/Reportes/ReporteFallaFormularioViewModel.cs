using ControlEquiposElectronicos.DTOs.Reportes;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Windows.Input;

namespace ControlEquiposElectronicos.ViewModels.Reportes;

public class ReporteFallaFormularioViewModel : BaseViewModel
{
    private readonly IReporteFallaApiService _reporteFallaApiService;

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

    public ICommand GuardarCommand { get; }
    public ICommand CancelarCommand { get; }

    public ReporteFallaFormularioViewModel(IReporteFallaApiService reporteFallaApiService)
    {
        Title = "Reporte de falla";
        _reporteFallaApiService = reporteFallaApiService;
        GuardarCommand = new Command(async () => await GuardarAsync());
        CancelarCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
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

        IsBusy = true;
        try
        {
            var dto = new CrearReporteFallaDto
            {
                EquipoId = EquipoId,
                UsuarioReportaId = 1,
                EstadoReporteId = 1,
                Titulo = Titulo,
                Descripcion = Descripcion,
                Prioridad = Prioridad
            };

            var resultado = await _reporteFallaApiService.CrearAsync(dto);

            if (resultado)
            {
                await Shell.Current.DisplayAlertAsync("Éxito", "Reporte registrado correctamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Error", "No se pudo registrar el reporte.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally { IsBusy = false; }
    }
}