using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;

namespace ControlEquiposElectronicos.ViewModels;

public class EquiposViewModel : BaseViewModel
{
    private readonly IEquipoApiService _equipoApiService;


    public ObservableCollection<EquipoListadoDto> Equipos { get; set; } = new();
    public EquiposViewModel(IEquipoApiService equipoApiService)
    {
        Title = "Gestión de Equipos";
        _equipoApiService = equipoApiService;
    }

    public async Task CargarEquiposAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var lista = await _equipoApiService.ObtenerTodosAsync();
            Equipos.Clear();
            foreach (var equipo in lista)
                Equipos.Add(equipo);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo cargar los equipos: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}