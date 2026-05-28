using ControlEquiposElectronicos.ViewModels.Checklist;

namespace ControlEquiposElectronicos.Views.Checklist;

public class PlantillaChecklistDataTemplate : DataTemplate
{
    public PlantillaChecklistDataTemplate() : base(CreateView) { }

    private static View CreateView()
    {
        var nombreLabel = new Label { FontAttributes = FontAttributes.Bold, FontSize = 16 };
        nombreLabel.SetBinding(Label.TextProperty, nameof(PlantillaChecklistItem.Nombre));

        var tipoLabel = new Label { FontSize = 13 };
        tipoLabel.SetBinding(Label.TextProperty, nameof(PlantillaChecklistItem.TipoEquipo), stringFormat: "Tipo de equipo: {0}");

        var categoriaLabel = new Label { FontSize = 12, TextColor = Colors.Gray };
        categoriaLabel.SetBinding(Label.TextProperty, nameof(PlantillaChecklistItem.CategoriaEquipo), stringFormat: "Categoría: {0}");

        var aspectosLabel = new Label { FontSize = 12, TextColor = Color.FromArgb("#512BD4") };
        aspectosLabel.SetBinding(Label.TextProperty, nameof(PlantillaChecklistItem.TotalAspectos), stringFormat: "{0} aspectos de revisión");

        var aspectosLayout = new VerticalStackLayout { Spacing = 6, Padding = new Thickness(12, 0, 0, 0) };
        aspectosLayout.SetBinding(BindableLayout.ItemsSourceProperty, nameof(PlantillaChecklistItem.Aspectos));
        BindableLayout.SetItemTemplate(aspectosLayout, new DataTemplate(() =>
        {
            var itemNombre = new Label { FontAttributes = FontAttributes.Bold };
            itemNombre.SetBinding(Label.TextProperty, nameof(PlantillaAspectoItem.Nombre));

            var itemDetalle = new Label { FontSize = 12, TextColor = Colors.Gray };
            itemDetalle.SetBinding(Label.TextProperty, nameof(PlantillaAspectoItem.Descripcion));

            var itemOrden = new Label { FontSize = 12 };
            itemOrden.SetBinding(Label.TextProperty, nameof(PlantillaAspectoItem.Orden), stringFormat: "Orden: {0}");

            var itemObligatorio = new Label { FontSize = 12, TextColor = Color.FromArgb("#512BD4") };
            itemObligatorio.SetBinding(Label.TextProperty, nameof(PlantillaAspectoItem.ObligatorioTexto));

            return new Border
            {
                Padding = 10,
                BackgroundColor = Color.FromArgb("#F5F5F5"),
                Content = new VerticalStackLayout
                {
                    Spacing = 2,
                    Children = { itemNombre, itemDetalle, itemOrden, itemObligatorio }
                }
            };
        }));

        return new Border
        {
            Padding = 14,
            Margin = new Thickness(0, 0, 0, 12),
            Stroke = Color.FromArgb("#E0E0E0"),
            StrokeThickness = 1,
            Content = new VerticalStackLayout
            {
                Spacing = 6,
                Children =
                {
                    nombreLabel,
                    tipoLabel,
                    categoriaLabel,
                    aspectosLabel,
                    aspectosLayout
                }
            }
        };
    }
}
