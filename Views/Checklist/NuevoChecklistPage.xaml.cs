using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public partial class NuevoChecklistPage : ContentPage, IQueryAttributable
{
    private readonly NuevoChecklistViewModel _viewModel;

    public NuevoChecklistPage() : this(ServiceHelper.GetRequiredService<NuevoChecklistViewModel>()) { }

    public NuevoChecklistPage(NuevoChecklistViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query) =>
        _viewModel.ApplyQueryAttributes(query);

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.ChecklistIniciado)
            await _viewModel.RecargarChecklistActivoAsync();
        else
            await _viewModel.InicializarAsync();
    }

    private async void OnEquipoSeleccionado(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not EquipoChecklistCardItem card)
            return;

        if (sender is CollectionView collectionView)
            collectionView.SelectedItem = null;

        if (_viewModel.ChecklistActivo != null)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(RevisionEquipoChecklistPage)}" +
                $"?ChecklistId={_viewModel.ChecklistActivo.Id}" +
                $"&ChecklistTecnicoEquipoId={card.ChecklistTecnicoEquipoId}");
        }
    }

}
