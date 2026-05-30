using ControlEquiposElectronicos.Services;

namespace ControlEquiposElectronicos.Views.Configuracion;

public partial class PerfilUsuarioPage : ContentPage
{
    private readonly SesionService _sesion;

    public PerfilUsuarioPage()
    {
        InitializeComponent();
        _sesion = IPlatformApplication.Current!.Services.GetRequiredService<SesionService>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarPerfil();
    }

    private void CargarPerfil()
    {
        var usuario = _sesion.UsuarioActual;
        if (usuario == null) return;

        NombreCompletoLabel.Text = usuario.Nombre;
        InicialesLabel.Text = ObtenerIniciales(usuario.Nombre);
        NombreInfoLabel.Text = usuario.Nombre;
        RolInfoLabel.Text = usuario.Rol;

        PermisosContainer.Children.Clear();

        if (usuario.Permisos == null || usuario.Permisos.Count == 0)
        {
            SinPermisosLabel.IsVisible = true;
            return;
        }

        SinPermisosLabel.IsVisible = false;

        var grupos = usuario.Permisos
            .Select(p => { var pts = p.Split('.'); return new { Modulo = pts.Length > 0 ? pts[0] : p, Accion = pts.Length > 1 ? pts[1] : "" }; })
            .GroupBy(p => p.Modulo)
            .OrderBy(g => g.Key);

        foreach (var grupo in grupos)
        {
            var filaModulo = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Auto }, new ColumnDefinition { Width = GridLength.Star } },
                ColumnSpacing = 10,
                Margin = new Thickness(0, 4, 0, 2)
            };
            filaModulo.Add(new Label { FontFamily = "FontAwesome", Text = ObtenerIconoModulo(grupo.Key), TextColor = Color.FromArgb("#512BD4"), FontSize = 13, VerticalOptions = LayoutOptions.Center }, 0, 0);
            filaModulo.Add(new Label { Text = grupo.Key, FontSize = 14, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#212121"), VerticalOptions = LayoutOptions.Center }, 1, 0);
            PermisosContainer.Children.Add(filaModulo);

            var wrap = new FlexLayout
            {
                Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap,
                Direction = Microsoft.Maui.Layouts.FlexDirection.Row,
                JustifyContent = Microsoft.Maui.Layouts.FlexJustify.Start,
                AlignItems = Microsoft.Maui.Layouts.FlexAlignItems.Center,
                Margin = new Thickness(22, 0, 0, 6)
            };

            foreach (var permiso in grupo.OrderBy(p => p.Accion))
            {
                if (string.IsNullOrEmpty(permiso.Accion)) continue;
                var chip = new Border
                {
                    BackgroundColor = Color.FromArgb("#F3F0FD"),
                    StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(6) },
                    StrokeThickness = 0,
                    Padding = new Thickness(10, 4),
                    Margin = new Thickness(0, 3, 6, 3),
                    Content = new Label { Text = permiso.Accion, FontSize = 12, TextColor = Color.FromArgb("#512BD4") }
                };
                wrap.Children.Add(chip);
            }
            PermisosContainer.Children.Add(wrap);
        }
    }

    // Botón volver
    private async void OnVolverClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("..");

    private static string ObtenerIniciales(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return "??";
        var partes = nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 1) return partes[0][..Math.Min(2, partes[0].Length)].ToUpper();
        return $"{partes[0][0]}{partes[1][0]}".ToUpper();
    }

    private static string ObtenerIconoModulo(string modulo) => modulo switch
    {
        "Dashboard" => "\uf015",
        "Equipos" => "\uf108",
        "Mantenimientos" => "\uf0ad",
        "Checklist" => "\uf46c",
        "Reportes" => "\uf15c",
        "Usuarios" => "\uf0c0",
        "Configuracion" => "\uf013",
        _ => "\uf111"
    };
}
