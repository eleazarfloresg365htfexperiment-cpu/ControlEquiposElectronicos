namespace ControlEquiposElectronicos.Helpers;

public static class ApiConstants
{
    // API local en Windows.
    public const string BaseUrl = "http://localhost:5003/api/";

    // Para emulador Android, normalmente se usa:
    // public const string BaseUrl = "http://10.0.2.2:5003/api/";

    // Para una API publicada en Azure, después se cambia aquí:
    // public const string BaseUrl = "https://tu-api.azurewebsites.net/api/";
}