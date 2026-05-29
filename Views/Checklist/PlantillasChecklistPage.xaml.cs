using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public partial class PlantillasChecklistPage : ContentPage
{
    private readonly PlantillasChecklistViewModel _viewModel;

    public PlantillasChecklistPage() : this(ServiceHelper.GetRequiredService<PlantillasChecklistViewModel>()) { }

    public PlantillasChecklistPage(PlantillasChecklistViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        PlantillasCollection.ItemTemplate = new DataTemplate(typeof(PlantillaChecklistDataTemplate));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        CargandoIndicator.IsVisible = CargandoIndicator.IsRunning = true;
        await _viewModel.CargarAsync();
        PlantillasCollection.ItemsSource = _viewModel.Plantillas;
        CargandoIndicator.IsRunning = CargandoIndicator.IsVisible = false;

        MensajeErrorLabel.Text = _viewModel.MensajeError ?? string.Empty;
        MensajeErrorLabel.IsVisible = _viewModel.TieneError;
    }

    private async void OnVolverClicked(object? sender, EventArgs e) =>
        await Navigation.PopModalAsync();

}
 