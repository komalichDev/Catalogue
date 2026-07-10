using System.Net.Http.Json;
using Common.Exception;
using Common.Types;
using Shared.Models;

namespace Client.Helpers;

public class HttpRequestExecuter
{
    public static async Task<QueryResult<T>> ExecuteGetRequests<T>(HttpClient httpClient, string url)
        where T : class
    {
        try
        {
            var value = await httpClient.GetFromJsonAsync<QueryResult<T>>(url);
            return (QueryResult<T>)(value ?? QueryResult<T>.Failure(ErrorCodes.NetworkError));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Netzwerkfehler: {ex.Message}");
            return (QueryResult<T>)QueryResult<T>.Failure(ErrorCodes.NetworkError);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Allgemeiner Fehler: {ex.ToString()}");
            return (QueryResult<T>)QueryResult<T>.Failure(ErrorCodes.NetworkError);
        }
    }

    public static async Task<Result> ExecutePostRequest<T>(HttpClient httpClient, string url, T data)
    {
        try
        {
            using var response = await httpClient.PostAsJsonAsync(url, data);

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

    public static async Task<Result> ExecutePutRequest<T>(HttpClient httpClient, string url, T data)
    {
        try
        {
            using var response = await httpClient.PutAsJsonAsync(url, data);

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

    public static async Task<Result> ExecuteDeleteRequest<T>(HttpClient httpClient, string url, T id)
    {
        try
        {
            var value = await httpClient.DeleteFromJsonAsync<Result>(url + $"/{id}");
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
