using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public partial class RevisionEquipoChecklistPage : ContentPage, IQueryAttributable
{
    private readonly RevisionEquipoChecklistViewModel _viewModel;

    public RevisionEquipoChecklistPage() : this(ServiceHelper.GetRequiredService<RevisionEquipoChecklistViewModel>()) { }

    public RevisionEquipoChecklistPage(RevisionEquipoChecklistViewModel viewModel)
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
        await _viewModel.CargarAsync();
    }

}
