using System.Collections.ObjectModel;
using System.Windows.Input;
using ControlEquiposElectronicos.DTOs.Catalogos;
using ControlEquiposElectronicos.DTOs.Checklist;
using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.ViewModels.Checklist;

public class PlantillaChecklistItem
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public string TipoEquipo { get; init; } = string.Empty;
    public string? CategoriaEquipo { get; init; }
    public bool Activo { get; init; }
    public int TotalAspectos { get; init; }
    public ObservableCollection<PlantillaAspectoItem> Aspectos { get; init; } = new();
}

public class PlantillaAspectoItem
{
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public int Orden { get; init; }
    public bool EsObligatorio { get; init; }
    public string ObligatorioTexto => EsObligatorio ? "Obligatorio" : "Opcional";
}

public class NuevoAspectoFormItem : BaseViewModel
{
    private string _nombre = string.Empty;
    private string? _descripcion;
    private bool _esObligatorio = true;

    public string Nombre
    {
        get => _nombre;
        set { _nombre = value; OnPropertyChanged(); }
    }

    public string? Descripcion
    {
        get => _descripcion;
        set { _descripcion = value; OnPropertyChanged(); }
    }

    public bool EsObligatorio
    {
        get => _esObligatorio;
        set { _esObligatorio = value; OnPropertyChanged(); }
    }
}

public class PlantillasChecklistViewModel : BaseViewModel
{
    private readonly IChecklistApiService _checklistApiService;
    private readonly ICatalogoApiService _catalogoApiService;
    private string? _mensajeError;
    private string? _mensajeInfo;
    private bool _mostrarFormularioCrear;
    private string _nombrePlantilla = string.Empty;
    private string? _descripcionPlantilla;
    private CatalogoItemDto? _categoriaSeleccionada;
    private CatalogoItemDto? _tipoSeleccionado;

    public PlantillasChecklistViewModel(
        IChecklistApiService checklistApiService,
        ICatalogoApiService catalogoApiService)
    {
        _checklistApiService = checklistApiService;
        _catalogoApiService = catalogoApiService;
        Title = "Plantillas de checklist";

        Plantillas = new ObservableCollection<PlantillaChecklistItem>();
        Categorias = new ObservableCollection<CatalogoItemDto>();
        TiposEquipo = new ObservableCollection<CatalogoItemDto>();
        AspectosNuevos = new ObservableCollection<NuevoAspectoFormItem>();

        CargarCommand = new AsyncRelayCommand(CargarAsync);
        VolverCommand = new AsyncRelayCommand(async () => await Shell.Current.GoToAsync(".."));
        AbrirCrearCommand = new Command(AbrirFormularioCrear);
        CerrarCrearCommand = new Command(CerrarFormularioCrear);
        AgregarAspectoCommand = new Command(AgregarAspectoVacio);
        GuardarPlantillaCommand = new AsyncRelayCommand(GuardarPlantillaAsync);
    }

    public ObservableCollection<PlantillaChecklistItem> Plantillas { get; }
    public ObservableCollection<CatalogoItemDto> Categorias { get; }
    public ObservableCollection<CatalogoItemDto> TiposEquipo { get; }
    public ObservableCollection<NuevoAspectoFormItem> AspectosNuevos { get; }

