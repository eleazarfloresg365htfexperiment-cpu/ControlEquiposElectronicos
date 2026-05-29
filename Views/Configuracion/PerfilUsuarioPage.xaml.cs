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

        // Cabecera: iniciales y nombre
        NombreCompletoLabel.Text = usuario.Nombre;
        InicialesLabel.Text = ObtenerIniciales(usuario.Nombre);

        // Tarjeta de información
        NombreInfoLabel.Text = usuario.Nombre;
        RolInfoLabel.Text = usuario.Rol;

        // Tarjeta de permisos
        PermisosContainer.Children.Clear();

        if (usuario.Permisos == null || usuario.Permisos.Count == 0)
        {
            SinPermisosLabel.IsVisible = true;
            return;
        }

        SinPermisosLabel.IsVisible = false;

        // Agrupar permisos por módulo (formato "Modulo.Accion")
        var grupos = usuario.Permisos
            .Select(p =>
            {
                var partes = p.Split('.');
                return new
                {
                    Modulo = partes.Length > 0 ? partes[0] : p,
                    Accion = partes.Length > 1 ? partes[1] : ""
                };
            })
            .GroupBy(p => p.Modulo)
            .OrderBy(g => g.Key);

        foreach (var grupo in grupos)
        {
            // Chip de módulo con icono
            var filaModulo = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                ColumnSpacing = 10,
                Margin = new Thickness(0, 4, 0, 2)
            };

            var iconoModulo = new Label
            {
                FontFamily = "FontAwesome",
                Text = ObtenerIconoModulo(grupo.Key),
                TextColor = Color.FromArgb("#512BD4"),
                FontSize = 13,
                VerticalOptions = LayoutOptions.Center
            };
            filaModulo.Add(iconoModulo, 0, 0);

            var labelModulo = new Label
            {
                Text = grupo.Key,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#212121"),
                VerticalOptions = LayoutOptions.Center
            };
            filaModulo.Add(labelModulo, 1, 0);

            PermisosContainer.Children.Add(filaModulo);

            // Chips de acciones dentro del módulo
            var wrapAcciones = new FlexLayout
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
                    Margin = new Thickness(0, 3, 6, 3)
                };

                var labelChip = new Label
                {
                    Text = permiso.Accion,
                    FontSize = 12,
                    TextColor = Color.FromArgb("#512BD4")
                };

                chip.Content = labelChip;
                wrapAcciones.Children.Add(chip);
            }

            PermisosContainer.Children.Add(wrapAcciones);
        }
    }

    // Extrae hasta 2 iniciales del nombre completo
    private static string ObtenerIniciales(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return "??";

        var partes = nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 1)
            return partes[0][..Math.Min(2, partes[0].Length)].ToUpper();

        return $"{partes[0][0]}{partes[1][0]}".ToUpper();
    }

    // Asocia un icono FontAwesome a cada módulo conocido
    private static string ObtenerIconoModulo(string modulo) => modulo switch
    {
        "Dashboard" => "\uf015", // house
        "Equipos" => "\uf108", // desktop
        "Mantenimientos" => "\uf0ad", // wrench
        "Checklist" => "\uf46c", // clipboard-check
        "Reportes" => "\uf15c", // file-alt
        "Usuarios" => "\uf0c0", // users
        "Configuracion" => "\uf013", // gear
        _ => "\uf111"  // circle (default)
    };
}
