using ControlEquiposElectronicos.DTOs.Usuarios;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Views.Login;

public partial class RegistroUsuarioPage : ContentPage
{
    private readonly IUsuarioApiService _usuarioApi;
    private string _rolSeleccionado = string.Empty;

    public RegistroUsuarioPage()
    {
        InitializeComponent();
        _usuarioApi = IPlatformApplication.Current!.Services.GetRequiredService<IUsuarioApiService>();
    }

    private void OnRolAdministrador(object? sender, EventArgs e)
    {
        // La API guarda el rol como "Administrador"
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

        // Validación: campos obligatorios
        if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(nickname) ||
            string.IsNullOrWhiteSpace(contrasena) ||
            string.IsNullOrWhiteSpace(rol))
        {
            await DisplayAlert("Datos incompletos",
                "Por favor completa el nombre, usuario, contraseña y selecciona un rol.", "Entendido");
            return;
        }

        if (contrasena.Length < 4)
        {
            await DisplayAlert("Contraseña muy corta",
                "La contraseña debe tener al menos 4 caracteres.", "Entendido");
            return;
        }

        // Armar el usuario para enviar a la API
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

        // Llamar a la API
        bool exito = await _usuarioApi.CrearAsync(nuevoUsuario);

        if (exito)
        {
            // Mostramos "Técnico" con tilde al usuario aunque internamente sea "Tecnico"
            var rolMostrar = rol == "Tecnico" ? "Técnico" : rol;

            await DisplayAlert("Usuario registrado",
                $"El usuario \"{nickname}\" ({rolMostrar}) fue registrado correctamente.",
                "Entendido");

            // Limpiar el formulario
            NombreEntry.Text = string.Empty;
            NicknameEntry.Text = string.Empty;
            CorreoEntry.Text = string.Empty;
            TelefonoEntry.Text = string.Empty;
            ContrasenaEntry.Text = string.Empty;
            ActivoSwitch.IsToggled = true;
            _rolSeleccionado = string.Empty;
            BtnAdministrador.BackgroundColor = Color.FromArgb("#F0F0F0");
            BtnAdministrador.TextColor = Color.FromArgb("#1F1F1F");
            BtnTecnico.BackgroundColor = Color.FromArgb("#F0F0F0");
            BtnTecnico.TextColor = Color.FromArgb("#1F1F1F");
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
        await Shell.Current.GoToAsync("..");
    }
}