    public string? MensajeError
    {
        get => _mensajeError;
        set
        {
            _mensajeError = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TieneError));
        }
    }

    public string? MensajeInfo
    {
        get => _mensajeInfo;
        set
        {
            _mensajeInfo = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TieneInfo));
        }
    }

    public bool TieneError => !string.IsNullOrWhiteSpace(MensajeError);
    public bool TieneInfo => !string.IsNullOrWhiteSpace(MensajeInfo);

    public bool MostrarFormularioCrear
    {
        get => _mostrarFormularioCrear;
        set { _mostrarFormularioCrear = value; OnPropertyChanged(); }
    }

    public string NombrePlantilla
    {
        get => _nombrePlantilla;
        set { _nombrePlantilla = value; OnPropertyChanged(); }
    }

    public string? DescripcionPlantilla
    {
        get => _descripcionPlantilla;
        set { _descripcionPlantilla = value; OnPropertyChanged(); }
    }

    public CatalogoItemDto? CategoriaSeleccionada
    {
        get => _categoriaSeleccionada;
        set { _categoriaSeleccionada = value; OnPropertyChanged(); }
    }

    public CatalogoItemDto? TipoSeleccionado
    {
        get => _tipoSeleccionado;
        set { _tipoSeleccionado = value; OnPropertyChanged(); }
    }

    public ICommand CargarCommand { get; }
    public ICommand VolverCommand { get; }
    public ICommand AbrirCrearCommand { get; }
    public ICommand CerrarCrearCommand { get; }
    public ICommand AgregarAspectoCommand { get; }
    public ICommand GuardarPlantillaCommand { get; }

    public async Task CargarAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            await CargarDatosAsync();
        }
        catch (Exception ex)
        {
            MensajeError = $"No se pudieron cargar las plantillas: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CargarDatosAsync()
    {
        MensajeError = null;

        await CargarCatalogosAsync();

        Plantillas.Clear();
        var plantillas = await _checklistApiService.ObtenerPlantillasAsync();

        if (plantillas.Count == 0)
            MensajeInfo = "No hay plantillas registradas. Crea la primera con el botón «Nueva plantilla».";

        foreach (var plantilla in plantillas.OrderBy(p => p.TipoEquipo).ThenBy(p => p.Nombre))
        {
            var item = new PlantillaChecklistItem
            {
                Id = plantilla.Id,
                Nombre = plantilla.Nombre,
                Descripcion = plantilla.Descripcion,
                TipoEquipo = plantilla.TipoEquipo,
                CategoriaEquipo = plantilla.CategoriaEquipo,
                Activo = plantilla.Activo,
                TotalAspectos = plantilla.Items.Count(i => i.Activo)
            };

            foreach (var aspecto in plantilla.Items
                         .Where(i => i.Activo)
                         .OrderBy(i => i.Orden))
            {
                item.Aspectos.Add(new PlantillaAspectoItem
                {
                    Nombre = aspecto.Nombre,
                    Descripcion = aspecto.Descripcion,
                    Orden = aspecto.Orden,
                    EsObligatorio = aspecto.EsObligatorio
                });
            }

            Plantillas.Add(item);
        }
    }

    private async Task CargarCatalogosAsync()
    {
        var categorias = await _catalogoApiService.ObtenerCategoriasAsync();
        var tipos = await _catalogoApiService.ObtenerTiposEquipoAsync();

        Categorias.Clear();
        foreach (var c in categorias.Where(x => x.Activo))
            Categorias.Add(c);

        TiposEquipo.Clear();
        foreach (var t in tipos.Where(x => x.Activo))
            TiposEquipo.Add(t);
    }

    private void AbrirFormularioCrear()
    {
        MensajeError = null;
        MensajeInfo = null;
        NombrePlantilla = string.Empty;
        DescripcionPlantilla = null;
        CategoriaSeleccionada = null;
        TipoSeleccionado = null;
        AspectosNuevos.Clear();
        AgregarAspectoVacio();
        MostrarFormularioCrear = true;
    }

    private void CerrarFormularioCrear() => MostrarFormularioCrear = false;

    private void AgregarAspectoVacio() =>
        AspectosNuevos.Add(new NuevoAspectoFormItem());

    public void QuitarAspecto(NuevoAspectoFormItem item)
    {
        if (AspectosNuevos.Count <= 1)
            return;

        AspectosNuevos.Remove(item);
    }

    private async Task GuardarPlantillaAsync()
    {
        var nombre = NombrePlantilla.Trim();
        if (string.IsNullOrWhiteSpace(nombre))
        {
            await Shell.Current.DisplayAlertAsync("Datos incompletos", "Indica el nombre de la plantilla.", "OK");
            return;
        }

        if (TipoSeleccionado == null)
        {
            await Shell.Current.DisplayAlertAsync("Datos incompletos", "Selecciona el tipo de equipo.", "OK");
            return;
        }

        var aspectosValidos = AspectosNuevos
            .Where(a => !string.IsNullOrWhiteSpace(a.Nombre))
            .ToList();

        if (aspectosValidos.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Datos incompletos",
                "Agrega al menos un aspecto de revisión con nombre.", "OK");
            return;
        }

        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            MensajeError = null;

            var dto = new CrearPlantillaChecklistDto
            {
                Nombre = nombre,
                Descripcion = string.IsNullOrWhiteSpace(DescripcionPlantilla) ? null : DescripcionPlantilla.Trim(),
                CategoriaEquipoId = CategoriaSeleccionada?.Id,
                TipoEquipoId = TipoSeleccionado.Id
            };

            var (plantilla, error) = await _checklistApiService.CrearPlantillaConDetalleAsync(dto);
            if (plantilla == null)
            {
                MensajeError = error ?? "No se pudo crear la plantilla.";
                await Shell.Current.DisplayAlertAsync("Error", MensajeError, "OK");
                return;
            }

            var orden = 1;
            foreach (var aspecto in aspectosValidos)
            {
                var itemDto = new CrearPlantillaChecklistItemDto
                {
                    Nombre = aspecto.Nombre.Trim(),
                    Descripcion = string.IsNullOrWhiteSpace(aspecto.Descripcion) ? null : aspecto.Descripcion.Trim(),
                    Orden = orden++,
                    EsObligatorio = aspecto.EsObligatorio
                };

                var creado = await _checklistApiService.AgregarItemPlantillaAsync(plantilla.Id, itemDto);
                if (creado == null)
                {
                    MensajeError = $"La plantilla se creó, pero falló el aspecto «{aspecto.Nombre}».";
                    break;
                }
            }

            MostrarFormularioCrear = false;
            await CargarDatosAsync();

            MensajeInfo = MensajeError == null
                ? $"Plantilla «{nombre}» creada con {aspectosValidos.Count} aspecto(s)."
                : $"Plantilla «{nombre}» creada parcialmente. Revisa los aspectos en la API.";

            await Shell.Current.DisplayAlertAsync(
                MensajeError == null ? "Plantilla creada" : "Plantilla creada con advertencias",
                MensajeInfo,
                "OK");
        }
        catch (Exception ex)
        {
            MensajeError = ex.Message;
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
