using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public partial class ChecklistPage : ContentPage
{
    private readonly ChecklistViewModel _viewModel;

    public ChecklistPage() : this(ServiceHelper.GetRequiredService<ChecklistViewModel>()) { }

    public ChecklistPage(ChecklistViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }

    private async void OnRevisionSeleccionada(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not ChecklistResumenItem item)
            return;

        if (sender is CollectionView collectionView)
            collectionView.SelectedItem = null;

        await _viewModel.AbrirRevisionItemAsync(item);
    }

}
