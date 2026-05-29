using ControlEquiposElectronicos.DTOs.Usuarios;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Views.Login;

public partial class RegistroUsuarioPage : ContentPage
{
    private readonly IUsuarioApiService _usuarioApi;
    private string _rolSeleccionado = string.Empty;
    private readonly bool _permiteElegirRol;

    // permiteElegirRol = true  -> abierto por admin (desde Configuración): muestra botones de rol
    // permiteElegirRol = false -> auto-registro (desde el login): sin rol, se asigna "Consulta"
    public RegistroUsuarioPage(bool permiteElegirRol = true)
    {
        InitializeComponent();
        _usuarioApi = IPlatformApplication.Current!.Services.GetRequiredService<IUsuarioApiService>();
        _permiteElegirRol = permiteElegirRol;

        // Si es auto-registro desde el login, ocultar la selección de rol
        SeccionRol.IsVisible = _permiteElegirRol;

        if (!_permiteElegirRol)
        {
            // Rol por defecto para quien se auto-registra; el admin lo cambia después
            _rolSeleccionado = "Consulta";
        }
    }

    private void OnRolAdministrador(object? sender, EventArgs e)
    {
        _rolSeleccionado = "Administrador";
        MarcarRol(BtnAdministrador, BtnTecnico);
    }

    private void OnRolTecnico(object? sender, EventArgs e)
    {
        // OJO: en la base de datos el rol es "Tecnico" SIN tilde
        _rolSeleccionado = "Tecnico";
        MarcarRol(BtnTecnico, BtnAdministrador);
    }

    private void MarcarRol(Button elegido, Button otro)
    {
        elegido.BackgroundColor = Color.FromArgb("#512BD4");
        elegido.TextColor = Colors.White;

        otro.BackgroundColor = Color.FromArgb("#F0F0F0");
        otro.TextColor = Color.FromArgb("#1F1F1F");
    }

    private async void OnRegistrarClicked(object? sender, EventArgs e)
    {
        var nombre = NombreEntry.Text?.Trim() ?? string.Empty;
        var nickname = NicknameEntry.Text?.Trim() ?? string.Empty;
        var correo = CorreoEntry.Text?.Trim() ?? string.Empty;
        var telefono = TelefonoEntry.Text?.Trim() ?? string.Empty;
        var contrasena = ContrasenaEntry.Text ?? string.Empty;
        var rol = _rolSeleccionado;

        // Validación: campos obligatorios (el rol solo se valida si el usuario puede elegirlo)
        if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(nickname) ||
            string.IsNullOrWhiteSpace(contrasena) ||
            string.IsNullOrWhiteSpace(rol))
        {
            var mensaje = _permiteElegirRol
                ? "Por favor completa el nombre, usuario, contraseña y selecciona un rol."
                : "Por favor completa el nombre, usuario y contraseña.";
            await DisplayAlert("Datos incompletos", mensaje, "Entendido");
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
            Rol = rol,
            Activo = ActivoSwitch.IsToggled,
            Telefono = string.IsNullOrWhiteSpace(telefono) ? null : telefono,
            Correo = string.IsNullOrWhiteSpace(correo) ? null : correo
        };

        bool exito = await _usuarioApi.CrearAsync(nuevoUsuario);

        if (exito)
        {
            var rolMostrar = rol == "Tecnico" ? "Técnico" : rol;

            string mensajeExito = _permiteElegirRol
                ? $"El usuario \"{nickname}\" ({rolMostrar}) fue registrado correctamente."
                : $"El usuario \"{nickname}\" fue registrado correctamente. Un administrador te asignará los permisos.";

            await DisplayAlert("Usuario registrado", mensajeExito, "Entendido");

            // Limpiar el formulario
            NombreEntry.Text = string.Empty;
            NicknameEntry.Text = string.Empty;
            CorreoEntry.Text = string.Empty;
            TelefonoEntry.Text = string.Empty;
            ContrasenaEntry.Text = string.Empty;
            ActivoSwitch.IsToggled = true;

            if (_permiteElegirRol)
            {
                _rolSeleccionado = string.Empty;
                BtnAdministrador.BackgroundColor = Color.FromArgb("#F0F0F0");
                BtnAdministrador.TextColor = Color.FromArgb("#1F1F1F");
                BtnTecnico.BackgroundColor = Color.FromArgb("#F0F0F0");
                BtnTecnico.TextColor = Color.FromArgb("#1F1F1F");
            }
        }
        else
        {
            await DisplayAlert("No se pudo registrar",
                "Ocurrió un problema al registrar el usuario. Verifica que el nombre de usuario no esté repetido e intenta de nuevo.",
                "Entendido");
        }
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
        else
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
