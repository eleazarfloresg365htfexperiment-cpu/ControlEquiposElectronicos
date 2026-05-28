using ControlEquiposElectronicos.ViewModels;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.DTOs.Catalogos;
using ControlEquiposElectronicos.Services.Interfaces;
using System.Collections.ObjectModel;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class PlantillasChecklistViewModel : BaseViewModel
{
    private readonly IChecklistApiService _checklistApiService;
    private readonly ICatalogoApiService _catalogoApiService;

    public ObservableCollection<PlantillaChecklistDto> Plantillas { get; } = new();
    public ObservableCollection<CatalogoItemDto> TiposEquipo { get; } = new();

    private string _nombreNuevaPlantilla = string.Empty;
    private string _descripcionNuevaPlantilla = string.Empty;
    private string _aspectosTexto = string.Empty;
    private CatalogoItemDto? _tipoEquipoSeleccionado;

    public string NombreNuevaPlantilla
    {
        get => _nombreNuevaPlantilla;
        set { _nombreNuevaPlantilla = value; OnPropertyChanged(); }
    }

    public string DescripcionNuevaPlantilla
    {
        get => _descripcionNuevaPlantilla;
        set { _descripcionNuevaPlantilla = value; OnPropertyChanged(); }
    }

    public string AspectosTexto
    {
        get => _aspectosTexto;
        set { _aspectosTexto = value; OnPropertyChanged(); }
    }

    public CatalogoItemDto? TipoEquipoSeleccionado
    {
        get => _tipoEquipoSeleccionado;
        set { _tipoEquipoSeleccionado = value; OnPropertyChanged(); }
    }

    public PlantillasChecklistViewModel(IChecklistApiService checklistApiService, ICatalogoApiService catalogoApiService)
    {
        _checklistApiService = checklistApiService;
        _catalogoApiService = catalogoApiService;
        Title = "Plantillas de checklist";
    }

    public async Task CargarAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            Plantillas.Clear();

            var datos = await _checklistApiService.ObtenerPlantillasAsync();
            foreach (var plantilla in datos.OrderBy(p => p.Nombre))
            {
                Plantillas.Add(plantilla);
            }

            if (TiposEquipo.Count == 0)
            {
                var tipos = await _catalogoApiService.ObtenerTiposEquipoAsync();
                foreach (var tipo in tipos.Where(t => t.Activo))
                {
                    TiposEquipo.Add(tipo);
                }
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task<(bool Ok, string Mensaje)> CrearPlantillaAsync()
    {
        if (string.IsNullOrWhiteSpace(NombreNuevaPlantilla))
            return (false, "Ingresa el nombre de la plantilla.");

        if (TipoEquipoSeleccionado == null)
            return (false, "Selecciona un tipo de equipo.");

        var aspectos = AspectosTexto
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (aspectos.Count == 0)
            return (false, "Ingresa al menos un aspecto (uno por línea).");

        try
        {
            IsBusy = true;
            var nuevaPlantilla = await _checklistApiService.CrearPlantillaAsync(new CrearPlantillaChecklistDto
            {
                Nombre = NombreNuevaPlantilla.Trim(),
                Descripcion = string.IsNullOrWhiteSpace(DescripcionNuevaPlantilla) ? null : DescripcionNuevaPlantilla.Trim(),
                TipoEquipoId = TipoEquipoSeleccionado.Id
            });

            if (nuevaPlantilla == null)
                return (false, "La API no devolvió la plantilla creada.");

            var orden = 1;
            foreach (var aspecto in aspectos)
            {
                await _checklistApiService.AgregarItemPlantillaAsync(nuevaPlantilla.Id, new CrearPlantillaChecklistItemDto
                {
                    Nombre = aspecto,
                    Orden = orden++,
                    EsObligatorio = true
                });
            }

            LimpiarFormulario();
            await CargarAsync();
            return (true, "Plantilla creada correctamente.");
        }
        catch (Exception ex)
        {
            return (false, $"No se pudo crear la plantilla: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void LimpiarFormulario()
    {
        NombreNuevaPlantilla = string.Empty;
        DescripcionNuevaPlantilla = string.Empty;
        AspectosTexto = string.Empty;
        TipoEquipoSeleccionado = null;
    }
}
