namespace ControlEquiposElectronicos.Views.Login;

public partial class RegistroUsuarioPage : ContentPage
{
    private string _rolSeleccionado = string.Empty;

    public RegistroUsuarioPage()
    {
        InitializeComponent();
    }

    private void OnRolAdministrador(object? sender, EventArgs e)
    {
        _rolSeleccionado = "Administrador";
        MarcarRol(BtnAdministrador, BtnTecnico);
    }

    private void OnRolTecnico(object? sender, EventArgs e)
    {
        _rolSeleccionado = "Técnico";
        MarcarRol(BtnTecnico, BtnAdministrador);
    }

    // Marca el botón elegido en morado y el otro lo deja gris
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
        var contrasena = ContrasenaEntry.Text ?? string.Empty;
        var rol = _rolSeleccionado;

        if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(nickname) ||
            string.IsNullOrWhiteSpace(contrasena) ||
            string.IsNullOrWhiteSpace(rol))
        {
            await DisplayAlert("Datos incompletos",
                "Por favor completa todos los campos y selecciona un rol.", "Entendido");
            return;
        }

        if (contrasena.Length < 4)
        {
            await DisplayAlert("Contraseña muy corta",
                "La contraseña debe tener al menos 4 caracteres.", "Entendido");
            return;
        }

        await DisplayAlert("Usuario registrado",
            $"El usuario \"{nickname}\" ({rol}) fue registrado correctamente.",
            "Entendido");

        // Limpiar el formulario
        NombreEntry.Text = string.Empty;
        NicknameEntry.Text = string.Empty;
        ContrasenaEntry.Text = string.Empty;
        ActivoSwitch.IsToggled = true;
        _rolSeleccionado = string.Empty;
        BtnAdministrador.BackgroundColor = Color.FromArgb("#F0F0F0");
        BtnAdministrador.TextColor = Color.FromArgb("#1F1F1F");
        BtnTecnico.BackgroundColor = Color.FromArgb("#F0F0F0");
        BtnTecnico.TextColor = Color.FromArgb("#1F1F1F");
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
