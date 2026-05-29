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

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(ApiConstants.BaseUrl)
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
        catch { return default; }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var (response, _) = await PostWithErrorAsync<TRequest, TResponse>(endpoint, data);
        return response;
    }

    public async Task<(TResponse? Response, string? ErrorMessage)> PostWithErrorAsync<TRequest, TResponse>(
        string endpoint, TRequest data)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data, _jsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                return (default, string.IsNullOrWhiteSpace(errorBody)
                    ? $"Error HTTP {(int)response.StatusCode}"
                    : errorBody);
            }
            var result = await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
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
            var response = await _httpClient.PutAsJsonAsync(endpoint, data, _jsonOptions);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> PatchAsync<TRequest>(string endpoint, TRequest data)
    {
        var (exito, _) = await PatchWithErrorAsync(endpoint, data);
        return exito;
    }

    // Devuelve éxito + el mensaje de error exacto que manda la API (ej: "El usuario ya tiene ese rol.")
    public async Task<(bool Exito, string? ErrorMessage)> PatchWithErrorAsync<TRequest>(
        string endpoint, TRequest data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Patch, endpoint) { Content = content };

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
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}
