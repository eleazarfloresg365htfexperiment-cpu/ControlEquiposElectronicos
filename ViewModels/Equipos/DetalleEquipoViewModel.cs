using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels.Equipos;

public class DetalleEquiposViewModel : BaseViewModel
{
    private readonly IEquipoApiService _equipoApiService;
    private EquipoListadoDto? _equipo;
    public EquipoListadoDto? Equipo
    {
        get => _equipo;
        set
        {
            _equipo = value;
            OnPropertyChanged();
        }
    }
    public DetalleEquiposViewModel(IEquipoApiService equipoApiService)
    {
        Title = "Detalle del equipo";
        _equipoApiService = equipoApiService;
    }
    
    public async Task CargarEquipoAsync(int id)
    {
        try
        {
            IsBusy = true;
            Equipo = await _equipoApiService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo cargar el equipo: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}