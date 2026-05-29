namespace ControlEquiposElectronicos.Services.Interfaces;

public interface IApiService
{
    Task<T?> GetAsync<T>(string endpoint);

    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);

    Task<(TResponse? Response, string? ErrorMessage)> PostWithErrorAsync<TRequest, TResponse>(string endpoint, TRequest data);

    Task<bool> PutAsync<TRequest>(string endpoint, TRequest data);

    Task<bool> PatchAsync<TRequest>(string endpoint, TRequest data);

    Task<bool> DeleteAsync(string endpoint);
}