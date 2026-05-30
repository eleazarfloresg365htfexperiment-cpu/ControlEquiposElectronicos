using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ControlEquiposElectronicos.Helpers;
using ControlEquiposElectronicos.Services.Interfaces;

namespace ControlEquiposElectronicos.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SesionService _sesionService;

    public ApiService(SesionService sesionService)
    {
        _sesionService = sesionService;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(ApiConstants.BaseUrl)
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    private void AgregarUsuarioActualHeader()
    {
        _httpClient.DefaultRequestHeaders.Remove("X-Usuario-Id");

        if (_sesionService.UsuarioActual == null)
            return;

        _httpClient.DefaultRequestHeaders.Add(
            "X-Usuario-Id",
            _sesionService.UsuarioActual.UsuarioId.ToString());
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            AgregarUsuarioActualHeader();

            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
                return default;

            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
        catch
        {
            return default;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var (response, _) = await PostWithErrorAsync<TRequest, TResponse>(endpoint, data);
        return response;
    }

    public async Task<(TResponse? Response, string? ErrorMessage)> PostWithErrorAsync<TRequest, TResponse>(
        string endpoint,
        TRequest data)
    {
        try
        {
            AgregarUsuarioActualHeader();

            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                return (default, ExtraerMensajeError(errorBody, response.StatusCode));
            }

            var body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body))
                return (default, null);

            var result = JsonSerializer.Deserialize<TResponse>(body, _jsonOptions);
            return (result, null);
        }
        catch (Exception ex)
        {
            return (default, $"No se pudo conectar con la API: {ex.Message}");
        }
    }

    public async Task<bool> PutAsync<TRequest>(string endpoint, TRequest data)
    {
        try
        {
            AgregarUsuarioActualHeader();

            var response = await _httpClient.PutAsJsonAsync(endpoint, data, _jsonOptions);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> PatchAsync<TRequest>(string endpoint, TRequest data)
    {
        var (exito, _) = await PatchWithErrorAsync(endpoint, data);
        return exito;
    }

    public async Task<(bool Exito, string? ErrorMessage)> PatchWithErrorAsync<TRequest>(
        string endpoint,
        TRequest data)
    {
        try
        {
            AgregarUsuarioActualHeader();

            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Patch, endpoint)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();

                return (false, string.IsNullOrWhiteSpace(errorBody)
                    ? $"Error HTTP {(int)response.StatusCode}"
                    : errorBody);
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, $"No se pudo conectar con la API: {ex.Message}");
        }
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            AgregarUsuarioActualHeader();

            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static string ExtraerMensajeError(string errorBody, System.Net.HttpStatusCode statusCode)
    {
        if (string.IsNullOrWhiteSpace(errorBody))
            return $"Error HTTP {(int)statusCode}";

        try
        {
            using var doc = JsonDocument.Parse(errorBody);

            if (doc.RootElement.TryGetProperty("mensaje", out var mensaje))
                return mensaje.GetString() ?? errorBody;

            if (doc.RootElement.TryGetProperty("message", out var message))
                return message.GetString() ?? errorBody;

            if (doc.RootElement.TryGetProperty("title", out var title))
                return title.GetString() ?? errorBody;
        }
        catch
        {
            // Si no es JSON válido, se devuelve el texto original.
        }

        return errorBody;
    }
}