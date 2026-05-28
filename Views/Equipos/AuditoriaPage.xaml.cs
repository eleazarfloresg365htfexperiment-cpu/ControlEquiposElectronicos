using ControlEquiposElectronicos.DTOs.Equipos;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Views.Equipos;

public partial class AuditoriaPage : ContentPage
{
    private readonly IApiService _apiService;
    public List<AuditoriaDto> Auditorias { get; set; } = new();

    public AuditoriaPage(IApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarAuditoriaAsync();
    }

    private async Task CargarAuditoriaAsync()
    {
        try
        {
            var resultado = await _apiService.GetAsync<List<AuditoriaDto>>("Auditoria");
            if (resultado != null)
            {
                Auditorias = resultado;
                OnPropertyChanged(nameof(Auditorias));
            }
        }
        catch
        {
            // API no disponible aún
        }
    }

    private async void OnVolverTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}