using ControlEquiposElectronicos.DTOs.Usuarios;
using ControlEquiposElectronicos.Services.Interfaces;
using ControlEquiposElectronicos.ViewModels;

namespace ControlEquiposElectronicos.Views.Usuarios;

public partial class UsuariosPage : ContentPage
{
    private readonly UsuariosViewModel _viewModel;
    private readonly IUsuarioApiService _usuarioApi;
    private List<UsuarioListadoDto> _todosLosUsuarios = new();
    private string _rolSeleccionado = string.Empty;

    public UsuariosPage(UsuariosViewModel viewModel, IUsuarioApiService usuarioApi)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _usuarioApi = usuarioApi;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
        _todosLosUsuarios = _viewModel.Usuarios.ToList();
        ActualizarEstadisticas(_todosLosUsuarios);
        UsuariosCollection.ItemsSource = _todosLosUsuarios;
    }

    private void ActualizarEstadisticas(List<UsuarioListadoDto> lista)
    {
        LblTotal.Text = lista.Count.ToString();
        LblAdmins.Text = lista.Count(u => EsRolAdministrador(u.Rol)).ToString();
        LblTecnicos.Text = lista.Count(u => EsRolTecnico(u.Rol)).ToString();
        LblConsulta.Text = lista.Count(u => EsRolConsulta(u.Rol)).ToString();
    }

    private static bool EsRolAdministrador(string rol) =>
        rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ||
        rol.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
        rol.Equals("OP", StringComparison.OrdinalIgnoreCase);

    private static bool EsRolTecnico(string rol) =>
        rol.Equals("Tecnico", StringComparison.OrdinalIgnoreCase) ||
        rol.Equals("Técnico", StringComparison.OrdinalIgnoreCase);

    private static bool EsRolConsulta(string rol) =>
        rol.Equals("Consulta", StringComparison.OrdinalIgnoreCase);

    private void OnBusquedaTextChanged(object? sender, TextChangedEventArgs e)
    {
        var texto = (e.NewTextValue ?? string.Empty).Trim().ToLower();
        UsuariosCollection.ItemsSource = string.IsNullOrEmpty(texto)
            ? _todosLosUsuarios
            : _todosLosUsuarios.Where(u =>
                u.NombreCompleto.ToLower().Contains(texto) ||
                u.Nickname.ToLower().Contains(texto) ||
                u.Rol.ToLower().Contains(texto)).ToList();
    }

    private void OnNuevoUsuarioTapped(object? sender, TappedEventArgs e)
    {
        LimpiarFormulario();
        PanelOverlay.IsVisible = true;
    }

    private void OnCerrarPanelTapped(object? sender, EventArgs e)
    {
        PanelOverlay.IsVisible = false;
        LimpiarFormulario();
    }

    private void LimpiarFormulario()
    {
        NombreEntry.Text = string.Empty;
        NicknameEntry.Text = string.Empty;
        CorreoEntry.Text = string.Empty;
        TelefonoEntry.Text = string.Empty;
        ContrasenaEntry.Text = string.Empty;
        ActivoSwitch.IsToggled = true;
        _rolSeleccionado = string.Empty;
        ResetBotones();
    }

    private void ResetBotones()
    {
        foreach (var btn in new[] { BtnAdministrador, BtnTecnico, BtnConsulta })
        {
            btn.BackgroundColor = Color.FromArgb("#F8F7FC");
            btn.TextColor = Color.FromArgb("#4A5270");
            btn.BorderColor = Color.FromArgb("#E8E4F8");
        }
    }

    private void OnRolAdministrador(object? sender, EventArgs e)
    {
        _rolSeleccionado = "Administrador";
        MarcarRol(BtnAdministrador);
    }

    private void OnRolTecnico(object? sender, EventArgs e)
    {
        _rolSeleccionado = "Tecnico";
        MarcarRol(BtnTecnico);
    }

    private void OnRolConsulta(object? sender, EventArgs e)
    {
        _rolSeleccionado = "Consulta";
        MarcarRol(BtnConsulta);
    }

    private void MarcarRol(Button elegido)
    {
        ResetBotones();
        elegido.BackgroundColor = Color.FromArgb("#512BD4");
        elegido.TextColor = Colors.White;
        elegido.BorderColor = Color.FromArgb("#512BD4");
    }

    private async void OnRegistrarClicked(object? sender, EventArgs e)
    {
        var nombre = NombreEntry.Text?.Trim() ?? string.Empty;
        var nickname = NicknameEntry.Text?.Trim() ?? string.Empty;
        var correo = CorreoEntry.Text?.Trim() ?? string.Empty;
        var telefono = TelefonoEntry.Text?.Trim() ?? string.Empty;
        var contrasena = ContrasenaEntry.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(nickname) ||
            string.IsNullOrWhiteSpace(contrasena) ||
            string.IsNullOrWhiteSpace(_rolSeleccionado))
        {
            await DisplayAlert("Datos incompletos",
                "Por favor completa el nombre, usuario, contraseña y selecciona un rol.",
                "Entendido");
            return;
        }

        if (contrasena.Length < 4)
        {
            await DisplayAlert("Contraseña muy corta",
                "La contraseña debe tener al menos 4 caracteres.", "Entendido");
            return;
        }

        var nuevoUsuario = new CrearUsuarioDto
        {
            NombreCompleto = nombre,
            Nickname = nickname,
            Contrasena = contrasena,
            Rol = _rolSeleccionado,
            Activo = ActivoSwitch.IsToggled,
            Telefono = string.IsNullOrWhiteSpace(telefono) ? null : telefono,
            Correo = string.IsNullOrWhiteSpace(correo) ? null : correo
        };

        bool exito = await _usuarioApi.CrearAsync(nuevoUsuario);

        if (exito)
        {
            var rolMostrar = _rolSeleccionado == "Tecnico" ? "Técnico" : _rolSeleccionado;
            await DisplayAlert("Usuario registrado",
                $"El usuario \"{nickname}\" ({rolMostrar}) fue registrado correctamente.",
                "Entendido");

            PanelOverlay.IsVisible = false;
            LimpiarFormulario();

            await _viewModel.CargarAsync();
            _todosLosUsuarios = _viewModel.Usuarios.ToList();
            ActualizarEstadisticas(_todosLosUsuarios);
            UsuariosCollection.ItemsSource = _todosLosUsuarios;
        }
        else
        {
            await DisplayAlert("No se pudo registrar",
                "Verifica que el nombre de usuario no esté repetido e intenta de nuevo.",
                "Entendido");
        }
    }

    private async void OnPermisosRolTapped(object? sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("PermisosRol");

    private async void OnRolesUsuarioTapped(object? sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("RolesUsuario");
}
