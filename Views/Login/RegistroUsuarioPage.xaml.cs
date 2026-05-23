namespace ControlEquiposElectronicos.Views.Login;

public partial class RegistroUsuarioPage : ContentPage
{
    public RegistroUsuarioPage()
    {
        InitializeComponent();
    }

    private async void OnRegistrarClicked(object? sender, EventArgs e)
    {
        var nombre = NombreEntry.Text?.Trim() ?? string.Empty;
        var nickname = NicknameEntry.Text?.Trim() ?? string.Empty;
        var contrasena = ContrasenaEntry.Text ?? string.Empty;
        var rol = RolPicker.SelectedItem?.ToString();

        // Validación: ningún campo vacío
        if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(nickname) ||
            string.IsNullOrWhiteSpace(contrasena) ||
            string.IsNullOrWhiteSpace(rol))
        {
            await DisplayAlert("Datos incompletos",
                "Por favor completa todos los campos y selecciona un rol.", "Entendido");
            return;
        }

        // Validación: contraseña mínima
        if (contrasena.Length < 4)
        {
            await DisplayAlert("Contraseña muy corta",
                "La contraseña debe tener al menos 4 caracteres.", "Entendido");
            return;
        }

        // Registro simulado (la creación real se conecta cuando la API tenga el endpoint)
        await DisplayAlert("Usuario registrado",
            $"El usuario \"{nickname}\" ({rol}) se registró localmente. " +
            "La creación en el servidor se conectará cuando la API esté disponible.",
            "Entendido");

        // Limpiar el formulario
        NombreEntry.Text = string.Empty;
        NicknameEntry.Text = string.Empty;
        ContrasenaEntry.Text = string.Empty;
        RolPicker.SelectedIndex = -1;
        ActivoSwitch.IsToggled = true;
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}