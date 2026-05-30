namespace ControlEquiposElectronicos.Helpers;

public static class ApiConstants
{
    public static string BaseUrl =>
        DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5003/api/"
            : "http://localhost:5003/api/";
}
