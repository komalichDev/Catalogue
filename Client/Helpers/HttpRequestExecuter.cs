using System.Net.Http.Json;
using Common.Exception;
using Common.Types;

namespace Client.Helpers;

public class HttpRequestExecuter(HttpClient http) : IHttpRequestExecuter
{

    public async Task<QueryResult<T>> ExecuteGetRequests<T>(string url)
        where T : class
    {
        try
        {
            var value = await http.GetFromJsonAsync<QueryResult<T>>(url);
            return value ?? QueryResult<T>.Failure(ErrorCodes.NetworkError);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Netzwerkfehler: {ex.Message}");
            return QueryResult<T>.Failure(ErrorCodes.NetworkError);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Allgemeiner Fehler: {ex.ToString()}");
            return QueryResult<T>.Failure(ErrorCodes.NetworkError);
        }
    }

    public async Task<Result> ExecutePostRequest<T>(string url, T data)
    {
        try
        {
            using var response = await http.PostAsJsonAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                var serverResult = await response.Content.ReadFromJsonAsync<Result>();

                return serverResult ?? Result.Failure(ErrorCodes.NetworkError);
            }
            else
            {
                return Result.Failure(ErrorCodes.NetworkError);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Netzwerkfehler: {ex.Message}");
            return Result.Failure(ErrorCodes.NetworkError);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Allgemeiner Fehler: {ex.ToString()}");
            return Result.Failure(ErrorCodes.NetworkError);
        }
    }

    public async Task<Result> ExecutePutRequest<T>(string url, T data)
    {
        try
        {
            using var response = await http.PutAsJsonAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                var serverResult = await response.Content.ReadFromJsonAsync<Result>();

                return serverResult ?? Result.Failure(ErrorCodes.NetworkError);
            }
            else
            {
                return Result.Failure(ErrorCodes.NetworkError);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Netzwerkfehler: {ex.Message}");
            return Result.Failure(ErrorCodes.NetworkError);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Allgemeiner Fehler: {ex.ToString()}");
            return Result.Failure(ErrorCodes.NetworkError);
        }
    }

    public async Task<Result> ExecuteDeleteRequest<T>(string url, T id)
    {
        try
        {
            var value = await http.DeleteFromJsonAsync<Result>(url + $"/{id}");
            if (value == null)
            {
                return Result.Failure(ErrorCodes.NetworkError);
            }

            return value.IsSuccess ? Result.Success() : Result.Failure(value.ErrorCode);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Netzwerkfehler: {ex.Message}");
            return Result.Failure(ErrorCodes.NetworkError);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Allgemeiner Fehler: {ex.ToString()}");
            return Result.Failure(ErrorCodes.NetworkError);
        }
    }
}
